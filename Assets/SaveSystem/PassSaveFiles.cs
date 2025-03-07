using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PassSaveFiles
{
    public static void Save()
    {
        SaveFile saveFile = null;
        //guardar datos por guardar en saveFile.xxxx

        SaveSystem.SaveGame(saveFile);
    }

    public static void Load()
    {
        SaveFile loadedFile = SaveSystem.LoadGame();
    }
}
