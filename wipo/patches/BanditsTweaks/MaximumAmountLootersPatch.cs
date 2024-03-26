using HarmonyLib;
using TaleWorlds.CampaignSystem.GameComponents;

namespace wipo.patches
{
    [HarmonyPatch(typeof(DefaultBanditDensityModel), "NumberOfMaximumLooterParties", MethodType.Getter)]
    internal class MaximumAmountLootersPatch
    {
        [HarmonyPostfix]
        protected static void Postfix(ref int __result)
        {
            __result = 100;
        }
    }
}