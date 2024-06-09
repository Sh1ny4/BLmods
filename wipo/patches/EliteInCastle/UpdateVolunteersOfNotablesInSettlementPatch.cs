using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace wipo.patches.EliteInCastle
{

    [HarmonyPatch(typeof(RecruitmentCampaignBehavior), "UpdateVolunteersOfNotablesInSettlement")]
    public class UpdateVolunteersOfNotablesInSettlementPatch
    {
        [HarmonyPostfix]
        static void Postfix(Settlement settlement)
        {
            if (settlement.IsCastle && !settlement.IsUnderSiege)
            {
                foreach (Hero hero in settlement.Notables)
                {
                    if (hero.CanHaveRecruits && hero.IsAlive)
                    {
                        bool flag = false;
                        CharacterObject basicVolunteer = Campaign.Current.Models.VolunteerModel.GetBasicVolunteer(hero);
                        for (int i = 0; i < 6; i++)
                        {
                            if (MBRandom.RandomFloat <= MathF.Clamp(settlement.Town.Prosperity/2000, 0f,0.5f))
                            {
                                CharacterObject characterObject = hero.VolunteerTypes[i];
                                if (characterObject == null)
                                {
                                    hero.VolunteerTypes[i] = basicVolunteer;
                                    flag = true;
                                }
                                else if (characterObject.UpgradeTargets.Length != 0 && characterObject.Tier < 5)
                                {
                                    float num = MathF.Log(hero.Power / (float)characterObject.Tier, 2f) * 0.01f;
                                    if (MBRandom.RandomFloat < num)
                                    {
                                        hero.VolunteerTypes[i] = characterObject.UpgradeTargets[MBRandom.RandomInt(characterObject.UpgradeTargets.Length)];
                                        flag = true;
                                    }
                                }
                            }
                        }
                        if (flag)
                        {
                            CharacterObject[] volunteerTypes = hero.VolunteerTypes;
                            for (int j = 1; j < 6; j++)
                            {
                                CharacterObject characterObject2 = volunteerTypes[j];
                                if (characterObject2 != null)
                                {
                                    int num2 = 0;
                                    int num3 = j - 1;
                                    CharacterObject characterObject3 = volunteerTypes[num3];
                                    while (num3 >= 0 && (characterObject3 == null || (float)characterObject2.Level + (characterObject2.IsMounted ? 0.5f : 0f) < (float)characterObject3.Level + (characterObject3.IsMounted ? 0.5f : 0f)))
                                    {
                                        if (characterObject3 == null)
                                        {
                                            num3--;
                                            num2++;
                                            if (num3 >= 0)
                                            {
                                                characterObject3 = volunteerTypes[num3];
                                            }
                                        }
                                        else
                                        {
                                            volunteerTypes[num3 + 1 + num2] = characterObject3;
                                            num3--;
                                            num2 = 0;
                                            if (num3 >= 0)
                                            {
                                                characterObject3 = volunteerTypes[num3];
                                            }
                                        }
                                    }
                                    volunteerTypes[num3 + 1 + num2] = characterObject2;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}