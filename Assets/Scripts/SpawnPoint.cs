using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private Target _assignedTarget;
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private Color _enemyColor;
    
    public Target AssignedTarget => _assignedTarget;
    public Enemy EnemyPrefab => _enemyPrefab;
    public Transform Transform => transform;
    public Color EnemyColor => _enemyColor;
}
