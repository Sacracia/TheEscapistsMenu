using HarmonyLib;
using UnityEngine;

namespace TheEscapists2
{
    public class Loader
    {
        private static GameObject Load;
        internal static Harmony harmony;

        public static void Init()
        {
            harmony = new Harmony("te2.mod.sacracia");
            Load = new GameObject();
            Load.AddComponent<Hacks>();
            Load.AddComponent<PlayerMenu>();
            Load.AddComponent<GeneralMenu>();
            Load.AddComponent<PrisonMenu>();
            Load.AddComponent<JobMenu>();
            Load.AddComponent<OutfitMenu>();
            Load.AddComponent<WeaponMenu>();
            Object.DontDestroyOnLoad(Load);
        }

        public static void Unload()
        {
            _Unload();
        }

        private static void _Unload()
        {
            Object.Destroy(Load);
            harmony.UnpatchAll();
        }
    }
}
