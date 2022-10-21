using OWML.Common;
using OWML.ModHelper;
using HarmonyLib;

namespace ModTemplate
{
    [HarmonyPatch]
    public class ModTemplate : ModBehaviour
    {
        private void Awake()
        {
            // You won't be able to access OWML's mod helper in Awake.
            // So you probably don't want to do anything here.
            // Use Start() instead.
        }

        private void Start()
        {
            // Starting here, you'll have access to OWML's mod helper.
            ModHelper.Console.WriteLine($"My mod {nameof(ModTemplate)} is loaded!", MessageType.Success);
            Harmony.CreateAndPatchAll(System.Reflection.Assembly.GetExecutingAssembly());
            // Example of accessing game code.
            LoadManager.OnCompleteSceneLoad += (scene, loadScene) =>
            {
                if (loadScene != OWScene.SolarSystem) return;
                var playerBody = FindObjectOfType<PlayerBody>();
                ModHelper.Console.WriteLine($"Found player body, and it's called {playerBody.name}!",
                    MessageType.Success);
            };
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerResources),
            nameof(PlayerResources.StartRefillResources))]

        private static void YourPostfixNameMethod(bool fuel, bool dlcFuelTank, ThrusterFlameColorSwapper ____jetpackFlameColorSwapper)
        {
            ____jetpackFlameColorSwapper.SetFlameColor(dlcFuelTank);

            if (fuel)
            {
                ____jetpackFlameColorSwapper.SetFlameColor(dlcFuelTank);
            }

        }
        static bool wasUsingOxygenAsPropellant = false;
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerResources),
           nameof(PlayerResources.UpdateFuel))]


        private static void UpdateFuelPostFix(PlayerResources __instance, bool ____usingOxygenAsPropellant)
        {
            if (____usingOxygenAsPropellant && !wasUsingOxygenAsPropellant)
            {
                __instance._jetpackFlameColorSwapper.SetFlameColor(false);


            }
            wasUsingOxygenAsPropellant = ____usingOxygenAsPropellant;

        }
    }
}
