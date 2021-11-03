using HarmonyLib;
using Nick;
using System.Collections.Generic;

namespace TwoPlayerTraining.Patches
{
    [HarmonyPatch(typeof(GameRunner), "Update")]
    class GameRunner_Update
    {
        static void Postfix(GameRunner __instance)
        {
            if (Plugin.Disabled.Value) return;
            if (!__instance.SpawnedGame || __instance.SpawnedModeType != GameModeSetup.Mode.Endless) return;

            var gameInstance = __instance.SpawnedGame;

            if (!gameInstance.GetAgentFromPlayerIndex(1, out var playerTwo)) return;

            if (playerTwo.TryGetControls(out var p2controls))
            {
                if (p2controls.controller.type == GameController.ctrlType.CPU)
                {
                    if (!gameInstance.GetAgentFromPlayerIndex(0, out var playerOne)) return;

                    if (playerOne.TryGetControls(out var p1controls))
                    {
                        var controllers = new List<GameController>();
                        Controllers.GetControllersMovingStick(ref controllers);

                        foreach (var controller in controllers)
                        {
                            if (controller != p1controls.controller)
                            {
                                p2controls.controller = controller;
                                p2controls.PrepareControls();
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
