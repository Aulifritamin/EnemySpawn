using UnityEngine;

public class Target : MonoBehaviour
{
    private Vector3 _startPosition;
    private Vector3 movementDirection = Vector3.back;

    private float speed = 2f;
    private float distance = 2f;

    public Transform Transform => transform;

    private void Start()
    {
        _startPosition = transform.position;
    }
    
    private void Update()
    {
        float walk = Mathf.PingPong(Time.time * speed, distance);

        transform.position = _startPosition + movementDirection.normalized * walk;
    }
}
