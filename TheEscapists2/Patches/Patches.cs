using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace TheEscapists2
{
    internal class Patches
    {
        public static bool Godmode(ref bool __result)
        {
            __result = true;
            return false;
        }

        public static void PassThroughAnyDoor(Door __instance, Collider other)
        {
            
        }
    }
}
