using TMPro;
using UnityEngine;

public abstract class DropDownAbstract : MonoBehaviour
{
    [SerializeField] protected ScriptableWeapon[] weapon;

    private TMP_Dropdown _dropdown;

    private void OnEnable()
    {
        _dropdown = GetComponent<TMP_Dropdown>();

        _dropdown.options.Clear();
        foreach (ScriptableWeapon i in weapon)
            _dropdown.options.Add(new TMP_Dropdown.OptionData(i.name, i.Sprite));
    }

    private void Start()
    {
        _dropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(); });
    }


    private void DropdownValueChanged()
    {
        FindObjectOfType<WeaponFittingControl>().CollectWeaponData();
        PlayerManager.instance.LoadWeapon();
    }

    public int GetIDWeapon()
    {
        int i = 0;

        foreach (ScriptableWeapon sw in weapon) {
            if (i == _dropdown.value)
                return sw.IDWeapon;
            i++;
        }

        return -1;
    }

    public void SetValue(int idWeapon)
    {
        int i = 0;

        foreach (ScriptableWeapon sw in weapon) {
            if (idWeapon != -1 && sw.IDWeapon == idWeapon) //if "-1" - dont Instantiate and go next
                _dropdown.value = i;
            i++;
        }
    }
}
