using System.Linq;
using Configgy;

namespace StartingWeaponSwap;

public class StartingWeaponAttributes : MonoSingleton<StartingWeaponAttributes> {

    private static readonly string[] WEAPON_TYPES = {"rev", "sho", "nai", "rai", "rock"};
    private static readonly string[] WEAPON_TYPE_NAMES = {"REVOLVER", "SHOTGUN", "NAILGUN", "RAILCANNON", "ROCKET LAUNCHER"};
    [Configgable(null, "Weapon Type:")]
    private static readonly ConfigDropdown<string> weaponToStartWith =
                                new(
                                    WEAPON_TYPES, 
                                    WEAPON_TYPE_NAMES, 
                                    0
                                );


    private static readonly int[] VARIATION_TYPES = {0, 1, 2};
    private static readonly string[] VARIATION_TYPE_NAMES = {"BLUE", "GREEN", "RED"};
    [Configgable(null, "Weapon Variation:")]
    private static readonly ConfigDropdown<int> variantToStartWith =
                                new(
                                    VARIATION_TYPES,
                                    VARIATION_TYPE_NAMES,
                                    0
                                );


    [Configgable(null, "Start with alternate weapon?")]
    private static ConfigToggle startWithAlt = new(false);



    public string GetStartingWeapon() {
        string startingWeaponID = weaponToStartWith.GetValue();
        int startingVariantID = variantToStartWith.GetValue();
        
        // Las variantes del revolver están dadas la vuelta
        // Verde es 2, Rojo es 1
        if (startingWeaponID == "rev")
            if (startingVariantID == 1)
                startingWeaponID += "2";
            else if (startingVariantID == 2)
                startingWeaponID += "1";
            else
                startingWeaponID += variantToStartWith.ToString();
        else
            startingWeaponID += variantToStartWith.ToString();
        
        return startingWeaponID;
    }

    public bool IsAltWeaponSelected() {
        // Si el arma elegida es una de las que no tienen alt, se devuelve false
        if (weaponToStartWith.GetValue() == "rai" || weaponToStartWith.GetValue() == "rock")
            return false;
        return startWithAlt.GetValue();
    }

    public string GetAltWeaponID() {
        return weaponToStartWith + "alt";
    }

}