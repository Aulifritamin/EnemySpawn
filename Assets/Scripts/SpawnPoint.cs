using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private Target _assignedTarget;
    [SerializeField] private Color _enemyColor;
    
    public Target AssignedTarget => _assignedTarget;
    public Transform Transform => transform;
    public Color EnemyColor => _enemyColor;
}
