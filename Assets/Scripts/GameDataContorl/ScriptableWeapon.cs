using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Scriptable Weapon")]
public class ScriptableWeapon : ScriptableObject
{
    [SerializeField] private int idWeapon;
    [SerializeField] private GameObject weapon;
    [SerializeField] private Sprite sprite;

#region properties

    public int IDWeapon
    {
        get => idWeapon;
        set => idWeapon = value;
    }

    public GameObject Weapon
    {
        get => weapon;
        set => weapon = value;
    }

    public Sprite Sprite
    {
        get => sprite;
        set => sprite = value;
    }

#endregion
    
}
