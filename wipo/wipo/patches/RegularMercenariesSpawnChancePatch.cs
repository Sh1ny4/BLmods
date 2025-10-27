using HarmonyLib;
using TaleWorlds.CampaignSystem;

namespace wipo.patches
{
    [HarmonyPatch(typeof(DefaultTavernMercenaryTroopsModel), nameof(DefaultTavernMercenaryTroopsModel.RegularMercenariesSpawnChance), MethodType.Getter)]
    internal class IsSearchAvailablePatch
    {
        [HarmonyPostfix]
        static void Postfix(ref float __result)
        {
            //remove the chance for caravan guards to spawn in taverns
            __result = 1f;
        }
    }
}
