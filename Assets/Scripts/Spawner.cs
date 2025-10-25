using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<SpawnPoint> _spawnPoint;

    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private int _poolSize = 10;
    [SerializeField] private int _maxPoolSize = 20;

    private Dictionary<Enemy, ObjectPool<Enemy>> _enemyPool = new Dictionary<Enemy, ObjectPool<Enemy>>();

    private void Awake()
    {
        InitateEnemyPool();
    }

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private void InitateEnemyPool()
    {
        foreach (var spawnPoint in _spawnPoint)
        {
            if (spawnPoint.EnemyPrefab != null && !_enemyPool.ContainsKey(spawnPoint.EnemyPrefab))
            {
                Enemy prefab = spawnPoint.EnemyPrefab;

                ObjectPool<Enemy> pool = new ObjectPool<Enemy>(
                    () => CreateEnemy(prefab),
                    actionOnGet: ActivateEnemyFromPool,
                    actionOnRelease: ReleaseCleanUp,
                    actionOnDestroy: (enemy) => Destroy(enemy.gameObject),
                    collectionCheck: false,
                    _poolSize,
                    _maxPoolSize);

                _enemyPool.Add(prefab, pool);
            }
        }
    }
    private IEnumerator SpawnRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(_spawnInterval);

        while (enabled)
        {
            yield return delay;
            int spawnIndex = GetRandomIndex(_spawnPoint.Count);
            SpawnPoint selectedSpawnPoint = _spawnPoint[spawnIndex];
            Enemy prefabToSpawn = selectedSpawnPoint.EnemyPrefab;

            if (prefabToSpawn != null && _enemyPool.TryGetValue(prefabToSpawn, out ObjectPool<Enemy> pool))
            {
                Enemy enemy = pool.Get();
                enemy.Init(selectedSpawnPoint.AssignedTarget, selectedSpawnPoint.Transform, selectedSpawnPoint.EnemyColor, pool);
            }
        }
    }

    private Enemy CreateEnemy(Enemy prefab)
    {
        Enemy newEnemy = Instantiate(prefab);
        newEnemy.gameObject.SetActive(false);
        return newEnemy;
    }

    private void ActivateEnemyFromPool(Enemy enemy)
    {
        enemy.Despawned += ReturnToPool;
        enemy.gameObject.SetActive(true);
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
        enemy.ReturnToPool();
    }

    private int GetRandomIndex(int listCount)
    {
        return Random.Range(0, listCount);
    }
}
