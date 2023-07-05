using HarmonyLib;
using UnityEngine;

namespace TheEscapists2
{
    internal class PlayerClass : MonoBehaviour
    {
        static bool _infEnergy = false;
        static bool _zeroHeat = false;
        static bool _teleport = false;
        static bool _godmode = false;
        public static Player player = null;
        float lastCacheTime = Time.time + 5f;

        public void Start() {}

        internal static int Strength
        {
            get
            {
                if (!player)
                    return 0;
                return Mathf.RoundToInt(player.m_CharacterStats.Strength);
            }
            set
            {
                if (!player)
                    return;
                player.m_CharacterStats.Strength = value;
            }
        }

        internal static int Cardio
        {
            get
            {
                if (!player)
                    return 0;
                return Mathf.RoundToInt(player.m_CharacterStats.Cardio);
            }
            set
            {
                if (!player)
                    return;
                player.m_CharacterStats.Cardio = value;
            }
        }

        internal static int Intellect
        {
            get
            {
                if (!player)
                    return 0;
                return Mathf.RoundToInt(player.m_CharacterStats.Intellect);
            }
            set
            {
                if (!player)
                    return;
                player.m_CharacterStats.Intellect = value;
            }
        }

        internal static float Speed
        {
            get
            {
                if (!player)
                    return 1f;
                return player.m_CharacterMovement.m_fMaxSpeed / 5f;
            }
            set
            {
                if (!player)
                    return;
                player.m_CharacterMovement.m_fMaxSpeed = 5f * value;
                player.m_CharacterMovement.m_fMaxSpeedBlocking = 1f * value * value;
                player.m_CharacterMovement.m_fMaxSpeedDashing = 1f * value > 20f ? 20f : 1f * value;
            }
        }

        internal static void Render()
        {
            bool _flag = GUILayout.Toggle(_godmode, "Godmode", new GUILayoutOption[0]);
            _infEnergy = GUILayout.Toggle(_infEnergy, "Max Stamina", new GUILayoutOption[0]);
            _zeroHeat = GUILayout.Toggle(_zeroHeat, "No Heat", new GUILayoutOption[0]);
            _teleport = GUILayout.Toggle(_teleport, "Enable Teleport (F1)", new GUILayoutOption[0]);
            if (GUILayout.Button("Add 100$", new GUILayoutOption[0]))
                player.m_CharacterStats.IncreaseMoney(100f);
            GUILayout.Label($"Strength {Strength}", new GUILayoutOption[0]);
            Strength = Mathf.RoundToInt(GUILayout.HorizontalSlider(Strength, 0f, CharacterStats.MaxStrength, new GUILayoutOption[0]));
            GUILayout.Label($"Cardio {Cardio}", new GUILayoutOption[0]);
            Cardio = Mathf.RoundToInt(GUILayout.HorizontalSlider(Cardio, 0f, CharacterStats.MaxCardio, new GUILayoutOption[0]));
            GUILayout.Label($"Intellect {Intellect}", new GUILayoutOption[0]);
            Intellect = Mathf.RoundToInt(GUILayout.HorizontalSlider(Intellect, 0f, CharacterStats.MaxIntellect, new GUILayoutOption[0]));
            GUILayout.Label($"Speed {Speed:f3}", new GUILayoutOption[0]);
            Speed = GUILayout.HorizontalSlider(Speed, 1f, 4f, new GUILayoutOption[0]);

            if (_flag != _godmode)
            {
                _godmode = _flag;
                if (_godmode)
                {
                    bool res = true;
                    var original = AccessTools.Method(typeof(Player), "TakeDamage");
                    var mPrefix = SymbolExtensions.GetMethodInfo(() => Patches.Godmode(ref res));
                    Loader.harmony.Patch(original, new HarmonyMethod(mPrefix));
                } else
                {
                    var original = AccessTools.Method(typeof(Player), "TakeDamage");
                    Loader.harmony.Unpatch(original, HarmonyPatchType.Prefix);
                }
            }

        }

        public void Update()
        {
            if (Time.time >= lastCacheTime)
            {
                lastCacheTime = Time.time + 5f;
                player = Gamer.GetPrimaryGamer().m_PlayerObject;
            }


            if (_godmode)
            {
                if (!player) 
                    return;
                player.m_CharacterStats.Health = CharacterStats.MaxHealth;
            }

            if (_infEnergy)
            {
                if (!player)
                    return;
                player.m_CharacterStats.Energy = CharacterStats.MaxEnergy;
            }

            if (_zeroHeat)
            {
                if (!player)
                    return;
                player.m_CharacterStats.Heat = 0f;
            }

            if (Input.GetKeyDown(KeyCode.F1) && _teleport && player)
            {
                Camera camera = CameraManager.GetInstance().GetCamera(player.m_PlayerCameraManagerBindingID);
                if (camera != null)
                {
                    Vector2 a = Input.mousePosition;
                    Vector2 b = default(Vector2);
                    MouseDetector.GetMouseToCameraOffset(camera, ref b);
                    a += b;
                    Vector3 vector = new Vector3(a.x, a.y, camera.nearClipPlane);
                    vector = camera.ScreenToWorldPoint(vector);
                    vector.z = player.CurrentFloor.m_zPos;
                    player.Teleport(vector);
                }
            }
        }
    }
}
