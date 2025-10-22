using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private Target _assignedTarget;
    private Enemy _assignedEnemy;
    private Color _enemyColor;

    private void Start()
    {
        _enemyColor = GetRandomColor();
    }

    public void SetTarget(Target target)
    {
        _assignedTarget = target;
    }

    public Enemy GetAssignedEnemy()
    {
        return _assignedEnemy;
    }

    public void SetAssignedEnemy(Enemy enemy)
    {
        _assignedEnemy = enemy;
    }

    public Vector3 GetSpawnPosition()
    {
        return transform.position;
    }

    public void ChangeAtributes(Enemy enemy)
    {
        enemy.SetTarget(_assignedTarget);
        enemy.SetSpawnPoint(transform);
        enemy.SetColor(_enemyColor);
    }

    private Color GetRandomColor()
    {
        return Random.ColorHSV();
    }   
}
