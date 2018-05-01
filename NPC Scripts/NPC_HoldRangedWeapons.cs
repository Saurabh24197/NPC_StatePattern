using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
	public class NPC_HoldRangedWeapons : MonoBehaviour 
	{

        private NPC_StatePattern npcStatePattern;
        private Animator myAnimator;

        public Transform rightHandTarget;
        public Transform leftHandTarget;

		void Start () 
		{
            SetInitialReferences();
		}

		void SetInitialReferences()
		{
            npcStatePattern = GetComponent<NPC_StatePattern>();
            myAnimator = GetComponent<Animator>();
		}

		void OnAnimatorIK()
        {
            if (npcStatePattern.rangedWeapon == null)
            {
                return;
            }

            if (myAnimator.enabled)
            {
                if (npcStatePattern.rangedWeapon.activeSelf)
                {
                    if (rightHandTarget != null)
                    {
                        myAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                        myAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);

                        myAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
                        myAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);
                    }

                    if (leftHandTarget != null)
                    {
                        myAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                        myAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);

                        myAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
                        myAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
                    }
                }
            }

        }
	}
}
