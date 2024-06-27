using System;
using System.Collections;
using System.Collections.Generic;
using CombatSystem.Control;
using UnityEngine;

namespace CombatSystem.Abilites.Targeting
{
    [CreateAssetMenu(menuName = "Abilities/Targeting/Delayed Point Targeting")]
    public class DelayedPointTargeting : TargetingStrategy
    {
        [SerializeField] GameObject pointPrefab;
        [SerializeField] float areaAffectRadius = 5;
        const string activationInput = "Attack";

        public override void StartTargeting(GameObject user, Action<IEnumerable<GameObject>> finished)
        {
            user.GetComponent<MonoBehaviour>().StartCoroutine(TargetingRoutine(user, finished));
        }

        IEnumerator TargetingRoutine(GameObject user, Action<IEnumerable<GameObject>> finished)
        {
            GameObject pointInstance = Instantiate(pointPrefab, user.transform.position, Quaternion.identity);

            pointInstance.transform.localScale = new Vector3(areaAffectRadius * 2, 1, areaAffectRadius * 2);

            InputReader inputReader = user.GetComponent<InputReader>();

            while(true)
            {
                if(inputReader.GetInputAction(activationInput).IsPressed())
                {
                    yield return new WaitWhile(() => Input.GetMouseButton(0));

                    Destroy(pointInstance.gameObject);

                    finished(GetTargetsInRadius(pointInstance.transform.position));

                    yield break;
                }
            
                yield return null;
            }
        }

        IEnumerable<GameObject> GetTargetsInRadius(Vector3 pointPosition)
        {
            RaycastHit[] hits = Physics.SphereCastAll(pointPosition, areaAffectRadius, Vector3.up, 0);

            foreach(var hit in hits)
            {
                yield return hit.collider.gameObject;
            }
        }
    }
}