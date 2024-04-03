using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Playable Ship")]
public class PlayableShip : ScriptableObject
{
    [SerializeField] private int idShip;
    [SerializeField] private GameObject ship;
    [SerializeField] private Sprite sprite;

    
#region properties

    public int IDShip
    {
        get => idShip;
        set => idShip = value;
    }

    public GameObject Ship
    {
        get => ship;
        set => ship = value;
    }

    public Sprite Sprite
    {
        get => sprite;
        set => sprite = value;
    }

#endregion
    
}
