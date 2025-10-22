using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Target : MonoBehaviour
{
    public Transform Transform => transform;
    [SerializeField] public Transform WayPoints;

    private List<WayPoint> _directions;
    [SerializeField] private float _speed = 2f;

    private Vector3 _currentDirection;

    private void Awake()
    {
        _directions = WayPoints.GetComponentsInChildren<WayPoint>().ToList(); 
        ChangeDirection();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _currentDirection) < 0.1f)
        {
            ChangeDirection();
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _currentDirection, _speed * Time.deltaTime);
        }
    }

    private void ChangeDirection()
    {
        if (_directions.Count == 0)
            return;

        int randomIndex = Random.Range(0, _directions.Count);
        _currentDirection = _directions[randomIndex].Position;
    }
}
