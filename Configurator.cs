using System;
using Mono.Cecil;
using PluginConfig.API;
using PluginConfig.API.Fields;

namespace StartingWeaponSwap;

internal class Configurator {

    // Declaración de las enums
    enum WeaponType {
        REVOLVER, SHOTGUN, NAILGUN, RAILCANNON, ROCKETLAUNCHER
    }

    enum WeaponVariant {
        BLUE, GREEN, RED
    }

    // Declaración de los campos
    private static BoolField weaponless;
    private static ConfigDivision division;
    private static EnumField<WeaponType> weaponType;
    private static EnumField<WeaponVariant> weaponVariant;
    private static BoolField weaponAlt;

    internal static void BuildMenu(PluginConfigurator config) {
        
        // Establecer icono
        config.SetIconWithURL($"file://Icon.png");

        // Inicializar campo toggleable para Weaponless
        weaponless = new BoolField(config.rootPanel, "Weaponless", "toggleWeaponless", false);

        // Inicializar una division para el resto de campos
        division = new ConfigDivision(config.rootPanel, "division");

        // Inicializar los campos de seleccion de arma, variante y alt
        weaponType = new EnumField<WeaponType>(division, "Weapon Type:", "dropdownWeaponType", WeaponType.REVOLVER);
        weaponVariant = new EnumField<WeaponVariant>(division, "Variant: ", "dropdownVariant", WeaponVariant.BLUE);
        weaponAlt = new BoolField(division, "Start with alt", "toggleAltWeapon", false);

        weaponless.onValueChange += (BoolField.BoolValueChangeEvent e) => {
            division.hidden = e.value;
        };
    }

    // Devuelve la ID completa del arma seleccionada en la configuración
    public static string GetStartingWeapon() {
        return GetWeaponID() + GetVariantID();    
    }

    private static string GetWeaponID() {
        switch (weaponType.value) {
            case WeaponType.REVOLVER:
            default:
                return "rev";
            case WeaponType.SHOTGUN:
                return "sho";
            case WeaponType.NAILGUN:
                return "nai";
            case WeaponType.RAILCANNON:
                return "rai";
            case WeaponType.ROCKETLAUNCHER:
                return "rock";
        }
    }

    private static string GetVariantID() {
        switch (weaponVariant.value) {
            case WeaponVariant.BLUE:
            default:
                return "0";
            
            // Las variantes del revolver están dadas la vuelta
            // Verde es 2, Rojo es 1
            // En el resto, Verde es 1 y Rojo es 2
            case WeaponVariant.GREEN:
                if (GetWeaponID() == "rev")
                    return "2";
                return "1";
            case WeaponVariant.RED:
                if (GetWeaponID() == "rev")
                    return "1";
                return "2";
        }
    }
    
    public static string GetAltWeaponID() {
        return GetWeaponID() + "alt";
    }

    public static bool IsAltWeaponSelected() {
        // Si el arma elegida es una de las que no tienen alt, se devuelve false
        if (GetWeaponID() == "rai" || GetWeaponID() == "rock")
            return false;
        return weaponAlt.value;
    }

    public static bool IsWeaponlessSelected() {
        return weaponless.value;
    }
}