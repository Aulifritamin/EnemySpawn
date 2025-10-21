using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private Utilities _utilities;
    [SerializeField] private Target _target;

    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private int _poolSize = 10;
    [SerializeField] private int _maxPoolSize = 20;

    private Color _enemyColor;
    private Transform _spawnPoint;

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

        _enemyColor = _utilities.GetRandomColor();
        _spawnPoint = GetComponent<Transform>();
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
        Vector3 newDirection = _utilities.GetRandomDirection();

        enemy.gameObject.SetActive(true);
        enemy.SetSpawnPoint(_spawnPoint);
        enemy.SetTarget(_target);
        enemy.SetColor(_enemyColor);
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
