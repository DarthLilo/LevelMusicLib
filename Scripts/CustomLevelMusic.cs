using UnityEngine;

namespace LevelMusicLib.Scripts;

public class CustomLevelMusic : MonoBehaviour
{   
    [Range(0,100)]
    [Tooltip("Every time you land what is the chance of music playing?")]
    public int MusicChance = 20;

    public bool DisableTimeOfDayStingers;
    public NewMusicEntry[] DaytimeMusics;
    public NewMusicEntry[] EveningMusics;
}