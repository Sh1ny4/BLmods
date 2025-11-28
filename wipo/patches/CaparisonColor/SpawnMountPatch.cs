using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.Scripts;

namespace wipo.patches.CaparisonColor
{ 
    [HarmonyPatch(typeof(CharacterSpawner), "SpawnMount")]
    internal class SpawnMountPatch
    {
        public static bool Prefix(bool __state, ref AgentVisuals ____agentVisuals , CharacterCode characterCode)
        {
            if (SavedColorData.colordata == null)
            {
                SavedColorData.colordata = new SavedColor(characterCode.Color1, characterCode.Color2);
            }
            return true;
        }
        public static void Postfix(bool __state, ref AgentVisuals ____agentVisuals ,CharacterCode characterCode)
        {
            if (__state)
            {
                SavedColorData.colordata = null; 
            }    
        }
    }
}