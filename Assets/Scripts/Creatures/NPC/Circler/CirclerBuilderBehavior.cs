using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CirclerBuilderBehavior: BuilderBehavior
{
    int test = 1;


    public override void CustomUpdate()
    {
        switch(_creature._state)
        {
            case States.Idle:
                // TO DO
                IdleTest();
                break;
            case States.Idle_Wandering:
                IdleWander();
                CheckForWork();
                break;
            case States.Working_Moving_To_Work:
                if (CheckForArrivingAtDestination())
                {
                    navAgent.areaMask -= 1 << NavMesh.GetAreaFromName("Road");
                    _creature._state = States.Working;
                }
                break;
            case States.Working:
                assignedBuildPlan.ReduceBuildTimer((_creature as NPC));
                WorkingWander();
                break;
            case States.Working_FinnishBuilding:
                if (CheckForArrivingAtDestination())
                {
                    GameObject structure = assignedBuildPlan.ReplaceWithStructure();
                    if (structure != null)
                    {
                        (_creature as NPC)._city.removeBuildPlan(assignedBuildPlan.gameObject, structure);
                        assignedBuildPlan = null;
                        _creature._state = States.Idle;
                    }
                }
                break;
            default:
                break;
        }

    }

    public CirclerBuilderBehavior(Creature creature) : base(creature)
    {
        navAgent = _creature.GetComponent<NavMeshAgent>();
        // Disable automatic rotation of the navAgent
        navAgent.updateRotation = false;
		navAgent.updateUpAxis = false;
        builds = new CirclerBuilds();
    }

    public override void CheckForWork()
    {
        if (assignedBuildPlan == null)
        {
            if(FindBuildPlan())
            {
                // TO DO
                // Make and call the function that checks if the ressources for the build are available/needed
                navAgent.areaMask += 1 << NavMesh.GetAreaFromName("BuildPlan");
                MoveTo(assignedBuildPlan.transform.position);
                _creature._state = States.Working_Moving_To_Work;
            }
        }
        else
        {
            // TO DO
            // Make and call the function that checks if the ressources for the build are available/needed
            navAgent.areaMask += 1 << NavMesh.GetAreaFromName("BuildPlan");
            MoveTo(assignedBuildPlan.transform.position);
            _creature._state = States.Working_Moving_To_Work;
        }
    }

    public override bool FindBuildPlan()
    {
        StructurePlan buildPlanToAssign = null;
        // Check every BuildPlan of the city and return the first with no builder working on it, or the first that needs workers the most
        foreach (GameObject buildPlan in (_creature as NPC)._city.buildPlans)
        {
            StructurePlan plan = buildPlan.GetComponent<StructurePlan>();
            if (plan.assignedBuilders.Count < plan.maxBuilders)
            {
                if (buildPlanToAssign != null)
                {
                    if ((plan.maxBuilders - plan.assignedBuilders.Count) < (buildPlanToAssign.maxBuilders - buildPlanToAssign.assignedBuilders.Count))
                        buildPlanToAssign = plan;
                }
                else   
                    buildPlanToAssign = plan;
            }
        }
        if (buildPlanToAssign == null)
            return false;
        else
        {
            AssingToBuildPlan(buildPlanToAssign);
            buildPlanToAssign.AssignBuilder((_creature as NPC));
            return true;
        }
    }

    public void IdleTest()
    {
        idleTimer += Time.deltaTime;
        if (idleTimer >= 5f)
        {
            idleTimer = 0;
            _creature._state = States.Idle_Wandering;
        }
    }
}

public class CirclerBuilds : Builds
{
    
}