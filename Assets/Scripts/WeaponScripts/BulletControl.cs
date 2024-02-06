using UnityEngine;

public class BulletControl : MonoBehaviour
{
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
        excludePlayer = ~LayerMask.GetMask("Player");
    }

    private void FixedUpdate()
    {
        BulletHitAndDestroy2();
    }


    private void BulletHitAndDestroy2()
    {
        currentPos = transform.position;

        //Improvement collision
        var direction = currentPos - previousPos;
        if (Physics.Raycast(previousPos, direction, out var hit, direction.magnitude, excludePlayer)) {
            if (hit.collider.TryGetComponent(out EnemyControl enemyControl)) enemyControl.TakeDamage(damage);

            Destroy(gameObject);
        }

        //Slow down old bullet
        if ((currentPos - previousPos).magnitude < 1f) {
            transform.position += Vector3.down * .1f;
            _rigidbody.drag += .1f;
        }

        //Destroy slowed bullet
        if ((currentPos - previousPos).magnitude < 0.2f) {
            Destroy(gameObject);
            print("Bullet destroying by min speed");
        }

        previousPos = currentPos;
    }

    public void BulletShot(Vector3 direction, float muzzleVelocity, float damage, Vector3 muzzlePos)
    {
        _rigidbody.AddForce(direction * muzzleVelocity);

        this.damage = damage;
        previousPos = muzzlePos;
    }
}