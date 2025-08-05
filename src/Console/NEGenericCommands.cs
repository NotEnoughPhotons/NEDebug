using Il2CppSLZ.Marrow;
using Il2CppSLZ.Marrow.SceneStreaming;
using Il2CppSLZ.Marrow.Warehouse;

using UnityEngine;

using BoneLib;

namespace NEP.NEDebug.Console
{
    internal static class NEGenericCommands
    {
        internal static LevelInfo currentLevel;
        
        [NEConsoleCommand("kill")]
        public static void KillPlayer()
        {
            RigManager rig = BoneLib.Player.RigManager;
            rig.health.TAKEDAMAGE(-10000f);
        }
        
        [NEConsoleCommand("getpos")]
        public static void GetPlayerPosition()
        {
            RigManager rig = BoneLib.Player.RigManager;
            Vector3 position = rig.transform.position;
            NELog.Log($"Player position: ({position.x}, {position.y}, {position.z})");
        }
        
        [NEConsoleCommand("setpos")]
        public static void SetPlayerPosition(float x, float y, float z)
        {
            BoneLib.Player.RigManager.Teleport(new Vector3(x, y, z));
        }

        [NEConsoleCommand("setlevel")]
        public static void SetLevel(string barcode)
        {
            SceneStreamer.Load(new Barcode(barcode));
        }

        [NEConsoleCommand("reload")]
        public static void Reload()
        {
            SceneStreamer.Load(new Barcode(currentLevel.barcode));
        }
        
        [NEConsoleCommand("quit")]
        public static void QuitGame()
        {
            Application.Quit();
        }
    }
}