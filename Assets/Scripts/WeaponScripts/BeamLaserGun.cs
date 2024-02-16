using System.Collections.Generic;
using UIScripts;
using UnityEngine;

public class BeamLaserGun : MonoBehaviour
{
    [Header("Weapon Properties")] 
    [SerializeField] protected float damagePerMinute;
    [SerializeField] protected Stat damage;
    [SerializeField] protected Stat maxDistantShot;
    [SerializeField] protected float coneTargetLock;
    [SerializeField] protected float coneTarget;
    
    [Header("Initialization")] 
    [SerializeField] protected Transform muzzle;
    private LayerMask excludePlayer;
    
    private List<ClosestEnemy> enemies = new ();
    private bool waeponFire;
    private Vector3 endPoint;

    private LineRenderer _lineRenderer;


    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        excludePlayer = ~LayerMask.GetMask("Player");
    }

    private void Update()
    {
        WeaponShotRender();

        //Only for test
        DrawConeShape(muzzle.transform.position, transform.forward, coneTargetLock, maxDistantShot.GetValue());
        DrawConeShape(muzzle.transform.position, transform.forward, coneTarget, maxDistantShot.GetValue());
    }

    private void FixedUpdate()
    {
        WeaponShot();
    }

    private void OnValidate()
    {
        damagePerMinute = damage.GetValue() * 75 * 60; //75 - bc we have 75 physics ticks in second
    }

    private void WeaponShotRender()
    {
        if (waeponFire) {
            _lineRenderer.enabled = true;
            _lineRenderer.SetPosition(0, muzzle.position);
            _lineRenderer.SetPosition(1, endPoint);
        }
        else {
            _lineRenderer.enabled = false;
        }
    }

    private void WeaponShot()
    {
        if (Input.GetMouseButton(0) && GameManager.instance.IsRPGState()) {
            //Check if Target enemy in Cone
            if (TargetEnemyInCone()) return;

            //Check if any enemy in Cone
            if (AnyEnemyInCone()) return;

            //If no enemy in cone
            NoEnemyInCone();
        }
        else {
            waeponFire = false;
        }


        bool TargetEnemyInCone()
        {
            float coneRadius = LawOfSines(maxDistantShot.GetValue(), 90f, coneTargetLock / 2);
            Vector3 coneDimension = new(coneRadius, coneRadius, maxDistantShot.GetValue() / 2);
            Vector3 conePosition = muzzle.position + transform.forward * maxDistantShot.GetValue() / 2;

            //Search enemy in Cone
            enemies = OverlapConeCollider(
                conePosition,
                coneDimension * 1.1f,
                transform.rotation,
                coneTargetLock,
                maxDistantShot.GetValue(),
                muzzle
            );

            foreach (ClosestEnemy enemy in enemies)
                if (enemy.Enemy == GameUserInterface.instance.GetLockTarget()) {
                    Vector3 direction = enemy.Enemy.transform.position - muzzle.position;
                    Physics.Raycast(muzzle.position, direction, out var hit, direction.magnitude, excludePlayer);

                    waeponFire = true;
                    endPoint = hit.point;

                    if (hit.collider.TryGetComponent(out EnemyControl enemyControl)) {
                        enemyControl.TakeDamage(damage.GetValue());
                        return true;
                    }

                    return false;
                }

            return false;
        }

        bool AnyEnemyInCone()
        {
            float coneRadius = LawOfSines(maxDistantShot.GetValue(), 90f, coneTarget / 2);
            Vector3 coneDimension = new(coneRadius, coneRadius, maxDistantShot.GetValue() / 2);
            Vector3 conePosition = muzzle.position + transform.forward * maxDistantShot.GetValue() / 2;

            //Search enemy in Cone
            enemies = OverlapConeCollider(conePosition,
                coneDimension * 1.1f,
                transform.rotation,
                coneTarget,
                maxDistantShot.GetValue(),
                muzzle
            );

            enemies.Sort();

            foreach (ClosestEnemy enemy in enemies) {
                Vector3 direction = enemy.Enemy.transform.position - muzzle.position;
                Physics.Raycast(muzzle.position, direction, out var hit, direction.magnitude, excludePlayer);
                if (hit.collider.gameObject == enemy.Enemy) {
                    waeponFire = true;
                    endPoint = hit.point;

                    hit.collider.TryGetComponent(out EnemyControl enemyControl);
                    enemyControl.TakeDamage(damage.GetValue());
                    return true;
                }
            }

            return false;
        }

        void NoEnemyInCone()
        {
            if (Physics.Raycast(muzzle.position, transform.forward, out var hit, 
                    maxDistantShot.GetValue(), excludePlayer)) {
                waeponFire = true;
                _lineRenderer.SetPosition(1, hit.point);
            }
            else {
                waeponFire = true;
                endPoint = muzzle.position + transform.forward * maxDistantShot.GetValue();
            }
        }
    }

    private static List<ClosestEnemy> OverlapConeCollider(Vector3 colliderPos, Vector3 colliderDimension,
        Quaternion quaternion, float coneAngle, float coneHeight, Transform coneApex)
    {
        //all hail cthulhu
        List<ClosestEnemy> objectsInSight = new();
        Collider[] prey = new Collider[20];

        int count = Physics.OverlapBoxNonAlloc(
            colliderPos,
            colliderDimension,
            prey,
            quaternion,
            LayerMask.GetMask("Enemy")
        );

        if (count > 0)
            for (int i = 0; i < count; i++) {
                float distanceToPray = Vector3.Distance(coneApex.position, prey[i].transform.position);

                Vector3 preyClosestPoint = prey[i].ClosestPoint(coneApex.position);

                if (coneHeight < Vector3.Distance(preyClosestPoint, coneApex.position)) continue;

                Vector3 pointForward = coneApex.forward * distanceToPray + coneApex.position;
                preyClosestPoint = prey[i].ClosestPoint(pointForward);

                float pointForwardAngle = AngleBetweenVector(
                    pointForward - preyClosestPoint, pointForward - coneApex.position);

                float preyDistance = Vector3.Distance(preyClosestPoint, pointForward);
                float coneDistance = LawOfSines(distanceToPray, pointForwardAngle, coneAngle / 2);

                if (preyDistance < coneDistance || preyDistance <= 0)
                    objectsInSight.Add(new ClosestEnemy(prey[i].gameObject, distanceToPray));
            }

        return objectsInSight;
    }

    private static float AngleBetweenVector(Vector3 AB, Vector3 BC)
    {
        float rad = Mathf.Acos(Vector3.Dot(AB, BC) / (AB.magnitude * BC.magnitude));
        return Mathf.Rad2Deg * rad;
    }

    private static float LawOfSines(float AB, float ABC, float BAC)
    {
        float ABCRad = Mathf.Deg2Rad * ABC;
        float BACRad = Mathf.Deg2Rad * BAC;

        float BC = AB * Mathf.Sin(BACRad) / Mathf.Sin(ABCRad + BACRad);

        return BC;
    }

    private void DrawConeShape(Vector3 origin, Vector3 direction, float coneAngle, float coneLength)
    {
        float halfAngle = coneAngle / 2f;
        Quaternion leftRotation = Quaternion.AngleAxis(-halfAngle, Vector3.up);
        Quaternion rightRotation = Quaternion.AngleAxis(halfAngle, Vector3.up);

        Vector3 leftDirection = leftRotation * direction;
        Vector3 rightDirection = rightRotation * direction;

        // Draw the lines representing the cone edges
        Debug.DrawRay(origin, leftDirection * coneLength, Color.green);
        Debug.DrawRay(origin, rightDirection * coneLength, Color.green);
    }
}