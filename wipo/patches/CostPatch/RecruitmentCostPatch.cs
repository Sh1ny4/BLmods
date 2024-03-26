using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace wipo.patches
{
    [HarmonyPatch(typeof(DefaultPartyWageModel), nameof(DefaultPartyWageModel.GetTroopRecruitmentCost))]
    public class RecruitmentCostPatch
    {
        [HarmonyPostfix]
        static void Postfix(ref int __result, CharacterObject troop, Hero buyerHero, bool withoutItemCost = false)
        {
            int num = 10 * MathF.Round((float)troop.Level * MathF.Pow((float)troop.Level, 0.65f) * 0.2f);
            if (troop.Level <= 1)
            {
                num = 10;
            }
            else if (troop.Level <= 6)
            {
                num = 20;
            }
            else if (troop.Level <= 11)
            {
                num = 50;
            }
            else if (troop.Level <= 16)
            {
                num = 100;
            }
            else if (troop.Level <= 21)
            {
                num = 200;
            }
            else if (troop.Level <= 26)
            {
                num = 400;
            }
            else if (troop.Level <= 31)
            {
                num = 600;
            }
            else if (troop.Level <= 36)
            {
                num = 1000;
            }
            else
            {
                num = 1500;
            }
            if (troop.Equipment.Horse.Item != null && !withoutItemCost)
            {
                if (troop.Level < 26)
                {
                    num += 150;
                }
                else
                {
                    num += 500;
                }
            }
            bool flag = troop.Occupation == Occupation.Mercenary || troop.Occupation == Occupation.Gangster || troop.Occupation == Occupation.CaravanGuard;
            if (flag)
            {
                num = MathF.Round((float)num / 2f);
            }
            if (buyerHero != null)
            {
                ExplainedNumber explainedNumber = new ExplainedNumber(1f, false, null);
                if (troop.Tier >= 2 && buyerHero.GetPerkValue(DefaultPerks.Throwing.HeadHunter))
                {
                    explainedNumber.AddFactor(DefaultPerks.Throwing.HeadHunter.SecondaryBonus, null);
                }
                if (troop.IsInfantry)
                {
                    if (buyerHero.GetPerkValue(DefaultPerks.OneHanded.ChinkInTheArmor))
                    {
                        explainedNumber.AddFactor(DefaultPerks.OneHanded.ChinkInTheArmor.SecondaryBonus, null);
                    }
                    if (buyerHero.GetPerkValue(DefaultPerks.TwoHanded.ShowOfStrength))
                    {
                        explainedNumber.AddFactor(DefaultPerks.TwoHanded.ShowOfStrength.SecondaryBonus, null);
                    }
                    if (buyerHero.GetPerkValue(DefaultPerks.Polearm.HardyFrontline))
                    {
                        explainedNumber.AddFactor(DefaultPerks.Polearm.HardyFrontline.SecondaryBonus, null);
                    }
                    if (buyerHero.Culture.HasFeat(DefaultCulturalFeats.SturgianRecruitUpgradeFeat))
                    {
                        explainedNumber.AddFactor(DefaultCulturalFeats.SturgianRecruitUpgradeFeat.EffectBonus, GameTexts.FindText("str_culture", null));
                    }
                }
                else if (troop.IsRanged)
                {
                    if (buyerHero.GetPerkValue(DefaultPerks.Bow.RenownedArcher))
                    {
                        explainedNumber.AddFactor(DefaultPerks.Bow.RenownedArcher.SecondaryBonus, null);
                    }
                    if (buyerHero.GetPerkValue(DefaultPerks.Crossbow.Piercer))
                    {
                        explainedNumber.AddFactor(DefaultPerks.Crossbow.Piercer.SecondaryBonus, null);
                    }
                }
                if (troop.IsMounted && buyerHero.Culture.HasFeat(DefaultCulturalFeats.KhuzaitRecruitUpgradeFeat))
                {
                    explainedNumber.AddFactor(DefaultCulturalFeats.KhuzaitRecruitUpgradeFeat.EffectBonus, GameTexts.FindText("str_culture", null));
                }
                if (buyerHero.IsPartyLeader && buyerHero.GetPerkValue(DefaultPerks.Steward.Frugal))
                {
                    explainedNumber.AddFactor(DefaultPerks.Steward.Frugal.SecondaryBonus, null);
                }
                if (flag)
                {
                    if (buyerHero.GetPerkValue(DefaultPerks.Trade.SwordForBarter))
                    {
                        explainedNumber.AddFactor(DefaultPerks.Trade.SwordForBarter.PrimaryBonus, null);
                    }
                    if (buyerHero.GetPerkValue(DefaultPerks.Charm.SlickNegotiator))
                    {
                        explainedNumber.AddFactor(DefaultPerks.Charm.SlickNegotiator.PrimaryBonus, null);
                    }
                }
                num = MathF.Max(1, MathF.Round((float)num * explainedNumber.ResultNumber));
            }
            __result = num;
        }
    }
}