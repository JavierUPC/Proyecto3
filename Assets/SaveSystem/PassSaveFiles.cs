using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class PassSaveFiles
{
    public static void Save()
    {
        SaveFile saveFile = null;
        saveFile.CurrentLevelName = SceneManager.GetActiveScene().name;

        SaveSystem.SaveGame(saveFile);
    }

    public static void Load()
    {
        SaveFile loadedFile = SaveSystem.LoadGame();

        SceneManager.LoadScene(loadedFile.CurrentLevelName);
    }
}
