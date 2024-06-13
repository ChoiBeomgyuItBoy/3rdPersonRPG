using CombatSystem.Attributes;
using CombatSystem.Core;
using UnityEngine;
using UnityEngine.Events;

namespace CombatSystem.Combat
{
    public class Weapon : MonoBehaviour 
    { 
        [SerializeField] Transform hitboxCenter;
        [SerializeField] float hitboxRadius = 0.5f;
        [SerializeField] UnityEvent onHit;

        public void Hit(GameObject user, float damage, Vector2 knockback)
        {
            var hits = Physics.SphereCastAll(hitboxCenter.position, hitboxRadius, Vector3.up, 0);

            foreach(var hit in hits)
            {
                Health health = hit.transform.GetComponent<Health>();

                if(health != null && health != user.GetComponent<Health>())
                {
                    if(health.IsDead())
                    {
                        continue;
                    }

                    if(health.IsInvulnerable())
                    {
                        continue;
                    }

                    ForceReceiver forceReceiver = hit.transform.GetComponent<ForceReceiver>();

                    if(forceReceiver != null && forceReceiver != user.GetComponent<ForceReceiver>())
                    {
                        Vector3 knockbackDirection = GetKnockbackDirection(user.transform, forceReceiver.transform, knockback);

                        forceReceiver.AddForce(knockbackDirection);
                    }

                    health.TakeDamage(damage);

                    onHit.Invoke();
                }
            }
        }

        Vector3 GetKnockbackDirection(Transform user, Transform target, Vector2 knockback)
        {
            Vector3 direction = (target.position - user.position).normalized;

            direction.y += knockback.y;
            direction.x *= knockback.x;
            direction.z *= knockback.x;

            return direction;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitboxCenter.position, hitboxRadius);
        }
    }
}