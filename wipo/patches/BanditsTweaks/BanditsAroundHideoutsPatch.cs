using HarmonyLib;
using TaleWorlds.CampaignSystem.GameComponents;

namespace wipo.patches
{
    [HarmonyPatch(typeof(DefaultBanditDensityModel), "NumberOfMaximumBanditPartiesAroundEachHideout", MethodType.Getter)]
    internal class BanditsAroundHideoutsPatch
    {
        [HarmonyPostfix]
        protected static void Postfix(ref int __result)
        {
            __result = 5;
        }
    }
}