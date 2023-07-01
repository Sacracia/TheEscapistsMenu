using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace TheEscapists2
{
    public class Loader
    {

        private static GameObject Load;
        public static void Init()
        {
            Loader.Load = new GameObject();
            Loader.Load.AddComponent<Hacks>();
            Loader.Load.AddComponent<Player>();
            UnityEngine.Object.DontDestroyOnLoad(Loader.Load);
        }

        public static void Unload()
        {
            _Unload();
        }

        private static void _Unload()
        {
            GameObject.Destroy(Load);
        }
    }
}
