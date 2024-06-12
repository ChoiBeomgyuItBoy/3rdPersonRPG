using CombatSystem.Attributes;
using CombatSystem.Core;
using UnityEngine;

namespace CombatSystem.Combat
{
    [CreateAssetMenu(menuName = "New Weapon")]
    public class WeaponData : ScriptableObject
    {
        [SerializeField] Weapon equippedWeapon;
        [SerializeField] Projectile projectile;
        [SerializeField] AnimatorOverrideController animatorOverride;
        [SerializeField] float baseDamage = 20;
        [SerializeField] float range = 5;
        [SerializeField] bool isLeftHanded = false;
        [SerializeField] WeaponAttack[] combo;
        Weapon weaponInstance;
        GameObject user;
        Transform rightHand;
        Transform leftHand;

        public WeaponData Clone()
        {
            return Instantiate(this);
        }

        public float GetRange()
        {
            return range;
        }

        public WeaponAttack GetAttack(int index)
        {
            return combo[index];
        }

        public int GetComboLength()
        {
            return combo.Length;
        }

        public void Spawn(GameObject user, Transform rightHand, Transform leftHand, AnimationPlayer animationPlayer)
        {
            this.user = user;
            this.rightHand = rightHand;
            this.leftHand = leftHand;

            weaponInstance = Instantiate(equippedWeapon, GetHand());
            animationPlayer.SetAnimatorOverride(animatorOverride);
        }
 
        public void Hit(int comboIndex, Health target)
        {
            float damage = CalculateDamage(comboIndex);
            Vector3 knockback = GetAttack(comboIndex).GetKnockback();

            if(projectile != null)
            {
                LaunchProjectile(target, damage, knockback);
            }
            else
            {
                weaponInstance.Hit(user, damage, knockback);
            }
        }

        void LaunchProjectile(Health target, float damage, Vector3 knockback)
        {
            var projectileInstance = Instantiate(projectile, GetHand().position, Quaternion.identity);

            if(target != null)
            {
                projectileInstance.SetData(user, damage, knockback, target);
            }
            else
            {
                projectileInstance.SetData(user, damage, knockback, user.transform.position + user.transform.forward * range);
            }
        }

        float CalculateDamage(int comboIndex)
        {
            float bonusPercentage = GetAttack(comboIndex).GetBonusPercentage();
            float bonusDamage = baseDamage * bonusPercentage;
            return Mathf.Round(baseDamage + bonusDamage);
        }

        Transform GetHand()
        {
            return isLeftHanded ? leftHand : rightHand;
        }
    }
}