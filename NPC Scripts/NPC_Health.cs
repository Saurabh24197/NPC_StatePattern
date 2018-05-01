using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
	public class NPC_Health : MonoBehaviour 
	{

        private NPC_Master npcMaster;
        public int npcHealth = 100;
        public int npcMaxHealth = 100;

        private bool healthCritical;
        private float healthLow = 20;

        public int destroyWaitTime = 10;

		void OnEnable()
		{
            SetInitialReferences();

            npcMaster.EventNpcDeductHealth += DeductHealth;
            npcMaster.EventNpcIncreaseHealth += IncreaseHealth;
		}

		void OnDisable()
		{
            npcMaster.EventNpcDeductHealth -= DeductHealth;
            npcMaster.EventNpcIncreaseHealth -= IncreaseHealth;
        }

		void Update () 
		{
            if (Input.GetKey(KeyCode.Period)) npcMaster.CallEventNpcIncreaseHealth(20);
		}

		void SetInitialReferences()
		{
            npcMaster = GetComponent<NPC_Master>();
            npcHealth = (npcHealth >= npcMaxHealth) ? npcMaxHealth : npcHealth;
		}

		void CheckHealthFraction()
        {
            if (npcHealth <= healthLow && npcHealth > 0)
            {
                npcMaster.CallEventNpcLowHealth();
                healthCritical = true;
            }

            else if (npcHealth > healthLow && healthCritical)
            {
                npcMaster.CallEventNpcRecoveredAnim();
                healthCritical = false;
            }
        }

        void IncreaseHealth(int healthChange)
        {
            npcHealth += healthChange;
            if (npcHealth > npcMaxHealth)
            {
                npcHealth = npcMaxHealth;
            }

            CheckHealthFraction();
        }

        void DeductHealth(int healthChange)
        {
            npcHealth -= healthChange;

            if (npcHealth <= 0)
            {
                npcHealth = 0;
                npcMaster.CallEventNpcDeath();
                Destroy(gameObject, destroyWaitTime);
            }

            CheckHealthFraction();
        }
	}
}
