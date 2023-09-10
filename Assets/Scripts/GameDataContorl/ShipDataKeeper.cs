using System;
using System.IO;
using UnityEngine;

[Serializable]
public class ShipDataKeeper
{
    public int idShip;
    public int[] idWeaponS1;
    public int[] idWeaponS2;
    public int[] idWeaponS4;

    private string fullPath = "";
    private string data = "";

    public ShipDataKeeper()
    {
    }

    #region properties
    public int IDShip
    {
        get => idShip;
        set => idShip = value;
    }
    
    public int[] IDWeaponS1
    {
        get => idWeaponS1;
        set => idWeaponS1 = value;
    }
    
    public int[] IDWeaponS2
    {
        get => idWeaponS2;
        set => idWeaponS2 = value;
    }
    public int[] IDWeaponS4
    {
        get => idWeaponS4;
        set => idWeaponS4 = value;
    }
    #endregion

    public void SaveShip(ShipDataKeeper shipDataKeeper)
    {
        data = JsonUtility.ToJson(shipDataKeeper, true);

        try
        {
            fullPath = Path.Combine(Application.persistentDataPath, "data\\Saved Ships", new DataKeeper().Load().IDShipSaveSlot.ToString());
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

            using FileStream stream = new FileStream(fullPath, FileMode.Create);
            using StreamWriter writer = new StreamWriter(stream);
            writer.Write(data);
        }
        catch (Exception e)
        {
            Debug.LogError($"Problem with saving data to {fullPath}\n{e}");
        }
    }

    public ShipDataKeeper LoadShip()
    {
        try
        {
            fullPath = Path.Combine(Application.persistentDataPath, "data\\Saved Ships", new DataKeeper().Load().IDShipSaveSlot.ToString());
    
            using FileStream stream = new FileStream(fullPath, FileMode.Open);
            using StreamReader reader = new StreamReader(stream);
            data = reader.ReadToEnd();
    
            ShipDataKeeper shipDataKeeper = JsonUtility.FromJson<ShipDataKeeper>(data);
            return shipDataKeeper;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Problem with loading data from {fullPath}\n{e}");
        }
    
        return null;
    }
}
