using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using StoryMode.CharacterCreationContent;
using TaleWorlds;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Localization;

namespace wipo.patches.CharacterCreationPatch
{
    [HarmonyPatch(typeof(SandboxCharacterCreationContent), "YouthOnInit")]
    internal class YouthOnInitPatch : SandboxCharacterCreationContent
    {
        [HarmonyPrefix]
        static bool prefix(ref YouthOnInitPatch __instance, CharacterCreation characterCreation)
        {
            TextObject _youthIntroductoryText = new TextObject("{=!}{YOUTH_INTRO}");
            characterCreation.IsPlayerAlone = true;
            characterCreation.HasSecondaryCharacter = false;
            characterCreation.ClearFaceGenPrefab();
            TextObject textObject = new TextObject("{=F7OO5SAa}As a youngster growing up in Calradia, war was never too far away. You...", null);
            TextObject textObject2 = new TextObject("{=5kbeAC7k}In wartorn Calradia, especially in frontier or tribal areas, some women as well as men learn to fight from an early age. You...", null);
            _youthIntroductoryText.SetTextVariable("YOUTH_INTRO", CharacterObject.PlayerCharacter.IsFemale ? textObject2 : textObject);
            characterCreation.ChangeFaceGenChars(SandboxCharacterCreationContent.ChangePlayerFaceWithAge((float)__instance.YouthAge, "act_childhood_schooled"));
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_schooled"
            });
            if (__instance.SelectedTitleType < 1 || __instance.SelectedTitleType > 10)
            {
                __instance.SelectedTitleType = 1;
            }
            __instance.RefreshPlayerAppearance(characterCreation);
            return false;
        }
    }
}
