using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class PassSaveFiles
{
    public static void Save(string sceneName)
    {
        SaveFile saveFile = new SaveFile();
        saveFile.CurrentLevelName = sceneName;

        SaveSystem.SaveGame(saveFile);
    }

    public static void Load()
    {
        SaveFile loadedFile = SaveSystem.LoadGame();

        if (loadedFile != null)
        {
            SceneManager.LoadScene(loadedFile.CurrentLevelName);
        }
        else
        {
            SceneManager.LoadScene("Blockout 1");
        }
    }
}
