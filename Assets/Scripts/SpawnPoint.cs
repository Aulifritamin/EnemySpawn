using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private Target _assignedTarget;
    [SerializeField] private Color _enemyColor;
    
    public Enemy EnemyPrefab => _enemyPrefab;
    public Target AssignedTarget => _assignedTarget;
    public Transform Transform => transform;
    public Color EnemyColor => _enemyColor;
}
