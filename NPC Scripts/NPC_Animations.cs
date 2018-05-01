using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
	public class NPC_Animations : MonoBehaviour 
	{

        private NPC_Master npcMaster;
        private Animator myAnimator;

		[Space]
        public AudioClip deathAudio;

		[Space]
        public AudioClip walkAnimAudio;
        public AudioClip idleAnimAudio;
        public AudioClip attackAnimAudio;
        public AudioClip recoveredAnimAudio;
        public AudioClip StruckAnimAudio;

		[Header("Optional : Switch Hitbox for Struck Animations")]
		public GameObject hitBoxDefault;
		public GameObject hitBoxStruck;

		void OnEnable()
		{
            SetInitialReferences();

            npcMaster.EventNpcAttackAnim += ActivateAttackAnimation;
            npcMaster.EventNpcWalkAnim += ActivateWalkingAnimation;
            npcMaster.EventNpcIdleAnim += ActivateIdleAnimation;

            npcMaster.EventNpcRecoveredAnim += ActivateRecoveredAnimation;
            npcMaster.EventNpcStruckAnim += ActivateStruckAnimation;

            npcMaster.EventNpcDeath += OnDeathGOManagement;
        
        }

		void OnDisable()
		{
            npcMaster.EventNpcAttackAnim -= ActivateAttackAnimation;
            npcMaster.EventNpcWalkAnim -= ActivateWalkingAnimation;
            npcMaster.EventNpcIdleAnim -= ActivateIdleAnimation;
            npcMaster.EventNpcRecoveredAnim -= ActivateRecoveredAnimation;
            npcMaster.EventNpcStruckAnim -= ActivateStruckAnimation;

            npcMaster.EventNpcDeath -= OnDeathGOManagement;
        }

		void SetInitialReferences()
		{
            npcMaster = GetComponent<NPC_Master>();

            if (GetComponent<Animator>() != null)
            {
                myAnimator = GetComponent<Animator>();
            }
		}

        void ActivateWalkingAnimation()
        {
            if (myAnimator != null && myAnimator.enabled)
            {
                myAnimator.SetBool(npcMaster.animBoolPursuing, true);
                PlayMusic(walkAnimAudio, true);
            }
        }

        void ActivateIdleAnimation()
        {
            if (myAnimator != null && myAnimator.enabled)
            {
                myAnimator.SetBool(npcMaster.animBoolPursuing, false);
                PlayMusic(idleAnimAudio, false);
            }
        }

        void ActivateAttackAnimation()
        {
            if (myAnimator != null && myAnimator.enabled)
            {
                myAnimator.SetTrigger(npcMaster.animTriggerMelee);
                PlayMusic(attackAnimAudio, false);
            }
        }

        void ActivateRecoveredAnimation()
        {
            if (myAnimator != null && myAnimator.enabled)
            {
                myAnimator.SetTrigger(npcMaster.animTriggerRecovered);
                
				if (hitBoxStruck != null && hitBoxDefault != null)
				{
					hitBoxStruck.SetActive(false);
					hitBoxDefault.SetActive(true);
				}

				PlayMusic(recoveredAnimAudio, false);
            }
        }

        void ActivateStruckAnimation()
        {
            if (myAnimator != null && myAnimator.enabled)
            {
                myAnimator.SetTrigger(npcMaster.animTriggerStruck);

				if (hitBoxStruck != null && hitBoxDefault != null)
				{
					hitBoxDefault.SetActive(false);
					hitBoxStruck.SetActive(true);
				}

                PlayMusic(StruckAnimAudio, false);
            }
        }

        void PlayMusic(AudioClip sentClip, bool loopConfig)
        {
            if (GetComponent<AudioSource>() == null)
            {
                return;
            }

            if (sentClip != null)
            {
                AudioSource audio = GetComponent<AudioSource>();

                if (audio.isPlaying && (audio.clip == sentClip))
                {
                    return;
                }

                audio.Stop();

                audio.clip = sentClip;
                audio.loop = loopConfig;

                audio.Play();
            }
        }

        void OnDeathGOManagement()
        {
			//Play the Death Music and Destroy the HitboxStruck
            PlayMusic(deathAudio, false);

			if (hitBoxStruck != null) 
			{
				Destroy (hitBoxStruck);
			}
        }
    }
}
