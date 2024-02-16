using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [Header("Player Module Initialize")] 
    [SerializeField] private GameObject[] rocketPoint;
    [SerializeField] private GameObject[] gunPointS1;
    [SerializeField] private GameObject[] gunPointS2;
    [SerializeField] private GameObject[] gunPointS4;

    [Header("Module list")]
    [SerializeField] private ScriptableWeapon[] weaponS1;
    [SerializeField] private ScriptableWeapon[] weaponS2;
    [SerializeField] private ScriptableWeapon[] weaponS4;

    #region properties

    public GameObject[] RocketPoint
    {
        get => rocketPoint;
        set => rocketPoint = value;
    }

    public GameObject[] GunPointS1
    {
        get => gunPointS1;
        set => gunPointS1 = value;
    }

    public GameObject[] GunPointS2
    {
        get => gunPointS2;
        set => gunPointS2 = value;
    }

    public GameObject[] GunPointS4
    {
        get => gunPointS4;
        set => gunPointS4 = value;
    }

    #endregion
    

    private void OnEnable()
    {
        #region Singelton

        if (instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;

        #endregion
    }

    private void Start()
    {
        LoadWeapon();
    }
    
    
    public void LoadWeapon()
    {
        ShipDataKeeper shipData = new();

        // TODO - Instantiate rocket

        try //Destroying weapon 
        {
            for (var i = 0; i < gunPointS1.Length; i++)
                foreach (Transform child in gunPointS1[i].transform)
                    Destroy(child.gameObject);

            for (var i = 0; i < gunPointS2.Length; i++)
                foreach (Transform child in gunPointS2[i].transform)
                    Destroy(child.gameObject);

            for (var i = 0; i < gunPointS4.Length; i++)
                foreach (Transform child in gunPointS4[i].transform)
                    Destroy(child.gameObject);
        }
        catch (Exception e) {
            Debug.LogError($"Cant destroy weapon:\n{e}");
        }

        try //Instantiate s1
        {
            for (var i = 0; i < gunPointS1.Length; i++)
                if (shipData.LoadShip().IDWeaponS1[i] != -1) //if "-1" - dont Instantiate and go next
                    foreach (var weapon in weaponS1)
                        if (weapon.IDWeapon == shipData.LoadShip().IDWeaponS1[i]) {
                            Instantiate(weapon.Weapon, gunPointS1[i].transform);
                            break;
                        }
        }
        catch (Exception e) {
            Debug.LogError($"Trouble with instantiate weapon s1:\n{e}");
        }

        try //Instantiate s2
        {
            for (var i = 0; i < gunPointS2.Length; i++)
                if (shipData.LoadShip().IDWeaponS2[i] != -1) //if "-1" - dont Instantiate and go next
                    foreach (var weapon in weaponS2)
                        if (weapon.IDWeapon == shipData.LoadShip().IDWeaponS2[i]) {
                            Instantiate(weapon.Weapon, gunPointS2[i].transform);
                            break;
                        }
        }
        catch (Exception e) {
            Debug.LogError($"Trouble with instantiate weapon s2:\n{e}");
        }

        try //Instantiate s4
        {
            for (var i = 0; i < gunPointS4.Length; i++)
                if (shipData.LoadShip().IDWeaponS4[i] != -1) //if "-1" - dont Instantiate and go next
                    foreach (var weapon in weaponS4)
                        if (weapon.IDWeapon == shipData.LoadShip().IDWeaponS4[i]) {
                            Instantiate(weapon.Weapon, gunPointS4[i].transform);
                            break;
                        }
        }
        catch (Exception e) {
            Debug.LogError($"Trouble with instantiate weapon s4:\n{e}");
        }
    }

}