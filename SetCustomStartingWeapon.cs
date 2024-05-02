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

        // Desbloquear el arma elegida en las opciones al save
        GameProgressSaver.AddGear(Configurator.GetStartingWeapon());

        // Si se selecciona el arma alternativa, se desbloquea también y se establece en las preferencias como activa
        // Si no, se establece como activa el arma normal
        if (Configurator.IsAltWeaponSelected()) {
            GameProgressSaver.AddGear(Configurator.GetAltWeaponID());
            MonoSingleton<PrefsManager>.Instance.SetInt("weapon." + Configurator.GetStartingWeapon(), 2);
        } else
            MonoSingleton<PrefsManager>.Instance.SetInt("weapon." + Configurator.GetStartingWeapon(), 1);

        // Se actualiza el arsenal y se fuerza a la mano el arma seleccionada
        MonoSingleton<GunSetter>.Instance.ResetWeapons();
        MonoSingleton<GunSetter>.Instance.ForceWeapon(Configurator.GetStartingWeapon());

        return false;
    }
}