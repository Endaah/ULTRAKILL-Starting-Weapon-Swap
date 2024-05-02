using BepInEx;
using BepInEx.Bootstrap;
using PluginConfig.API;
using HarmonyLib;
using System.Runtime.CompilerServices;

namespace StartingWeaponSwap;

[BepInPlugin("com.ender.startingweaponswap", MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("ULTRAKILL.exe")]
public class Plugin : BaseUnityPlugin
{

    private PluginConfigurator config;

    private void Awake()
    {
        // Plugin startup logic
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        // Harmony
        new Harmony(MyPluginInfo.PLUGIN_GUID).PatchAll();

        // PluginConfigurator
        config = PluginConfigurator.Create("Starting Weapon Swap", "com.ender.startingweaponswap");
        Configurator.BuildMenu(config);
    }
}
