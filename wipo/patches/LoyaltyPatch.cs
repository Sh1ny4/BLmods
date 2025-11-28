using HarmonyLib;
using TaleWorlds.CampaignSystem.GameComponents;

namespace wipo.patches
{
    [HarmonyPatch(typeof(DefaultSettlementLoyaltyModel), nameof(DefaultSettlementLoyaltyModel.SettlementOwnerDifferentCultureLoyaltyEffect), MethodType.Getter)]
    internal class LoyaltyPatch
    {
        [HarmonyPostfix]
        static void Postfix(ref float __result)
        {
            __result = -1f;
        }
    }
}