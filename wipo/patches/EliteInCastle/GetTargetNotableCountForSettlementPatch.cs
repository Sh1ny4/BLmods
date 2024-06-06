using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Settlements;

namespace wipo.patches.EliteInCastle
{

    [HarmonyPatch(typeof(DefaultNotableSpawnModel), nameof(DefaultNotableSpawnModel.GetTargetNotableCountForSettlement))]
    public class GetTargetNotableCountForSettlementPatch
    {
        [HarmonyPostfix]
        static void Postfix(ref int __result, Settlement settlement, Occupation occupation)
        {
            if (settlement.IsCastle)
            {
                if (occupation == Occupation.Headman)
                {
                    __result = 1;
                }
                else if (occupation == Occupation.RuralNotable)
                {
                    __result = 2;
                }
            }
        }
    }
}