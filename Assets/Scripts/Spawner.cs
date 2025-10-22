using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;

    [SerializeField] private List<SpawnPoint> _spawnPoint;
    [SerializeField] private List<Target> _targets;

    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private int _poolSize = 10;
    [SerializeField] private int _maxPoolSize = 20;

    private ObjectPool<Enemy> _enemyPool;

    private void Awake()
    {
        _enemyPool = new ObjectPool<Enemy>(
            CreateEnemy,
            actionOnGet: GetFromPool,
            actionOnRelease: ReleaseCleanUp,
            null,
            false,
            _poolSize,
            _maxPoolSize);

        AddTargetToSpawnPoints(_spawnPoint);
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

    private Enemy CreateEnemy()
    {
        Enemy newEnemy = Instantiate(_enemyPrefab);
        newEnemy.gameObject.SetActive(false);
        return newEnemy;
    }

    private void GetFromPool(Enemy enemy)
    {
        int spawnIndex = GetRandomIndex(_spawnPoint.Count);
        SpawnPoint selectedSpawnPoint = _spawnPoint[spawnIndex];

        enemy.gameObject.SetActive(true);
        selectedSpawnPoint.ChangeAtributes(enemy);
        enemy.Despawned += ReturnToPool;
        enemy.ActivateWalk();
    }

    private void ReleaseCleanUp(Enemy enemy)
    {
        enemy.Despawned -= ReturnToPool;
        enemy.ResetState();
        enemy.gameObject.SetActive(false);
    }

    private void AddTargetToSpawnPoints(List<SpawnPoint> spawnPoints)
    {
        if (_targets.Count == 0)
        {
            return;
        }

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            spawnPoints[i].SetTarget(_targets[i % _targets.Count]);
        }
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
