using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.Tableaus;

namespace wipo.patches.CaparisonColor
{ 
    [HarmonyPatch(typeof(Mission), "OnEquipItemsFromSpawnEquipment")]
    internal class RefreshCharacterTableauPatch
    {
        public static bool Prefix(bool __state, Agent agent, Agent.CreationType creationType)
        {
            if (SavedColorData.colordata == null && agent.Banner != null) 
            { 
                SavedColorData.colordata = new SavedColor(agent.ClothingColor1, agent.ClothingColor2); 
            }
            return true;
        }
        public static void Postfix(bool __state, Agent agent, Agent.CreationType creationType)
        {
            if (__state) 
            { 
                SavedColorData.colordata = null; 
            }    
        }
    }
}