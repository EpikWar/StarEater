using UnityEngine;
using System;
public class ClosestEnemy : IComparable<ClosestEnemy>
{
    public GameObject Enemy { get; }
    public float Distance { get; }

    public ClosestEnemy(GameObject enemy, float distance)
    {
        Enemy = enemy;
        Distance = distance;
    }
    
    public int CompareTo(ClosestEnemy closestDistance)
    {
        return Distance.CompareTo(closestDistance.Distance);
    }
}
