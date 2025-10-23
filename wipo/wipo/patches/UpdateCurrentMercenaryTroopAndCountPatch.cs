using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace wipo.patches
{
    [HarmonyPatch(typeof(RecruitmentCampaignBehavior), "UpdateCurrentMercenaryTroopAndCount")]
    internal class UpdateCurrentMercenaryTroopAndCountPatch : RecruitmentCampaignBehavior
    {
        [HarmonyPrefix]
        static bool Prefix(ref RecruitmentCampaignBehavior __instance, Town town, bool forceUpdate = false)
        {
            RecruitmentCampaignBehavior.TownMercenaryData mercenaryData = __instance.GetMercenaryData(town);
            if (!forceUpdate && mercenaryData.HasAvailableMercenary(Occupation.NotAssigned))
            {
                int difference = __instance.FindNumberOfMercenariesWillBeAdded(mercenaryData.TroopType, true);
                mercenaryData.ChangeMercenaryCount(difference);
                return false;
            }
            CharacterObject randomElementInefficiently = town.Culture.BasicMercenaryTroops.GetRandomElementInefficiently<CharacterObject>();
            __instance._selectedTroop = null;
            float num = __instance.FindTotalMercenaryProbability(randomElementInefficiently, 1f);
            float randomValueRemaining = MBRandom.RandomFloat * num;
            __instance.FindRandomMercenaryTroop(randomElementInefficiently, 1f, randomValueRemaining);
            int number = __instance.FindNumberOfMercenariesWillBeAdded(__instance._selectedTroop, false);
            mercenaryData.ChangeMercenaryType(__instance._selectedTroop, number);
            return false;
        }
        private int FindNumberOfMercenariesWillBeAdded(CharacterObject character, bool dailyUpdate = false)
        {
            int tier = Campaign.Current.Models.CharacterStatsModel.GetTier(character);
            int maxCharacterTier = Campaign.Current.Models.CharacterStatsModel.MaxCharacterTier;
            int num = (maxCharacterTier - tier) * 2;
            int num2 = (maxCharacterTier - tier) * 5;
            float randomFloat = MBRandom.RandomFloat;
            float randomFloat2 = MBRandom.RandomFloat;
            return MBRandom.RoundRandomized(MBMath.ClampFloat((randomFloat * randomFloat2 * (float)(num2 - num) + (float)num) * (dailyUpdate ? 0.1f : 1f), 1f, (float)num2));
        }
    }
}
