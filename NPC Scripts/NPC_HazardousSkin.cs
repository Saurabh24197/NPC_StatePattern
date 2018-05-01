using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public class NPC_HazardousSkin : MonoBehaviour
    {
        private NPC_Master npcMaster;

        [Header("Use with <NPC_CollisionField>, <Ragdoll>")]
        public bool deathOnPlayerCollide = false;

        [Space]
        public int damageToApply = 10;
        public GameObject hitEffectsCustom;

        private float waitTime;

        private void Awake()
        {
            waitTime = Time.time + 3f;
            npcMaster = transform.root.GetComponent<NPC_Master>();
        }

        private void OnTriggerEnter(Collider other)
        {
			ApplyDamage(other, damageToApply);
        }

		/*
		 * Don't turn this ON! takes way too much performance
		 * Starts Self harming. Crazy Suicidal NPC (-_-)
		 * 
		//This turned out to be a necessary case due to the necessity of checking isHazardType.
        private void OnTriggerStay(Collider other)
        {
        	ApplyDamage(other, damageToApply / 10);
        }
		*/

        void ApplyDamage(Collider other, int damage)
        {
			if (Time.time < waitTime)
			{
				return;
			}

			bool isHazardType = other.transform.root.GetComponentInChildren<NPC_HazardousSkin>();
			bool isPlayer = (other.gameObject == GameManager_References._player);

			//Don't attack the sametype entity
			if (isHazardType)
			{
				return;
			}
				
		
			if (isPlayer || other.GetComponent<NPC_TakeDamage>() != null)
            {
				other.SendMessage("ProcessDamage", damage, SendMessageOptions.DontRequireReceiver);
                other.SendMessage("CallEventPlayerHealthDeduction", damage, SendMessageOptions.DontRequireReceiver);
			
				if (!isPlayer)
				{
					//Self damage
					transform.root.SendMessage ("ProcessDamage", damage, SendMessageOptions.DontRequireReceiver);
				}

				ManageDestruction(isPlayer);
            }
        }

		void ManageDestruction(bool isPlayer = false)
        {
			if (deathOnPlayerCollide && isPlayer)
			{
				//Find and Disable the Drop Item Script.
				if (transform.root.GetComponent<NPC_DropItems> () != null) 
				{
					transform.root.GetComponent<NPC_DropItems> ().enabled = false;
				}

				if (hitEffectsCustom != null)
				{
					Instantiate(hitEffectsCustom, gameObject.transform.position, gameObject.transform.rotation);
				}

				npcMaster.CallEventNpcDeath ();
			}

			else
			{
				//Just Spawn hitEffects and be done with it.
				if (hitEffectsCustom != null)
				{
					Instantiate (hitEffectsCustom, gameObject.transform.position, gameObject.transform.rotation);
				}
			}
        }



    }
}


