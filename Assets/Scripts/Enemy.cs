using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float TimeToLive = 15f;

    private Rigidbody _rigidBody;
    private Vector3 _direction;
    private Vector3 _initialPosition = Vector3.zero;
    private bool _isActive = false;

    private Coroutine _despawnCoroutine;
    
    public event System.Action<Enemy> OnDespawn;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_isActive)
        {
            _rigidBody.MovePosition(transform.position + _direction * _speed * Time.deltaTime);
        }
    }

    public void ActivateWalk()
    {
        _isActive = true;
        _despawnCoroutine = StartCoroutine(DespawningTimer());
    }

    public void ResetState()
    {
        if (_despawnCoroutine != null)
        {
            StopCoroutine(_despawnCoroutine);
            _despawnCoroutine = null;
        }
        _isActive = false;
        _rigidBody.linearVelocity = _initialPosition;
        _rigidBody.angularVelocity = _initialPosition;
    }

    public void SetDirection(Vector3 setDirection)
    {
        _direction = setDirection.normalized;
    }

    public void SetSpawnPoint(Transform spawnPoint)
    {
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
    }

    private IEnumerator DespawningTimer()
    {
        yield return new WaitForSeconds(TimeToLive);
        OnDespawn?.Invoke(this);
    }
}