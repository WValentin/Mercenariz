using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NavMeshPlus;

public abstract class CreatureBehavior : MonoBehaviour
{
    public Creature _creature;
    public NavMeshAgent navAgent;
    public float idleTimer = 0f;


    private float idleMoveRadius = 25f;
    private float idleMoveInterval = 5f;
    private float idleMoveTimer = 0f;


    public abstract void CustomUpdate();

    public CreatureBehavior(Creature creature)
    {
        _creature = creature;
    }

    public void IdleWander()
    {
        idleMoveTimer -= Time.deltaTime;

        if (idleMoveTimer <= 0f)
        {
            // Get a random position on the NavMesh within the move radius
            Vector3 randomDirection = Random.insideUnitSphere * idleMoveRadius;
            randomDirection += _creature.transform.position;
            NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, idleMoveRadius, NavMesh.AllAreas);

            MoveTo(hit.position);

            idleMoveTimer = idleMoveInterval;
        }
    }

    public void MoveTo(Vector3 destination)
    {
        // Set the agent's destination to desired position
        navAgent.SetDestination(destination);

        // Calculate the direction to the destination
        Vector3 direction = destination - _creature.transform.position;
        direction.y = 0f;

        // Rotate the transform to face the destination
        if (direction.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
            _creature.transform.rotation = Quaternion.Slerp(_creature.transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    public bool CheckForArrivingAtDestination()
    {
        if (!navAgent.pathPending && navAgent.remainingDistance < 0.01f)
            return true;
        return false;
    }
}