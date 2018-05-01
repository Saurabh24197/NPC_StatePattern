using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    [System.Serializable]
    public class NPC_Type
    {
        public string name = "Untitled";
        public string type = "Default";
    }

	public class NPC_Master : MonoBehaviour 
	{
        public NPC_Type NPCMetaDetails;

        public delegate void GeneralEventHandler();

        public event GeneralEventHandler EventNpcDeath;
        public event GeneralEventHandler EventNpcLowHealth;
        public event GeneralEventHandler EventNpcHealthRecovered;

        public event GeneralEventHandler EventNpcWalkAnim;
        public event GeneralEventHandler EventNpcStruckAnim;
        public event GeneralEventHandler EventNpcAttackAnim;
        public event GeneralEventHandler EventNpcRecoveredAnim;
        public event GeneralEventHandler EventNpcIdleAnim;


        public delegate void HealthEventHandler(int health);
        public event HealthEventHandler EventNpcDeductHealth;
        public event HealthEventHandler EventNpcIncreaseHealth;

        public delegate void NPCRelationsChangeEventHandler();
        public event NPCRelationsChangeEventHandler EventNpcRelationsChange;

        //Variables used for Animation
        [Space]
        public string animBoolPursuing = "isPursuing";
        public string animTriggerStruck = "Struck";
        public string animTriggerMelee = "Attack";
        public string animTriggerRecovered = "Recovered";


        public void CallEventNpcDeath()
        {
            if (EventNpcDeath != null) EventNpcDeath();
        }

        public void CallEventNpcLowHealth()
        {
            if (EventNpcLowHealth != null) EventNpcLowHealth();
        }

        public void CallEventNpcHealthRecovered()
        {
            if (EventNpcHealthRecovered != null) EventNpcHealthRecovered();
        }


        //Call Events of Animations.
        public void CallEventNpcWalkAnim()
        {
            if (EventNpcWalkAnim != null) EventNpcWalkAnim();
        }

        public void CallEventNpcStruckAnim()
        {
            if (EventNpcStruckAnim != null) EventNpcStruckAnim();
        }

        public void CallEventNpcAttackAnim()
        {
            if (EventNpcAttackAnim != null) EventNpcAttackAnim();
        }

        public void CallEventNpcRecoveredAnim()
        {
            if (EventNpcRecoveredAnim != null) EventNpcRecoveredAnim();
        }

        public void CallEventNpcIdleAnim()
        {
            if (EventNpcIdleAnim != null) EventNpcIdleAnim();
        }


        //Call Events of Health
        public void CallEventNpcDeductHealth(int health)
        {
            if (EventNpcDeductHealth != null) EventNpcDeductHealth(health);
        }

        public void CallEventNpcIncreaseHealth(int health)
        {
            if (EventNpcIncreaseHealth != null) EventNpcIncreaseHealth(health);
        }

        public void CallEventNPCRelationsChange()
        {
            if (EventNpcRelationsChange != null) EventNpcRelationsChange();
        }
	}
}
