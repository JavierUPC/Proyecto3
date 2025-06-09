using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class PassSaveFiles
{
    public static SaveFile loadedFile;
    public static SaveFile saveFile = new SaveFile();

    public static void Save(string sceneName)
    {
        if(sceneName != "Menu")
            saveFile.CurrentLevelName = sceneName;

        SaveSystem.SaveGame(saveFile);
    }

    public static void Load()
    {
        loadedFile = SaveSystem.LoadGame();

        if (loadedFile.CurrentLevelName != null)
        {
            SceneManager.LoadScene(loadedFile.CurrentLevelName);
        }
        else
        {
            SceneManager.LoadScene("Nivel1_Final");
        }
    }

    public static void SetVariables()
    {
        if (loadedFile != null)
            loadedFile = SaveSystem.LoadGame();
    }
}
