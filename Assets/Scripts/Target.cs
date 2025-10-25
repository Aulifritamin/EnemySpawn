using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private Transform _wayPointsContainer;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _minDistanceToWaypoint = 0.3f;
    [SerializeField] private Transform[] _targetPoints = new Transform[0];

    private float _sqrMinDistanceToWaypoint;
    private Vector3 _currentTarget;

    public Transform Transform => transform;

    private void Awake()
    {
        _sqrMinDistanceToWaypoint = _minDistanceToWaypoint * _minDistanceToWaypoint;
        ChangeDirection();
    }

    private void Update()
    {
        if (IsEnoughClose(transform.position, _currentTarget))
        {
            ChangeDirection();
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _currentTarget, _speed * Time.deltaTime);
        }
    }

    private bool IsEnoughClose(Vector3 start, Vector3 end)
    {
        return (end - start).sqrMagnitude <= _sqrMinDistanceToWaypoint;
    }

    private void ChangeDirection()
    {
        if (_targetPoints.Length == 0)
            return;

        int randomIndex = Random.Range(0, _targetPoints.Length);
        _currentTarget = _targetPoints[randomIndex].position;
    }

    [ContextMenu("Refresh Child Array")]
    private void RefreshChildArray()
    {
        int pointCount = _wayPointsContainer.childCount;
        _targetPoints = new Transform[pointCount];

        for (int i = 0; i < pointCount; i++)
            _targetPoints[i] = _wayPointsContainer.GetChild(i);
    }
}
