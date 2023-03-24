using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NavMeshPlus;

public abstract class BuilderBehavior: CreatureBehavior
{
    public Builds builds;
    public bool isBuilding;
    public StructurePlan assignedBuildPlan;

    private float workingMoveInterval = 10f;
    private float workingMoveTimer = 0f;

    public BuilderBehavior(Creature creature) : base(creature)
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == assignedBuildPlan.gameObject && (_creature._state == States.Working_Moving_To_Work || _creature._state == States.Working))
            navAgent.areaMask -= 1 << NavMesh.GetAreaFromName("Road");
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject == assignedBuildPlan.gameObject && _creature._state == States.Working_FinnishBuilding)
        {
            navAgent.areaMask -= 1 << NavMesh.GetAreaFromName("BuildPlan");
            assignedBuildPlan.ReplaceWithStructure();
            assignedBuildPlan = null;
            _creature._state = States.Idle_Wandering;
        }
    }

    public void WorkingWander()
    {
        workingMoveTimer -= Time.deltaTime;

        if (workingMoveTimer <= 0f)
        {
            // Get a random position on the NavMesh within the build plan
            float radius = Mathf.Sqrt(assignedBuildPlan.transform.position.x * assignedBuildPlan.transform.position.x 
                                  + assignedBuildPlan.transform.position.y * assignedBuildPlan.transform.position.y);
            NavMesh.SamplePosition(assignedBuildPlan.transform.position, out NavMeshHit hit, radius, NavMesh.AllAreas);

            MoveTo(hit.position);

            workingMoveTimer = workingMoveInterval;
        }
    }

    public void CheckForLeavingBuildPlan()
    {

    }

    public void BuildFinished()
    {
        navAgent.areaMask += 1 << NavMesh.GetAreaFromName("Road");
        _creature._state = States.Working_FinnishBuilding;

        // Calculate a point outside of the area
        float radius = Mathf.Sqrt(assignedBuildPlan.transform.position.x * assignedBuildPlan.transform.position.x 
                                  + assignedBuildPlan.transform.position.y * assignedBuildPlan.transform.position.y);
        NavMesh.SamplePosition(assignedBuildPlan.transform.position, out NavMeshHit exitPoint, radius + 1, (NavMesh.AllAreas - (1 << NavMesh.GetAreaFromName("BuildPlan"))));

        // Set the agent's destination to the exit point
        MoveTo(exitPoint.position);
    }

    public void AssingToBuildPlan(StructurePlan buildPlan)
    {
        assignedBuildPlan = buildPlan;
    }

    public abstract void CheckForWork();

    public abstract bool FindBuildPlan();
}

public abstract class Builds
{
    public GameObject road = Resources.Load("Prefabs/Structures/Road") as GameObject;
}
