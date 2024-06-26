using System;
using System.IO;
using UnityEngine;

[Serializable]
public class DataKeeper
{
    public int idShipSaveSlot;
    // lvl exp etc.

    private string path = "";
    private string data = "";


#region properties

    public int IDShipSaveSlot
    {
        get => idShipSaveSlot;
        set => idShipSaveSlot = value;
    }

#endregion

    public void Save(DataKeeper dataKeeper)
    {
        data = JsonUtility.ToJson(dataKeeper, true);

        try {
            path = Path.Combine(Application.persistentDataPath, "data\\GameData.json");
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);

            using FileStream stream = new (path, FileMode.Create);
            using StreamWriter writer = new (stream);
            writer.Write(data);
        }
        catch (Exception e) {
            Console.WriteLine($"Problem with saving data to {path}\n{e}");
        }
    }

    public DataKeeper Load()
    {
        try {
            path = Path.Combine(Application.persistentDataPath, "data\\GameData.json");

            using FileStream stream = new (path, FileMode.Open);
            using StreamReader reader = new (stream);
            data = reader.ReadToEnd();

            DataKeeper dataKeeper = JsonUtility.FromJson<DataKeeper>(data);
            return dataKeeper;
        }
        catch (Exception e) {
            Console.WriteLine($"Problem with loading data from {path}\n{e}");
        }

        return null;
    }
}