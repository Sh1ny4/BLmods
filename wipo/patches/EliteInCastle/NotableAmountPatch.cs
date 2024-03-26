using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Settlements;

namespace wipo.patches
{

    [HarmonyPatch(typeof(DefaultNotableSpawnModel), nameof(DefaultNotableSpawnModel.GetTargetNotableCountForSettlement))]
    public class NotableAmountPatch
    {
        [HarmonyPostfix]
        static void Postfix(ref int __result, Settlement settlement, Occupation occupation)
        {
            int result = 0;
            if (settlement.IsTown)
            {
                if (occupation == Occupation.Merchant)
                {
                    result = 2;
                }
                else if (occupation == Occupation.GangLeader)
                {
                    result = 2;
                }
                else if (occupation == Occupation.Artisan)
                {
                    result = 1;
                }
                else
                {
                    result = 0;
                }
            }
            else if (settlement.IsVillage)
            {
                if (occupation == Occupation.Headman)
                {
                    result = 1;
                }
                else if (occupation == Occupation.RuralNotable)
                {
                    result = 2;
                }
            }
            else if (settlement.IsCastle)
            {
                if (occupation == Occupation.Headman)
                {
                    result = 1;
                }
                else if (occupation == Occupation.RuralNotable)
                {
                    result = 1;
                }
            }
            __result = result;
        }
    }
}