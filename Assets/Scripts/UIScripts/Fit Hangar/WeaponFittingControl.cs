using System;
using UnityEngine;

public class WeaponFittingControl : MonoBehaviour
{
    [SerializeField] private GameObject s1DropDown;
    [SerializeField] private GameObject s1Slots;
    [SerializeField] private GameObject s2DropDown;
    [SerializeField] private GameObject s2Slots;
    [SerializeField] private GameObject s4DropDown;
    [SerializeField] private GameObject s4Slots;

    private PlayerManager _playerManager;
    
    private ShipDataKeeper _shipData = new();

    private void Start()
    {
        WeaponFittingLayout();
    }


    private void WeaponFittingLayout()
    {
        _playerManager = PlayerManager.instance;
        
        // TODO - rocket

        if (_playerManager.GunPointS1.Length >= 1)
        {
            for (int i = 0; i < _playerManager.GunPointS1.Length; i++)
                Instantiate(s1DropDown, s1Slots.transform);
            
            DropDownS1[] dds1 = FindObjectsOfType<DropDownS1>();
            for (int i = 0; i < dds1.Length; i++)
                dds1[i].SetValue(_shipData.LoadShip().IDWeaponS1[i]);
        }
        else
            s1Slots.SetActive(false);

        if (_playerManager.GunPointS2.Length >= 1)
        {
            for (int i = 0; i < _playerManager.GunPointS2.Length; i++)
                Instantiate(s2DropDown, s2Slots.transform);

            DropDownS2[] dds2 = FindObjectsOfType<DropDownS2>();
            for (int i = 0; i < dds2.Length; i++)
                dds2[i].SetValue(_shipData.LoadShip().IDWeaponS2[i]);
        }
        else
            s2Slots.SetActive(false);
        
        if (_playerManager.GunPointS4.Length >= 1)
        {
            for (int i = 0; i < _playerManager.GunPointS4.Length; i++)
                Instantiate(s4DropDown, s4Slots.transform);

            DropDownS4[] dds4 = FindObjectsOfType<DropDownS4>();
            for (int i = 0; i < dds4.Length; i++)
                dds4[i].SetValue(_shipData.LoadShip().IDWeaponS4[i]);
        }
        else
            s4Slots.SetActive(false);
    }

    public void CollectWeaponData()
    {
        DropDownS1[] dds1 = FindObjectsOfType<DropDownS1>();
        DropDownS2[] dds2 = FindObjectsOfType<DropDownS2>();
        DropDownS4[] dds4 = FindObjectsOfType<DropDownS4>();
        
        int[] idWeaponS1 = new int[dds1.Length];
        int[] idWeaponS2 = new int[dds2.Length];
        int[] idWeaponS4 = new int[dds4.Length];

        for (int i = 0; i < dds1.Length; i++)
            idWeaponS1[i] = dds1[i].GetIDWeapon();
        for (int i = 0; i < dds2.Length; i++)
            idWeaponS2[i] = dds2[i].GetIDWeapon();
        for (int i = 0; i < dds4.Length; i++)
            idWeaponS4[i] = dds4[i].GetIDWeapon();
        
        _shipData.IDWeaponS1 = idWeaponS1;
        _shipData.IDWeaponS2 = idWeaponS2;
        _shipData.IDWeaponS4 = idWeaponS4;
        _shipData.IDShip = new ShipDataKeeper().LoadShip().IDShip; // bug - set 0 if not initialize
        _shipData.SaveShip(_shipData);
    }
}




















