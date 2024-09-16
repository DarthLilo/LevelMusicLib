using HarmonyLib;
using LevelMusicLib.Scripts;
using UnityEngine;

namespace LevelMusicLib.Patches;

[HarmonyPatch(typeof(SoundManager))]
public class VanillaMusicPatch
{
    [HarmonyPatch("PlayRandomOutsideMusic")]
    [HarmonyPrefix]
    private static bool VanillaMusicRemovePatch(SoundManager __instance)
    {
        CustomLevelMusic CustomMusicObject = GameObject.FindObjectOfType<CustomLevelMusic>();
        if (CustomMusicObject != null)
        {
            LevelMusicLib.Logger.LogInfo("Cancelled vanilla music call since custom music object was found!");
            return false;
        }
        return true;

    }
}