using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    private static readonly string SaveFolderPath;
    private static readonly string SaveFileName = "savedSession.dat";
    private static bool isInitialized = false;

    static SaveSystem()
    {
        SaveFolderPath = Path.Combine(Application.dataPath, "SaveSystem", "SaveFile");
        SaveFolderPath = Path.GetFullPath(SaveFolderPath);
        Initialize();
    }

    private static void Initialize()
    {
        if (isInitialized) return;

        if (!Directory.Exists(SaveFolderPath))
        {
            Directory.CreateDirectory(SaveFolderPath);
        }

        Debug.Log($"SaveSystem initialized. Save folder: {SaveFolderPath}");
        isInitialized = true;
    }

    public static void SaveGame(SaveFile session)
    {
        Initialize();

        string fullPath = Path.Combine(SaveFolderPath, SaveFileName);
        using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fileStream, session);
        }

        Debug.Log($"Session saved to: {fullPath}");
    }

    public static SaveFile LoadGame()
    {
        Initialize();

        string fullPath = Path.Combine(SaveFolderPath, SaveFileName);

        if (File.Exists(fullPath))
        {
            using (FileStream fileStream = new FileStream(fullPath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                SaveFile session = (SaveFile)formatter.Deserialize(fileStream);
                Debug.Log("Session loaded successfully.");
                return session;
            }
        }
        else
        {
            Debug.LogWarning("Save file not found!");
            return null;
        }
    }
}


//EN JSON
//public static class SaveSystem
//{
//    private static readonly string SaveFolderPath;
//    private static readonly string SaveFileName = "savedSession.json";
//    private static bool isInitialized = false;

//    static SaveSystem()
//    {
//        SaveFolderPath = Path.Combine(Application.dataPath, "SaveSystem", "SaveFile");
//        SaveFolderPath = Path.GetFullPath(SaveFolderPath);
//        Initialize();
//    }

//    private static void Initialize()
//    {
//        if (isInitialized) return;

//        if (!Directory.Exists(SaveFolderPath))
//        {
//            Directory.CreateDirectory(SaveFolderPath);
//        }

//        Debug.Log($"SaveSystem initialized. Save folder: {SaveFolderPath}");
//        isInitialized = true;
//    }

//    public static void SaveGame(SaveFile session)
//    {
//        Initialize();

//        string jsonData = JsonUtility.ToJson(session, true);
//        string fullPath = Path.Combine(SaveFolderPath, SaveFileName);
//        File.WriteAllText(fullPath, jsonData);

//        Debug.Log($"Session saved to: {fullPath}");
//    }

//    public static SaveFile LoadGame()
//    {
//        Initialize();

//        string fullPath = Path.Combine(SaveFolderPath, SaveFileName);

//        if (File.Exists(fullPath))
//        {
//            string jsonData = File.ReadAllText(fullPath);
//            SaveFile session = JsonUtility.FromJson<SaveFile>(jsonData);

//            Debug.Log("Session loaded successfully.");
//            return session;
//        }
//        else
//        {
//            Debug.LogWarning("Save file not found!");
//            return null;
//        }
//    }
//}

