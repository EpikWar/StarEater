using System;
using UnityEngine;
using UnityEngine.UI;

public class RPGShipInfo : MonoBehaviour
{
    [Header("Ship Info")] 
    [SerializeField] private Slider afterburnerSlider;

    [Header("Weapon info")] 
    [SerializeField] private GameObject weaponLayout;
    [SerializeField] private GameObject weaponCell;

    private PlayerManager _playerManager;
    
    private ShipDataKeeper _shipData = new();


    private void Start()
    {
        afterburnerSlider = GetComponentInChildren<Slider>();

        WeaponInfoLayout();
    }

    public void AfterburnerSlider()
    {
        afterburnerSlider.value = RPGPlayerControl.instance.GetAfterburnerValue *
                                  (100 / RPGPlayerControl.instance.GetAfterburnerMaxValue);
    }

    private void WeaponInfoLayout()
    {
        _playerManager = PlayerManager.instance;

        if (_playerManager.GunPointS1.Length >= 1 || _playerManager.GunPointS2.Length >= 1 ||
            _playerManager.GunPointS4.Length >= 1) 
        {
            for (int i = 0; i < _playerManager.GunPointS1.Length; i++)
                Instantiate(weaponCell, weaponLayout.transform);
            for (int i = 0; i < _playerManager.GunPointS2.Length; i++)
                Instantiate(weaponCell, weaponLayout.transform);
            for (int i = 0; i < _playerManager.GunPointS4.Length; i++)
                Instantiate(weaponCell, weaponLayout.transform);

            WeaponCell[] wCell = FindObjectsOfType<WeaponCell>();//TODO - Remove FindObjectsOfType
            Array.Reverse(wCell);
            
            for (int i = 0; i < wCell.Length; i++)
                wCell[i].SetValue(_shipData.LoadShip().IDWeaponS1[i]);
        }
    }
}






