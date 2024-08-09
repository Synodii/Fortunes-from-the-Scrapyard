using EntityStates;
using RoR2.Skills;
using RoR2;
using RoR2.Audio;
using UnityEngine;
using UnityEngine.Networking;
using FortunesFromTheScrapyard;

namespace EntityStates.Gump
{
    public class Tear : BasicMeleeAttack
    {
        public static float baseDamageCoefficient = .5f;
        public static float swingTimeCoefficient = 2f;
        public string swingString;
        public override void OnEnter()
        {
            swingEffectPrefab = EntityStates.Merc.Weapon.GroundLight2.comboFinisherSwingEffectPrefab;

            damageCoefficient = baseDamageCoefficient;

            swingString = "Primary" + (1 + step);

            switch (step)
            {
                case 0:
                    {
                        swingEffectMuzzleString = "SlugRL";
                        break;
                    }
                case 1:
                    {
                        swingEffectMuzzleString = "SlugLR";
                        break;
                    }
            }
            base.OnEnter();
        }
        /*public override void PlayAnimation()
        {
            PlayCrossfade("Gesture, Override", swingString, "Primary.playbackRate", duration * swingTimeCoefficient, 0.05f);
        }*/
        public override void AuthorityModifyOverlapAttack(OverlapAttack overlapAttack)
        {
            base.AuthorityModifyOverlapAttack(overlapAttack);
            Vector3 direction = GetAimRay().direction;
            direction.y = Mathf.Max(direction.y, direction.y * 0.5f);
            FindModelChild("SwingPivot").rotation = Util.QuaternionSafeLookRotation(direction);
        }
        void SteppedSkillDef.IStepSetter.SetStep(int i)
        {
            step = i;
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
        public override void OnExit()
        {
            base.OnExit();
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write((byte)step);
        }
        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            step = (int)reader.ReadByte();
        }
    }
}
