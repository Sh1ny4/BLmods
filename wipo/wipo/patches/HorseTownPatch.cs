using HarmonyLib;
using SandBox.Missions.MissionLogics.Towns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using static SandBox.SandBoxHelpers;

//Aragas' mod implemented, so that I don't have as many mods
namespace wipo.patches
{
    public static class HorseTownPatch
    {
        public static void Patch(Harmony harmony)
        {
            harmony.Patch(
                AccessTools.Method(typeof(TownCenterMissionController), nameof(TownCenterMissionController.AfterStart)),
                transpiler: new HarmonyMethod(typeof(HorseTownPatch), nameof(Transpiler)));
        }
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var instructionsList = instructions.ToList();

            IEnumerable<CodeInstruction> ReturnDefault(string reason)
            {
                return instructionsList.AsEnumerable();
            }

            var spawnPlayerMethod = AccessTools.Method(typeof(MissionHelper), nameof(MissionHelper.SpawnPlayer));

            var spawnPlayerParameters = spawnPlayerMethod.GetParameters();
            var noHorseParam = spawnPlayerParameters.FirstOrDefault(p => p.Name == "noHorses");
            var noHorseParamIndex = Array.IndexOf(spawnPlayerParameters, noHorseParam);

            var spawnPlayerIndex = -1;
            for (var i = 0; i < instructionsList.Count; i++)
            {
                if (!instructionsList[i].Calls(spawnPlayerMethod))
                    continue;

                spawnPlayerIndex = i;
                break;
            }

            if (spawnPlayerIndex == -1)
                return ReturnDefault("Pattern not found");

            var opCode = instructionsList[spawnPlayerIndex - spawnPlayerParameters.Length + noHorseParamIndex];
            opCode.opcode = OpCodes.Ldc_I4_0;

            return instructionsList;
        }
    }
}