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
            Loader.Load = new GameObject();
            Loader.Load.AddComponent<Hacks>();
            Loader.Load.AddComponent<PlayerClass>();
            UnityEngine.Object.DontDestroyOnLoad(Loader.Load);
        }

        public static void Unload()
        {
            _Unload();
        }

        private static void _Unload()
        {

            GameObject.Destroy(Load);
            harmony.UnpatchAll();
        }
    }
}
