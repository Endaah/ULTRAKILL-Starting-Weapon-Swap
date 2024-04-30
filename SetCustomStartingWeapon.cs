using System;
using System.Xml.Serialization;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable disable
namespace StartingWeaponSwap;

[HarmonyPatch(typeof(WeaponPickUp), "GotActivated")]
internal class SetCustomStartingWeapon
{
    private static bool Prefix(WeaponPickUp __instance)
    {
        // Ejecutar solo si la escena es el primer nivel
        // Esto implica que el arma que se va a obtener es el revolver del pedestal inicial
        // Si no es el primer nivel, se continua con la ejecución normal
        if (SceneHelper.CurrentScene != "Level 0-1")
        {
            return true;
        }

        // Se ha activado el trigger del pedestal
        // Enviar mensaje de Debug
        var myLogSource = BepInEx.Logging.Logger.CreateLogSource("MyLogSource");
        myLogSource.LogInfo("Activado weapon pickup del revolver");
        BepInEx.Logging.Logger.Sources.Remove(myLogSource);

        StartingWeaponAttributes config = MonoSingleton<StartingWeaponAttributes>.Instance;

        // Desbloquear el arma elegida en las opciones al save
        GameProgressSaver.AddGear(config.GetStartingWeapon());

        // Si se selecciona el arma alternativa, se desbloquea también y se establece en las preferencias como activa
        // Si no, se establece como activa el arma normal
        if (config.IsAltWeaponSelected()) {
            GameProgressSaver.AddGear(config.GetAltWeaponID());
            MonoSingleton<PrefsManager>.Instance.SetInt("weapon." + config.GetStartingWeapon(), 2);
        } else
            MonoSingleton<PrefsManager>.Instance.SetInt("weapon." + config.GetStartingWeapon(), 1);

        // Se actualiza el arsenal y se fuerza a la mano el arma seleccionada
        MonoSingleton<GunSetter>.Instance.ResetWeapons();
        MonoSingleton<GunSetter>.Instance.ForceWeapon(config.GetStartingWeapon());

        return false;
    }
}