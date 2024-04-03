using UIScripts;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [Header("Enemy properties")] 
    [SerializeField] private float maxHealPoint;
    public float CurrentHealPoint { get; set; } //TODO Stat variable
    
    private Outline outlineTarget;


    private void Start()
    {
        outlineTarget = gameObject.GetComponent<Outline>();

        CurrentHealPoint = maxHealPoint;
    }

    private void Update()
    {
        OutlineTarget();
    }

    private void OutlineTarget()
    {
        outlineTarget.enabled = GameUserInterface.instance.GetLockTarget() == gameObject;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealPoint -= damage;

        if (CurrentHealPoint < 1f) Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}