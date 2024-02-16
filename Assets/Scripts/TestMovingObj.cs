using UnityEngine;

public class TestMovingObj : MonoBehaviour
{
    [SerializeField] private Vector3 rotateCenter;
    [SerializeField] private float moveSpeed;

    private Vector3 center;

    private void Start()
    {
        center = transform.position + rotateCenter;
    }

    private void FixedUpdate()
    {
        transform.RotateAround(center, Vector3.up, moveSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + rotateCenter, rotateCenter.magnitude);
    }
}
