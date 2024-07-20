﻿using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static FortunesFromTheScrapyard.Modules.HitHooks;
using FortunesFromTheScrapyard.Modules;
using UnityEngine.Networking;
using RoR2.CharacterAI;

namespace FortunesFromTheScrapyard.Items
{
    public class Headphones : ItemBase<Headphones>
    {
        #region config

        [AutoConfig("Proc Chance Base", 17f)]
        public static float chanceBase = 17f;
        [AutoConfig("Proc Chance Stack", 10f)]
        public static float chanceStack = 10f;
        [AutoConfig("Disorient Duration", 2f)]
        public static float disorientDuration = 2f;
        [AutoConfig("Disorient Damage Increase", 0.2f)]
        public static float disorientDamage = 0.2f;
        #endregion
        public override void Init()
        {
            itemName = "Headphones";
            base.Init();
        }
        public override void Hooks()
        {
            GetHitBehavior += HeadphoneOnHit;
            On.RoR2.HealthComponent.TakeDamage += DisorientDamage;
        }

        private void DisorientDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            if(self.body && self.body.HasBuff(FortunesContent.contentPack.buffDefs.Find("Disorient")))
            {
                damageInfo.damage *= 1 + disorientDamage;
            }
            orig(self, damageInfo);
        }

        private void HeadphoneOnHit(CharacterBody attackerBody, DamageInfo damageInfo, CharacterBody victimBody)
        {
            int headphoneCount = GetCount(attackerBody);
            if (headphoneCount > 0 && !victimBody.HasBuff(FortunesContent.contentPack.buffDefs.Find("Disorient")))
            {
                float procChance = GetStackValue(chanceBase, chanceStack, headphoneCount) * damageInfo.procCoefficient;
                float adjustedProcChance = Util.ConvertAmplificationPercentageIntoReductionPercentage(procChance);
                if (Util.CheckRoll(adjustedProcChance, attackerBody.master))
                {
                    victimBody.AddTimedBuff(FortunesContent.contentPack.buffDefs.Find("Disorient"), disorientDuration);
                }
            }
        }
    }
}