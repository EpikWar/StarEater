using UnityEngine;
using UnityEngine.UI;

public class WeaponCell : MonoBehaviour
{
    [SerializeField] private ScriptableWeapon[] weapon;
    [SerializeField] private Image weaponSprite;

    public void SetValue(int idWeapon)
    {
        foreach (ScriptableWeapon sw in weapon) {
            if (idWeapon == -1)
                Destroy(gameObject);
            
            if (sw.IDWeapon == idWeapon) //if "-1" - dont Instantiate and go next
                weaponSprite.GetComponent<Image>().sprite = sw.Sprite;
        }
    }
}
