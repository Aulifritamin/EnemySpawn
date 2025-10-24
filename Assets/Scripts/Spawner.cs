using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;

    [SerializeField] private List<SpawnPoint> _spawnPoint;

    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private int _poolSize = 10;
    [SerializeField] private int _maxPoolSize = 20;

    private ObjectPool<Enemy> _enemyPool;

    private void Awake()
    {
        _enemyPool = new ObjectPool<Enemy>(
            () => CreateEnemy(_enemyPrefab),
            actionOnGet: ActivateEnemyFromPool,
            actionOnRelease: ReleaseCleanUp,
            null,
            false,
            _poolSize,
            _maxPoolSize);
    }

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        WaitForSeconds timer = new WaitForSeconds(_spawnInterval);

        while (enabled)
        {
            yield return timer;
            _enemyPool.Get();
        }
    }

    private Enemy CreateEnemy(Enemy prefab)
    {
        Enemy newEnemy = Instantiate(prefab);
        newEnemy.gameObject.SetActive(false);
        newEnemy.Despawned += ReturnToPool;
        return newEnemy;
    }

    private void ActivateEnemyFromPool(Enemy enemy)
    {
        int spawnIndex = GetRandomIndex(_spawnPoint.Count);
        SpawnPoint selectedSpawnPoint = _spawnPoint[spawnIndex];
        enemy.gameObject.SetActive(true);
        enemy.Init(selectedSpawnPoint.AssignedTarget, selectedSpawnPoint.Transform, selectedSpawnPoint.EnemyColor);
        enemy.ActivateWalk();
    }

    private void ReleaseCleanUp(Enemy enemy)
    {
        enemy.Despawned -= ReturnToPool;
        enemy.ResetState();
        enemy.gameObject.SetActive(false);
    }

    private void ReturnToPool(Enemy enemy)
    {
        _enemyPool.Release(enemy);
    }

    private int GetRandomIndex(int listCount)
    {
        return Random.Range(0, listCount);
    }
}
