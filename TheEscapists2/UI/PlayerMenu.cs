using HarmonyLib;
using UnityEngine;

namespace TheEscapists2
{
    internal class PlayerMenu : MonoBehaviour
    {
        internal static bool _visible = true;
        private bool _infEnergy = false;
        private bool _zeroHeat = false;
        private bool _teleport = false;
        private bool _godmode = false;
        private bool _noCollide = false;
        private bool _oneHitKill = false;
        private bool _fov = false;
        private bool _invisible = false;
        private float _lastCacheTime = Time.time + 3f;
        private Rect window = new Rect(10f, 10f, 250f, 400f);
        internal static float fov = 1f;
        internal static Player player = null;

        static int Strength
        {
            get
            {
                if (player == null)
                    return 0;
                return Mathf.RoundToInt(player.m_CharacterStats.Strength);
            }
            set
            {
                if (player == null)
                    return;
                player.m_CharacterStats.Strength = value;
            }
        }

        static int Cardio
        {
            get
            {
                if (player == null)
                    return 0;
                return Mathf.RoundToInt(player.m_CharacterStats.Cardio);
            }
            set
            {
                if (player == null)
                    return;
                player.m_CharacterStats.Cardio = value;
            }
        }

        static int Intellect
        {
            get
            {
                if (player == null)
                    return 0;
                return Mathf.RoundToInt(player.m_CharacterStats.Intellect);
            }
            set
            {
                if (player == null)
                    return;
                player.m_CharacterStats.Intellect = value;
            }
        }

        static float Speed
        {
            get
            {
                if (player == null)
                    return 1f;
                return player.m_CharacterMovement.m_fMaxSpeed / 5f;
            }
            set
            {
                if (player == null)
                    return;
                player.m_CharacterMovement.m_fMaxSpeed = 5f * value;
                player.m_CharacterMovement.m_fMaxSpeedBlocking = 1f * value * value;
                player.m_CharacterMovement.m_fMaxSpeedDashing = 1f * value > 20f ? 20f : 1f * value;
            }
        }

        public void OnGUI()
        {
            if (!_visible)
                return;
            window = GUILayout.Window(0, window, OnWindow, "Player", new GUILayoutOption[0]);
        }

        void OnWindow(int windowID)
        {
            DrawElements();
            GUI.DragWindow();
        }

        internal void DrawElements()
        {
            bool flag = GUILayout.Toggle(_godmode, "Godmode", new GUILayoutOption[0]);
            if (flag != _godmode)
            {
                _godmode = flag;
                if (_godmode)
                {
                    bool res = true;
                    var original = AccessTools.Method(typeof(Player), "TakeDamage");
                    var mPrefix = SymbolExtensions.GetMethodInfo(() => Patches.Godmode(ref res));
                    Loader.harmony.Patch(original, new HarmonyMethod(mPrefix));
                }
                else
                {
                    var original = AccessTools.Method(typeof(Player), "TakeDamage");
                    Loader.harmony.Unpatch(original, HarmonyPatchType.Prefix);
                }
            }
            
            flag = GUILayout.Toggle(_oneHitKill, "One Hit Kills", new GUILayoutOption[0]);
            if (flag != _oneHitKill)
            {
                _oneHitKill = flag;
                if (flag)
                {
                    float dmg = 0f;
                    var original = AccessTools.Method(typeof(Character), "DamageCharacterEvent");
                    var mPrefix = SymbolExtensions.GetMethodInfo(() => Patches.OneHitKill(null, null, ref dmg));
                    Loader.harmony.Patch(original, new HarmonyMethod(mPrefix));
                }
                else
                {
                    var original = AccessTools.Method(typeof(Character), "DamageCharacterEvent");
                    Loader.harmony.Unpatch(original, HarmonyPatchType.Prefix);
                }
            }

            flag = GUILayout.Toggle(_noCollide, "No Collision", new GUILayoutOption[0]);
            if (flag != _noCollide && player)
            {
                _noCollide = flag;
                PlayerMenu.player.m_PhysicsSphereCol.enabled = !flag;
            }
            
            _infEnergy = GUILayout.Toggle(_infEnergy, "Max Stamina", new GUILayoutOption[0]);
            _zeroHeat = GUILayout.Toggle(_zeroHeat, "No Heat", new GUILayoutOption[0]);
            _teleport = GUILayout.Toggle(_teleport, "Enable Teleport (F1)", new GUILayoutOption[0]);

            if (GUILayout.Button("Add 100$", new GUILayoutOption[0]) && player != null)
                player.m_CharacterStats.IncreaseMoney(100f);
            GUILayout.Label($"Strength {Strength}", new GUILayoutOption[0]);
            Strength = Mathf.RoundToInt(GUILayout.HorizontalSlider(Strength, 0f, CharacterStats.MaxStrength, new GUILayoutOption[0]));
            GUILayout.Label($"Cardio {Cardio}", new GUILayoutOption[0]);
            Cardio = Mathf.RoundToInt(GUILayout.HorizontalSlider(Cardio, 0f, CharacterStats.MaxCardio, new GUILayoutOption[0]));
            GUILayout.Label($"Intellect {Intellect}", new GUILayoutOption[0]);
            Intellect = Mathf.RoundToInt(GUILayout.HorizontalSlider(Intellect, 0f, CharacterStats.MaxIntellect, new GUILayoutOption[0]));
            GUILayout.Label($"Speed {Speed:f3}", new GUILayoutOption[0]);
            Speed = GUILayout.HorizontalSlider(Speed, 1f, 4f, new GUILayoutOption[0]);
            GUILayout.BeginHorizontal();
            flag = GUILayout.Toggle(_fov, "Change fov");
            if (flag != _fov)
            {
                _fov = flag;
                if (flag)
                {
                    float temp = 0f;
                    var original = AccessTools.Method(typeof(CameraManager), "CalculatePixelPerfectOffset");
                    var mPostfix = SymbolExtensions.GetMethodInfo(() => Patches.FOV(ref temp));
                    Loader.harmony.Patch(original, postfix: new HarmonyMethod(mPostfix));
                }
                else
                {
                    var original = AccessTools.Method(typeof(CameraManager), "CalculatePixelPerfectOffset");
                    Loader.harmony.Unpatch(original, HarmonyPatchType.Postfix);
                }
            }
            fov = GUILayout.HorizontalSlider(fov, 0.1f, 2f, new GUILayoutOption[0]);
            GUILayout.EndHorizontal();
            flag  = GUILayout.Toggle(_invisible, "Invisible");
            if (flag != _invisible)
            {
                _invisible = flag;
                if (player != null)
                    player.m_bIsHidden = flag;
            }
        }
        
        public void Update()
        {
            if (Time.time >= _lastCacheTime)
            {
                _lastCacheTime = Time.time + 3f;
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
                    Vector2 b = default;
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
