using UIScripts;
using UnityEngine;

public class BeamLaserGun : LaserGun
{
    private void Update()
    {
        LaserGunShot();
    }
    
    private void LaserGunShot()
    {
        _lineRenderer.SetPosition(0, gunEnd.position);

        if (Input.GetMouseButton(0) && GameManager.instance.currentGameState == GameManager.instance._rpgState)
        {
            _lineRenderer.enabled = true;

            if (Physics.SphereCast(gunEnd.position, 5f, transform.forward, out var hit, maxDistantShot.GetValue()) &&
                GameUserInterface.instance.GetLockTarget() == hit.collider.gameObject)
            {
                _lineRenderer.SetPosition(1, hit.collider.transform.position);
            }
            else if (Physics.SphereCast(gunEnd.position, 0.1f, transform.forward, out hit, maxDistantShot.GetValue()) &&
                     hit.collider.tag.Equals("Enemy"))
            {
                _lineRenderer.SetPosition(1, hit.collider.transform.position);
            }
            else if (Physics.Raycast(gunEnd.position, transform.forward, out hit, maxDistantShot.GetValue()))
            {
                _lineRenderer.SetPosition(1, hit.point);
            }
            else
                _lineRenderer.SetPosition(1, gunEnd.position + transform.forward * maxDistantShot.GetValue());

            try
            {
                hit.collider.gameObject.TryGetComponent(out EnemyControl enemyControl);
                enemyControl.TakeDamage(damage.GetValue() * Time.deltaTime);
            }
            catch
            {
            }
        }
        else
        {
            _lineRenderer.enabled = false;
        }
    }
}
