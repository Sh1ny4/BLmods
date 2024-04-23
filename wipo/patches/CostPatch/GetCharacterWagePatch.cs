using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;

namespace wipo.patches.CostPatch
{

    [HarmonyPatch(typeof(DefaultPartyWageModel), nameof(DefaultPartyWageModel.GetCharacterWage))]
    public class GetCharacterWagePatch
    {
        [HarmonyPostfix]
        static void Postfix(ref int __result, CharacterObject character)
        {
            int num;
            switch (character.Tier)
            {
                case 0:
                    num = 1;
                    break;
                case 1:
                    num = 2;
                    break;
                case 2:
                    num = 3;
                    break;
                case 3:
                    num = 5;
                    break;
                case 4:
                    num = 8;
                    break;
                case 5:
                    num = 12;
                    break;
                case 6:
                    num = 17;
                    break;
                default:
                    num = 23;
                    break;
            }
            if (character.IsMounted)
            {
                num = (int)((float)num * 1.3f);
            }
            if (character.IsRanged)
            {
                num = (int)((float)num * 1.1f);
            }
            __result = num;
        }
    }
}