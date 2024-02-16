using System.Collections.Generic;
using UnityEngine;

public class PooledObject
{
    public string pooledObjectName;
    public List<GameObject> inactiveObject = new ();
}
