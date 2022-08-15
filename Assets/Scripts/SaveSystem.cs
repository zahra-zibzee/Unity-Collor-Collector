using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    public static void SaveState(int lvl, int[] scenes)
    {
        BinaryFormatter formatter = new();
        string path = Application.persistentDataPath + "/playerState.fun";
        FileStream stream = new(path, FileMode.Create);
        PlayerData data = new(lvl, scenes);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadState()
    {
        string path = Application.persistentDataPath + "/playerState.fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new();
            FileStream stream = new(path, FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }
}
