using HarmonyLib;
using OutwardChatCommandsManager.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardChatCommandsManager.Patches
{
    [HarmonyPatch(typeof(WorldSave), nameof(WorldSave.PrepareSave))]
    public class Patch_WorldSave_PrepareSave
    {
        static bool Prefix()
        {
            ChatCommandsSerializer.Instance.AutomaticSaveManagerToXml();

            if(ChatCommandsManager.HasUsedCheat)
            {
                return false;
            }

            return true;
        }
    }
}
