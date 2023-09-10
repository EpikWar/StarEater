using UnityEngine;


public class LaserGun : MonoBehaviour
{
    [Header("Gun Properties")] 
    [SerializeField] protected Stat maxDistantShot;
    [SerializeField] protected Stat damage;

    [Header("Initialization")] 
    [SerializeField] protected Transform gunEnd;

    protected LineRenderer _lineRenderer;


    private void Awake()
    {
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
    }
    
    private void Start()
    {
        damage.SetDefaultValue(10);
        maxDistantShot.SetDefaultValue(90);
        
        damage.AddModifier(1);
        damage.AddModifier(-1);
    }

}
