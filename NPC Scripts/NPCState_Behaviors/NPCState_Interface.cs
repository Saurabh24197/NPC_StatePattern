using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
	public interface NPCState_Interface
	{

        void UpdateState();
        void ToPatrolState();
        void ToAlertState();
        void ToPursueState();
        void ToMeleeAttackState();
        void ToRangedAttackState();
		
	}
}
