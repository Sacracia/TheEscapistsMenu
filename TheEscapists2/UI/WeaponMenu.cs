using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace TheEscapists2
{
    internal class WeaponMenu : MonoBehaviour
    {
        internal static bool visible = true;
        private Rect window = new Rect(730f, 320f, 200f, 150f);

        public void OnGUI()
        {
            if (!visible)
                return;
            window = GUILayout.Window(5, window, OnWindow, "Set weapon", new GUILayoutOption[0]);
        }

        void OnWindow(int windowID)
        {
            DrawElements();
            GUI.DragWindow();
        }

        void DrawElements()
        {
            foreach (string weaponName in Enum.GetNames(typeof(Weapon)))
                if (GUILayout.Button(weaponName, new GUILayoutOption[0]))
                    GiveWeapon((int)(Weapon)Enum.Parse(typeof(Weapon), weaponName));
        }

        void GiveWeapon(int itemDataID)
        {
            Player player = Gamer.GetPrimaryGamer().m_PlayerObject;
            ItemManager itemManager = ItemManager.GetInstance();
            if (itemManager == null || player == null)
                return;
            List<ItemData> items = itemManager.GetAllowedList();
            if (player == null || items == null)
                return;
            ItemData itemData = itemManager.GetItemDataWithID(itemDataID);
            if (itemData == null)
                return;
            Item newItem = typeof(ItemManager).GetMethod("CreateNewItem_Internal", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(itemManager, new object[2] { "", 0 }) as Item;
            if (newItem == null) 
                return;
            newItem.m_ItemData = ScriptableObject.CreateInstance<ItemData>();
            newItem.m_ItemData.CopyData(itemData);
            newItem.m_ItemData.SetParentItem(newItem);
            newItem.MeshRendererProp.material = newItem.m_ItemData.m_ItemWorldMaterial;
            player.SetEquippedItem(newItem, true, false);
        }

        private enum Weapon
        {
            SuperBaton = 377,
            Baton = 482,
            Nunchuck = 290,
            Stungun = 369,
            SuperBat = 375,
            CombBlade = 138,
            SuperKnuckle = 376,
            Sledgehammer = 269,
            Spear = 270,
            SuperWhip = 379
        }
    }
}
