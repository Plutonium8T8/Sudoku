using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveGame(SudokuMatrix statSystem)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/data.save";

        FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);

        formatter.Serialize(stream, statSystem);

        stream.Close();
    }

    public static SudokuMatrix LoadGame()
    {
        string path = Application.persistentDataPath + "/data.save";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);

            SudokuMatrix statSystem = formatter.Deserialize(stream) as SudokuMatrix;

            stream.Close();

            return statSystem;
        }
        else
        {
            return new SudokuMatrix(0);
        }
    }
}