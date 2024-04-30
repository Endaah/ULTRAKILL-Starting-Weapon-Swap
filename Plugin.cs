using BepInEx;
using Configgy;
using HarmonyLib;

namespace StartingWeaponSwap;

[BepInPlugin("com.ender.startingweaponswap", MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("ULTRAKILL.exe")]
[BepInDependency("Hydraxous.ULTRAKILL.Configgy")]
public class Plugin : BaseUnityPlugin
{

    private void Awake()
    {
        // Plugin startup logic
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        // Harmony
        new Harmony(MyPluginInfo.PLUGIN_GUID).PatchAll();

        // Configgy
        new ConfigBuilder("com.ender.startingweaponswap", MyPluginInfo.PLUGIN_NAME).BuildAll();
    }
}
