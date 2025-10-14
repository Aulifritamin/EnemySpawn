using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private List<Transform> _spawnPoint;
    [SerializeField] private Utilities _utilities;

    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private int _poolSize = 10;
    [SerializeField] private int _maxPoolSize = 20;

    private ObjectPool<Enemy> _enemyPool;

    private void Awake()
    {
        _enemyPool = new ObjectPool<Enemy>(
            CreateEnemy,
            actionOnGet: GettingFromPool,
            actionOnRelease: ReleasingCleanUp,
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

    private Enemy CreateEnemy()
    {
        Enemy newEnemy = Instantiate(_enemyPrefab);
        newEnemy.gameObject.SetActive(false);
        return newEnemy;
    }

    private void GettingFromPool(Enemy enemy)
    {
        Transform randomPoint = _spawnPoint[_utilities.GetRandomSpawnPoint(_spawnPoint.Count)];
        Vector3 newDirection = _utilities.GetRandomDirection();

        enemy.gameObject.SetActive(true);
        enemy.SetDirection(newDirection);
        enemy.SetSpawnPoint(randomPoint);
        enemy.OnDespawn += ReturningToPool;
        enemy.ActivateWalk();
    }

    private void ReleasingCleanUp(Enemy enemy)
    {
        enemy.OnDespawn -= ReturningToPool;
        enemy.ResetState();
        enemy.gameObject.SetActive(false);
    }

    private void ReturningToPool(Enemy enemy)
    {
        _enemyPool.Release(enemy);
    }
}
