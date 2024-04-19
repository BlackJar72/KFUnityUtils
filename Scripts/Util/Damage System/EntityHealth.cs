using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


namespace kfutils {

    [Serializable]
    public class EntityHealth {
        public static readonly DefaultDamageAdjuster defaultDamageAdjuster = new DefaultDamageAdjuster();

        [SerializeField] float baseHealth;

        [SerializeField] float wound;
        [SerializeField] float shock;

        [SerializeField][HideInInspector] float buff;

        public int   BaseHealth { get => (int)baseHealth;  }
        public float RelativeWound { get => wound / baseHealth; }
        public float RelativeShock { get => shock / baseHealth; }

        public float Health { get => wound;  set { wound = value; } }
        public float Shock { get => shock;  set { shock = value; } }
        public float Buff { get => buff;  set { buff = value; MakeSane(); } }

        public bool ShouldDie { get => ((wound < 1) || (shock < 1)); }

        //Tried to fix BTree error, didn't work.
        //There really should be no conversion as errors found by the IDE help find places that need to be edited.
        //public static implicit operator float(EntityHealth h) => Mathf.Min(h.shock, h.wound);

        public EntityHealth(float baseHealth) {
            shock = wound = this.baseHealth = baseHealth;
            buff = 0;
        }


        public void MakeSane() {
            wound = Mathf.Min(wound, baseHealth + buff);
            shock = Mathf.Min(shock, baseHealth + buff);
        }


        public void ChangeBaseHealth(float newHealth) {
            if(newHealth > baseHealth) {
                baseHealth = newHealth;
            } else {
                float woundDiff = RelativeWound;
                float shockDiff = RelativeShock;
                baseHealth = newHealth;
                wound = baseHealth * woundDiff;
                shock = shock * shockDiff;
            }
            MakeSane();
        }


        public void ChangeBaseHealthBy(float amount) {
            ChangeBaseHealth(baseHealth + amount);
        }


        public void TakeDamage(Damages damage) {
            shock -= damage.shock;
            wound -= damage.wound;
        }


        public void Heal(float amount) {
            wound = Mathf.Clamp(wound + amount, 0, baseHealth + buff);
            shock = Mathf.Clamp(shock + amount, 0, baseHealth + buff);
        }


        public void HealShock(float amount) {;
            shock = Mathf.Clamp(shock + amount, 0, baseHealth + buff);
        }


        public void HealWound(float amount) {
            wound = Mathf.Clamp(wound + amount, 0, baseHealth + buff);
        }


        public void HealFully() {
            shock = wound = baseHealth;
        }


        public void MakeDead() {
            shock = wound = -1f;
        }


        // FIXME??: Integrate into regen coroutine?
        /// <summary>
        /// For shock regeneration after being wounded.
        ///
        /// This may be called by the regrn coroutine, or perhaps recreated in it, or even
        /// have the coroutine moved here...?
        /// </summary>
        public bool NaturalRegen() {
            shock = Mathf.Min((shock + ((baseHealth * 0.01f) + 1.0f) * Time.deltaTime), baseHealth);
            return shock < baseHealth;
        }


        //public static EntityHealth operator +(EntityHealth a) => a;


    }


    public class DefaultDamageAdjuster : IDamageAdjuster {
        public Damages Apply(kfutils.Damages damage) {
            return default(Damages);
        }
    }
}