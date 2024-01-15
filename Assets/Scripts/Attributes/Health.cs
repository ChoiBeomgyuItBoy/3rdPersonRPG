using CombatSystem.Core;
using RainbowAssets.Utils;
using UnityEngine;

namespace CombatSystem.Attributes
{
    public class Health : MonoBehaviour, IPredicateEvaluator
    {
        [SerializeField] float maxHealth = 100;
        [SerializeField] float currentHealth = 0;
        [SerializeField] LazyEvent onDamageTaken;

        public void TakeDamage(float damage)
        {
            if(!IsDead())
            {
                currentHealth = Mathf.Max(0, currentHealth - damage);
                StartCoroutine(onDamageTaken?.Invoke());        
            }
        }

        public float GetFraction()
        {
            return currentHealth / maxHealth;
        }

        bool IsDead()
        {
            return currentHealth == 0;
        }

        void Awake()
        {
            currentHealth = maxHealth;
        }

        bool? IPredicateEvaluator.Evaluate(string predicate, string[] parameters)
        {
            if(predicate == "Damage Taken")
            {
                return onDamageTaken.WasInvoked();
            }

            return null;
        }
    }
}