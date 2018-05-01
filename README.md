# NPC_StatePattern
NPC Statepattern is a State based AI that was implemented from GTGDS3, to Expunge!
The NPC's perform different actions as defined from their behaviour Scripts.
The NPC's are able to perform Melee as well as Ranged Attacks. 

## Contents
These are the different modules that work for the NPC, to handle their behaviour
and manage the components attached to their GameObject.

### NPC_Master.cs
```
NPC Master has the variable declarations for NPC Events and Animations.

Events that increase/decrease NPC-Health, handle NPC-Death and Relations between NPC Factions
and a Standard set of animation states like Idle, Walk, Attack, Struck and Recovered have been defined;
more can be created by defining the Events and boolean in this Script.
```

### NPC_StatePattern.cs
```
State Pattern defines all the State variables and sets-up the different state references.
The methods are used to initialize the state variables [SetInitialReferences(), SetupStateReferences(), OnEnable()]
perform state updates [CarryOutUpdateState()],
and activate state actions[ ActivatePatrolState(), ActivateFleeState(), OnEnemyAttack(), ... ].

Other behaviour Scripts will refer the NPCState_Interface variables (currentState/capturedState) to update some of the States.
```

### NPC_StateBehaviours
These are the different behaviours for the NPC.

* NPCState_Patrol
* NPCState_Alert
* NPCState_Pursue
* NPCState_Flee
* NPCState_Follow
* NPCState_MeleeAttack
* NPCState_RangedAttack
* NPCState_Struck
* NPCState_InvestigateHarm

### NPC_* Scripts
These scripts manage the different components attached to the gameobject.
They also manage the actions to perfrom for some of the states.

* NPC_Animations
* NPC_CollisionField
* NPC_DropItems
* NPC_HeadLook
* NPC_Health
* NPC_HoldRangedWeapons
* NPC_RagdollActivation
* NPC_SetMyAttacker
* NPC_StateColour
* NPC_TakeDamage

These are optional Scripts that perform npc actions but may or may not use the NPC_StatePattern.
They might refer to scripts in [BaseFramework](https://github.com/Saurabh24197/BaseFramework)
* NPC_OnHeadCollision
* NPC_ScoresUpdate
* NPC_HazardousSkin

### NPC Relations
Defines the NPC_Relations (NPC Factions).
- Attached to NPC game object
* NPCRelationsDataStructures
* NPC_ApplyRelations

- Attached to GameManager game object (See [BaseFramework](https://github.com/Saurabh24197/BaseFramework) Game-Manager Scripts)
* GameManager_NPCRelationsMaster
* GameManager_NPCRelationsProcessor
* GameManager_NPCRelationsUI


- Attached to Destructible Items (to make NPC's react on destroy of these Items)
* Destructible_NPCRelation

### NPCTO Scripts
Used to disable (Turn-off) components on death.

* NPCTO_Animator
* NPCTO_ChildGameObjects
* NPCTO_NavMeshAgent
* NPCTO_StatePattern
------------------------------------------------------------------------------------------------

# Expunge! v1.2 01-01-2018
```
Expunge is a Procedurally generated FPS Arcade.
All levels are randomly generated with enemies spawning with one objective, 
Find and eliminate the Player. Fight through a lair of Golems with Guns and Ammo. 
Pick up Health when you're hurt and score points for each enemy eliminated.
```

### You can find the game here
* [Itch.io](https://nogtx.itch.io/expunge) - Download
* [Indie DB](http://www.indiedb.com/games/expunge) - Download
* [Steam](http://steamcommunity.com/groups/Expunge_Game) - Steam Group
------------------------------------------------------------------------------------------------

## Acknowledgments

A Special thanks to  *Looqmaan Ali aka GTGD* who provided Game Development Tutorial
based on which these Scripts were created.

Created by Saurabh K
