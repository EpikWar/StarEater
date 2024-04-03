using UnityEngine;

public class BulletControl : MonoBehaviour
{
    [SerializeField] private float drag;
    
    private float damage;

    private LayerMask excludePlayer;
    private Vector3 previousPos;
    private Vector3 currentPos;

    private Rigidbody _rigidbody;


    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _rigidbody.drag = drag;
        
        excludePlayer = ~LayerMask.GetMask("Player");
    }

    private void FixedUpdate()
    {
        BulletHitAndDestroy();
    }


    private void BulletHitAndDestroy()
    {
        currentPos = transform.position;

        //Improvement collision
        Vector3 direction = currentPos - previousPos;
        if (Physics.Raycast(previousPos, direction, out var hit, direction.magnitude, excludePlayer)) {
            if (hit.collider.TryGetComponent(out EnemyControl enemyControl)) {
                enemyControl.TakeDamage(damage);
            }

            PoolManager.ReturnToPool(gameObject);
            ResetBullet();
        }

        //Slow down old bullet
        if ((currentPos - previousPos).magnitude < 1f) {
            transform.position += Vector3.down * .1f;
            _rigidbody.drag += .1f;
        }

        //Destroy slowed bullet
        if ((currentPos - previousPos).magnitude < 0.2f) {
            PoolManager.ReturnToPool(gameObject);
            ResetBullet();
            print("Bullet destroying by min speed " + (currentPos - previousPos).magnitude);
        }

        previousPos = currentPos;
    }

    private void ResetBullet()
    {
        _rigidbody.drag = drag;
        _rigidbody.velocity = Vector3.zero;
    }

    public void BulletShot(Vector3 direction, float muzzleVelocity, float damage, Vector3 muzzlePos)
    {
        _rigidbody.AddForce(direction * muzzleVelocity);

        this.damage = damage;
        previousPos = muzzlePos;
    }
}