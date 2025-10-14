using UnityEngine;

public class Utilities: MonoBehaviour
{
    private const int FirstIndex = 0;

    public int GetRandomSpawnPoint(int listCount)
    {
        return Random.Range(FirstIndex, listCount);
    }

    public Vector3 GetRandomDirection()
    {
        float randomAngle = Random.Range(0f, 360f);
        Quaternion randomRotation = Quaternion.Euler(0f, randomAngle, 0f);       

        return randomRotation * Vector3.forward;
    }
}
