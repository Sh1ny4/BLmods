using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;

namespace wipo.patches
{
    [HarmonyPatch(typeof(DefaultMobilePartyFoodConsumptionModel), nameof(DefaultMobilePartyFoodConsumptionModel.CalculateDailyBaseFoodConsumptionf))]
    public class FoodPatch
    {
        [HarmonyPostfix]
        static void Postfix(ref ExplainedNumber __result, MobileParty party, bool includeDescription = false)
        {
            int num = party.Party.NumberOfAllMembers + party.Party.NumberOfMounts + party.Party.NumberOfMenWithHorse + party.Party.NumberOfPackAnimals + party.Party.NumberOfPrisoners / 2;
            num = ((num < 1) ? 1 : num);
            __result = new ExplainedNumber(-(float)num / (float)20f, includeDescription, null);
        }
    }
}