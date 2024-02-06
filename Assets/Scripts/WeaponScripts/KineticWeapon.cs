using UnityEngine;

public class KineticWeapon : MonoBehaviour
{
    [Header("Weapon Properties")] 
    [SerializeField] protected float damagePerMinute;
    [SerializeField] protected Stat damage;
    [SerializeField] protected Stat shotPerMinute;
    [SerializeField] protected Stat muzzleVelocity;

    [Header("Initialization")] 
    [SerializeField] protected Transform muzzle;
    [SerializeField] protected GameObject bulletPrefab;
    private float shotInterval;
    private float timeSinceLastShot;


    private void Start()
    {
        shotInterval = 60 / shotPerMinute.GetValue();
    }

    private void FixedUpdate()
    {
        WeaponShot();
    }

    private void OnValidate()
    {
        damagePerMinute = damage.GetValue() * (shotPerMinute.GetValue() / 60);
    }


    private void WeaponShot()
    {
        timeSinceLastShot += Time.fixedDeltaTime;

        if (Input.GetMouseButton(0) && timeSinceLastShot >= shotInterval && GameManager.instance.IsRPGState()) {
            Instantiate(bulletPrefab, muzzle.position, transform.rotation).GetComponent<BulletControl>()
                .BulletShot(
                    transform.forward,
                    muzzleVelocity.GetValue(),
                    damage.GetValue(),
                    muzzle.position
                );

            timeSinceLastShot = 0f;
        }
    }
}