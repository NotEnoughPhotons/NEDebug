using MelonLoader;
using BoneLib;
using UnityEngine;

[assembly: MelonInfo(typeof(NEP.NEDebug.Core), "NEDebug", "1.0.0", "Not Enough Photons: adamdev", null)]
[assembly: MelonGame("Stress Level Zero", "BONELAB")]

namespace NEP.NEDebug
{
    public class Core : MelonMod
    {
        internal static MelonLogger.Instance m_logger;

        public Transform playerTorso;

        public override void OnInitializeMelon()
        {
            m_logger = Melon<Core>.Logger;
            Hooking.OnLevelLoaded += OnLevelLoaded;
        }

        public void OnLevelLoaded(LevelInfo info)
        {
            NEDebug.Initialize();

            NEDebug.Logger.Log("This is info");
            NEDebug.Logger.Warning("This is a warning");
            NEDebug.Logger.Error("This is an error");
        }

        public override void OnUpdate()
        {
            if (Input.GetKey(KeyCode.P))
            {
                NEDebug.DrawDisc();
            }
        }
    }
}