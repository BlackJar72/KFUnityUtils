using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace kfutils {

    /// <summary>
    /// An enum specifically to allow easy assignment in the unity editor as a type in GUI's like those used by
    /// the RPG Creation Kit (or created with Odin or the like?).  It the realized on the DamageAdjusterList class
    /// to translate these into DamageAdjuster and apply the effect.
    /// </summary>
    public enum DamageAdjustType {
        NONE = 0, // Basic Do Nothing
        FIRE_IMMUNE = 1,
        COLD_IMMUNE = 2,
        GHOSTLY = 3, // Only harmed by Magical / Spiritual attacks (c.f., "magic to hit" in D&D)

    }


    /// <summary>
    /// For adjusting damages in specialized and specific ways, such as immunity to certain damages or requiring a
    /// damage type to harm.
    ///
    /// Note that a general resistance filter, comparing each type and applying a multiplies might be better for some
    /// use cases, such as transient resistances from status effects or protective gear like magic rings.  This is
    /// needed from sistuations when more specific logic is required.
    ///
    /// This is speficically designed to allow for considering combinations -- for example, a ghost might be harmed by
    /// a magical fireball (magical attack) but not mundane fire, as the prsense of magical or spiritual energy allows
    /// for the damage.
    ///
    /// This has been designed for easy entension, though through editing the classes here, rather than throuh
    /// inheritance of some kind of editor magic.
    /// </summary>
    public struct DamageAdjuster {
        public delegate Damages Adjuster(Damages damage);
        public Adjuster adjust;

        public DamageAdjuster(Adjuster adjustment) {
            adjust = adjustment;
        }

        public static Damages None(Damages damage) => damage;

        public static Damages FireImmune(Damages damages) {
            if(((damages.type & DamageType.fire) > 0) && (((damages.type & DamageType.physical) == 0))) {
                damages *= 0;
            }
            return damages;
        }

        public static Damages ColdImmune(Damages damages) {
            if(((damages.type & DamageType.cold) > 0) && (((damages.type & DamageType.physical) == 0))) {
                damages *= 0;
            }
            return damages;
        }

        public static Damages Ghostly(Damages damages) {
            if(((damages.type & (DamageType.magic | DamageType.spiritual)) == 0)) {
                damages *= 0;
            }
            return damages;
        }
    }


    /// <summary>
    /// A class for translating the DamageAjustType enum into a real damage adjusters along with a convenience method
    /// for applying it directly -- thus completing what could be described as my "pseudo-Java-enum pattern."
    /// </summary>
    public class DamageAdjustList {
        public static readonly DamageAdjuster NONE = new DamageAdjuster(DamageAdjuster.None);
        public static readonly DamageAdjuster FIRE_IMMUNE = new DamageAdjuster(DamageAdjuster.FireImmune);
        public static readonly DamageAdjuster COLD_IMMUNE = new DamageAdjuster(DamageAdjuster.ColdImmune);
        public static readonly DamageAdjuster GHOSTLY = new DamageAdjuster(DamageAdjuster.Ghostly);

        public static readonly DamageAdjuster[] Adjusters = new DamageAdjuster[] {NONE, FIRE_IMMUNE, COLD_IMMUNE, GHOSTLY};

        public static DamageAdjuster GetAdjuster(DamageAdjustType type) => Adjusters[(int)type];
        public static Damages Adjust(Damages damage, DamageAdjustType type) => Adjusters[(int)type].adjust(damage);
    }
}