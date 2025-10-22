using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float TimeToLive = 7f;

    private Target _target;
    private Rigidbody _rigidBody;
    private Renderer _renderer;
    private Vector3 _zeroVelocity = Vector3.zero;
    private bool _isActive = false;

    private Coroutine _despawnCoroutine;

    public event System.Action<Enemy> Despawned;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (_isActive)
        {
            Vector3 direction = (_target.Transform.position - transform.position).normalized;
            Vector3 newPosition = transform.position + direction * _speed * Time.deltaTime;
            
            _rigidBody.MovePosition(newPosition);
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
        _rigidBody.linearVelocity = _zeroVelocity;
        _rigidBody.angularVelocity = _zeroVelocity;
    }

    public void SetTarget(Target target)
    {
        _target = target;
    }

    public void SetSpawnPoint(Transform spawnPoint)
    {
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
    }

    public void SetColor(Color color)
    {
        _renderer.material.color = color;
    }

    private IEnumerator DespawningTimer()
    {
        yield return new WaitForSeconds(TimeToLive);
        Despawned?.Invoke(this);
    }
}