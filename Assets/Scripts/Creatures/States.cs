using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TO DO
// Change the structure of the code when there will be more states (an exemple is far below in here)

public enum States
{
    Idle,
    Idle_Wandering,
    Working,
    Working_Moving_To_Work,
    Working_FinnishBuilding,
}






























// Code for a more advanced number of states
//                      |
//                      |
//                      v




// public class StateManager
// {
//     public enum MainState
//     {
//         Idle,
//         working,
//         Attacking
//     }

//     public enum IdleSubState
//     {
//         Wander,
//         Chat,
//         Rest
//     }

//     public enum WorkingSubState
//     {
//         Building,
//         MovingToWorkPlace,
//         CollectingRessources,
//         Delivering
//     }

//     public enum FightingSubState
//     {

//     }

//     public Dictionary<MainState, System.Enum> SubStates = new Dictionary<MainState, System.Enum>()
//     {
//         { MainState.Idle, new IdleSubState() },
//         { MainState.working, new WorkingSubState() },
//         { MainState.Attacking, new FightingSubState() }
//     };

//     public void test()
//     {
//         StateManager stateManager = new StateManager();
//         // Set the NPC's state to Attacking
//         MainState StateMainState = StateManager.MainState.Attacking;

//         // Get the NPC's sub-state in the Attacking main state
//         StateSubState = (StateManager.WorkingSubState)stateManager.SubStates[StateMainState];
//     }
// }