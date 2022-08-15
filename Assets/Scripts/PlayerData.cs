using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level;
    public int scene0;
    public int scene1;
    public int scene2;

    public PlayerData(int lvl, int[] scenes)
    {
        level = lvl;
        scene0 = scenes[0];
        scene1 = scenes[1];
        scene2 = scenes[2];
    }
}
