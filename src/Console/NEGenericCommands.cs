using Il2CppSLZ.Marrow;
using UnityEngine;

namespace NEP.NEDebug.Console
{
    internal static class NEGenericCommands
    {
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
        
        [NEConsoleCommand("quit")]
        public static void QuitGame()
        {
            Application.Quit();
        }
    }
}