using HarmonyLib;
using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;

namespace wipo.patches
{
    [HarmonyPatch(typeof(DefaultPartyTroopUpgradeModel), nameof(DefaultPartyTroopUpgradeModel.CanPartyUpgradeTroopToTarget))]
    internal class CanPartyUpgradeTroopToTargetPatch
    {
        //not working
        [HarmonyPostfix]
        static void Postfix(ref bool __result, PartyBase upgradingParty, CharacterObject upgradeableCharacter, CharacterObject upgradeTarget)
        {
            PerkObject perkObject;
            bool flag = Campaign.Current.Models.PartyTroopUpgradeModel.DoesPartyHaveRequiredItemsForUpgrade(upgradingParty, upgradeTarget);
            bool flag2 = Campaign.Current.Models.PartyTroopUpgradeModel.DoesPartyHaveRequiredPerksForUpgrade(upgradingParty, upgradeableCharacter, upgradeTarget, out perkObject);
            bool flag3 = true;
            if(upgradeTarget.Tier > 4) { flag3 = (upgradingParty.LeaderHero.GetSkillValue(DefaultSkills.Leadership) >= 50); }
            __result = Campaign.Current.Models.PartyTroopUpgradeModel.IsTroopUpgradeable(upgradingParty, upgradeableCharacter) && upgradeableCharacter.UpgradeTargets.Contains(upgradeTarget) && flag2 && flag && flag3;
        }
    }
}