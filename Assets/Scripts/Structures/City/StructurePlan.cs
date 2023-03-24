using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructurePlan : Structure
{
    public List<NPC> assignedBuilders = new List<NPC>();
    public float maxBuilders = 1;
    public Dictionary<int, MaterialData> requiredRessources;
    [SerializeField]
    private float buildingTimer = 30;

    [SerializeField]
    private bool exist = true;


    public StructurePlan(int width, int length, GameObject prefab)
    {
        _width = width;
        _length = length;
    }

    public void AssignBuilder(NPC builder)
    {
        assignedBuilders.Add(builder);
    }

    public void FinishBuilding()
    {
        foreach (NPC builder in assignedBuilders)
            (builder._behavior as BuilderBehavior).BuildFinished();
    }

    public GameObject ReplaceWithStructure()
    {
        if (exist)
        {
            GameObject structure;
            if (_structurePrefab.name != "Road")
                structure = GameObject.Instantiate(_structurePrefab, transform.position, transform.rotation, transform.parent.transform);
            else
                structure = GameObject.Instantiate(_structurePrefab, transform.position, transform.rotation, transform.parent.transform);
            Destroy(gameObject);
            exist = false;
            return structure;
        }
        return null;
    }

    public void AddRessourceToPlan(MaterialData ressource)
    {
        // TO DO
    }

    public void ReduceBuildTimer(NPC builder)
    {
        if (builder._state == States.Working)
        {
            // TO DO
            // Manage the builder's level when decreasing the timer
            // Reduce the work timer by the reduction rate times the time elapsed since the last frame
            buildingTimer -= 1f * Time.deltaTime;
            if (buildingTimer <= 0)
                FinishBuilding();
        }
    }

    public Dictionary<int, MaterialData> GetRequiredRessources()
    {
        return requiredRessources;
    }
}
