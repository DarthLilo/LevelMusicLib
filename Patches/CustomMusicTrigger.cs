using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using LevelMusicLib.Scripts;
using UnityEngine;

namespace LevelMusicLib.Patches;

[HarmonyPatch(typeof(TimeOfDay))]
public class CustomMusicTrigger
{

    public static int CustomSongOffset = 0;

    [HarmonyPatch("PlayTimeMusicDelayed")]
    [HarmonyPrefix]
    private static bool PlayTimeMusicDelayedPatch(TimeOfDay __instance)
    {

        CustomLevelMusic CustomMusicObject = GameObject.FindObjectOfType<CustomLevelMusic>();
        GameObject OutsideMusicObject = GameObject.Find("OutsideMusic");

        if (CustomMusicObject == null)
        {
            LevelMusicLib.Logger.LogInfo("CustomMusicObject was null, skipping!");
            return true;
        }


        
        if (OutsideMusicObject.scene.name == "SampleSceneRelay")
        {
            AudioSource OutsideMusicSource =  OutsideMusicObject.GetComponent<AudioSource>();
            if (OutsideMusicSource.isPlaying == false) // Continue unless audio is already playing
            {
                if (UnityEngine.Random.Range(0,100) < CustomMusicObject.MusicChance || ES3.Load("TimesLanded", "LCGeneralSaveData", 0) <= 1) // Play music if threshold is met or is players first few times landing
                {
                    PlayCustomMusic(CustomMusicObject,OutsideMusicSource,__instance.dayMode >= DayMode.Sundown);
                    if (CustomMusicObject.DisableTimeOfDayStingers == true) return false;
                }
            }


        } else { // INVALID OBJECT
            LevelMusicLib.Logger.LogError($"The OutsideMusic gameobject found was of another scene [{OutsideMusicObject.scene.name}] please verify that the moon doesn't have another identically named object!");
        }
        return true;
    }

    [HarmonyPatch("OnDayChanged")]
    [HarmonyPostfix]
    private static void ResetCustomSongOffset(TimeOfDay __instance)
    {
        CustomSongOffset = 0;
    }

    public static void PlayCustomMusic(CustomLevelMusic CustomMusicObject, AudioSource MusicSource, bool EveningMusic)
    {
        if (SoundManager.Instance.timeSincePlayingLastMusic < 200f) return;

        if (EveningMusic && CustomMusicObject.EveningMusics.Count() >= 1)
        {
            MusicSource.clip = ChooseWeightedClip(CustomMusicObject.EveningMusics);
        }

        if (EveningMusic == false && CustomMusicObject.DaytimeMusics.Count() >= 1)
        {
            MusicSource.clip = ChooseWeightedClip(CustomMusicObject.DaytimeMusics);
        }

        MusicSource.Play();
        SoundManager.Instance.playingOutsideMusic = true;
        SoundManager.Instance.timeSincePlayingLastMusic = 0f;
    }

    public static AudioClip ChooseWeightedClip(NewMusicEntry[] Musics)
    {
        List<AudioClip> WeightedClips = [];
        System.Random random = new(StartOfRound.Instance.randomMapSeed + CustomSongOffset);

        foreach(var song in Musics)
        {
            for (int i = 0; i < song.weight; i++)
            {
                WeightedClips.Add(song.clip);
            }
        }

        CustomSongOffset++;

        return WeightedClips[random.Next(WeightedClips.Count)];

    }
}