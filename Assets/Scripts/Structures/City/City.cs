using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NavMeshPlus.Components;

public class City : MonoBehaviour
{
    public List<GameObject> roads;
    public List<GameObject> structures;
    public List<NPC> npcs;
    public List<Structure> structuresQueue;
    public List<GameObject> buildPlans;
    public CityType cityType;


    private NavMeshSurface cityNavMeshSurface;
    private GameObject roadPrefab;
    private GameObject housePlanPrefab;
    private GameObject npcPrefab;


    private void Start()
    {
        // TO DO
        // Randomize or determine what type of city it is
        cityType = CityType.Circler;


        AssignPrefabs();


        // Test
        // roads.Add(GameObject.Instantiate((Resources.Load("Prefabs/Structures/Road") as GameObject), transform.position, Quaternion.identity, transform.GetChild(0).transform));
        // cityNavMeshSurface = transform.GetChild(0).transform.GetComponent<NavMeshSurface>();
        cityNavMeshSurface = transform.GetComponent<NavMeshSurface>();
        cityNavMeshSurface.BuildNavMeshAsync();

        // Test (10 houses)
        StartCoroutine(DetermineNextBuild());
        BuildHousePlan();
        BuildHousePlan();
        BuildHousePlan();
        BuildHousePlan();
        BuildHousePlan();
        BuildHousePlan();
        BuildHousePlan();
        BuildHousePlan();
        BuildHousePlan();
        BuildHousePlan();
        BuildHousePlan();
        BuildHousePlan();
        BuildHousePlan();
        BuildHousePlan();
        BuildHousePlan();
        BuildHousePlan();
        BuildHousePlan();
        BuildHousePlan();
        BuildHousePlan();
        BuildHousePlan();
        BuildHousePlan();
        BuildHousePlan();
        BuildHousePlan();
        BuildHousePlan();
        BuildHousePlan();
        BuildHousePlan();
        BuildHousePlan();
        BuildHousePlan();
        BuildHousePlan();
        BuildHousePlan();
        SpawnNpc();
        SpawnNpc();
        // SpawnNpc();
        // SpawnNpc();
        // SpawnNpc();
        // SpawnNpc();
        // SpawnNpc();
        // SpawnNpc();
        // SpawnNpc();
        // SpawnNpc();
    }

    // We define the prefabs to use depending on the type of city
    private void AssignPrefabs()
    {
        // TO DO
        // Assign prefabs for other types of cities
        switch(cityType)
        {
            case CityType.Circler:
                roadPrefab = Resources.Load("Prefabs/Structures/Road") as GameObject;
                housePlanPrefab = Resources.Load("Prefabs/Structures/CirclerHousePlan") as GameObject;
                npcPrefab = Resources.Load("Prefabs/Creatures/NPC/Circler") as GameObject;
                break;
            default:
                break;
        }
    }
 
    // Function to calculate the builds queue
    private IEnumerator DetermineNextBuild()
    {
        // foreach (Structure structure in structures)
        structuresQueue.Add(new CirclerHouse());
        structuresQueue.Add(new CirclerHouse());
        structuresQueue.Add(new CirclerHouse());
        structuresQueue.Add(new CirclerHouse());
        structuresQueue.Add(new CirclerHouse());
        yield return null;
    }

    void BuildHousePlan()
    {
        Vector2 buildingPosition = new Vector2();
        foreach(GameObject road in roads)
        {
            if (road.transform.rotation.z == 0)
                buildingPosition = FindBuildingLocationHorizontal(new Vector2(road.transform.position.x, road.transform.position.y), structuresQueue[0]._width, structuresQueue[0]._length);
            else
                buildingPosition = FindBuildingLocationVertical(new Vector2(road.transform.position.x, road.transform.position.y), structuresQueue[0]._width, structuresQueue[0]._length);
            if (!buildingPosition.Equals(Vector2.zero))
            {
                // TO DO
                // Replace with house plan prefab
                GameObject housePlan = GameObject.Instantiate(housePlanPrefab, new Vector3(buildingPosition.x, buildingPosition.y, 0), Quaternion.identity, transform.GetChild(1).transform);
                // We synchronise after instantiating the prefab, so that its collider will be used during calculations and so we avoid any problem cause by it being instantiated the same frame on which a calculation runs.
                Physics2D.SyncTransforms();
                cityNavMeshSurface.UpdateNavMesh(cityNavMeshSurface.navMeshData);
                buildPlans.Add(housePlan);
                break;
            }
        }
        if (buildingPosition.Equals(Vector2.zero) && roads.Count < 40)
        {
            BuildRoadPlan();
            BuildHousePlan();
        }
    }

    // Checks and find if there is available space around an horizontal road to build a specific building.
    // Also returns the best space (ex: spot with less trees)
    Vector2 FindBuildingLocationHorizontal(Vector2 roadPosition, int buildingWidth, int buildingLength)
    {
        // Create a list of all possible locations for the building
        List<Vector2> buildingLocations = new List<Vector2>();
        for (float x = roadPosition.x - 10 + (buildingWidth / 2); x <= roadPosition.x + 10 - (buildingWidth / 2); x++)
        {
            for (float y = roadPosition.y - 2 - (buildingLength / 2); y <= roadPosition.y + 2 + (buildingLength / 2); y++)
            {
                if (y == roadPosition.y - 1 - (buildingLength / 2))
                    y += buildingLength + 3;
                buildingLocations.Add(new Vector2(x, y));
            }
        }
    
        // Check each locations for obstacles and create a list with the best locations in it
        List<Vector2> bestLocations = new List<Vector2>();
        int minObstacles = int.MaxValue - 1;
        foreach (Vector2 location in buildingLocations)
        {
            int obstacles = 0;
            Collider2D[] hits = Physics2D.OverlapAreaAll(new Vector2(location.x - (buildingWidth / 2) + 0.1f, location.y + (buildingLength / 2) - 0.1f),
                                                         new Vector2(location.x + (buildingWidth / 2) - 0.1f, location.y - (buildingLength / 2) + 0.1f));
            foreach (Collider2D hit in hits)
            {
                // TO DO
                // Modify to manage more tags
                if (hit.CompareTag("Structure"))
                {
                    obstacles = int.MaxValue;
                    break;
                }
                if (hit.CompareTag("Tree"))
                    obstacles++;
            }
            if (obstacles <= minObstacles)
            {
                minObstacles = obstacles;
                bestLocations.Add(location);
            }
        }

        // Return one of the random best positions
        if (bestLocations.Count > 0)
        {
            int randomIndex = Random.Range(0, bestLocations.Count);
            return bestLocations[randomIndex];
        }
    
        return new Vector2();
    }

    // Checks and find if there is available space around a vertical road to build a specific building.
    // Also returns the best space (ex: spot with less trees)
    Vector2 FindBuildingLocationVertical(Vector2 roadPosition, int buildingWidth, int buildingLength)
    {
        // Create a list of all possible locations for the building
        List<Vector2> buildingLocations = new List<Vector2>();
        float x = roadPosition.x - 2 - (buildingLength / 2);
        for (float y = roadPosition.y + 10 - (buildingWidth / 2); y >= roadPosition.y - 10 + (buildingWidth / 2); y--)
        {
            buildingLocations.Add(new Vector2(x, y));

            if (y == roadPosition.y - 10 + (buildingWidth / 2) && x < roadPosition.x)
            {
                y = roadPosition.y + 10 - (buildingWidth / 2) + 1;
                x = roadPosition.x + 2 + (buildingLength / 2);
            }
        }
    
        // Check each locations for obstacles and create a list with the best locations in it
        List<Vector2> bestLocations = new List<Vector2>();
        int minObstacles = int.MaxValue - 1;
        foreach (Vector2 location in buildingLocations)
        {
            int obstacles = 0;
            Collider2D[] hits = Physics2D.OverlapAreaAll(new Vector2(location.x - (buildingWidth / 2) + 0.1f, location.y + (buildingLength / 2) - 0.1f),
                                                         new Vector2(location.x + (buildingWidth / 2) - 0.1f, location.y - (buildingLength / 2) + 0.1f));
            foreach (Collider2D hit in hits)
            {
                // TO DO
                // Modify to manage more tags
                if (hit.CompareTag("Structure"))
                {
                    obstacles = int.MaxValue;
                    break;
                }
                if (hit.CompareTag("Tree"))
                    obstacles++;
            }
            if (obstacles <= minObstacles)
            {
                minObstacles = obstacles;
                bestLocations.Add(location);
            }
        }

        // Return one of the random best positions
        if (bestLocations.Count > 0)
        {
            int randomIndex = Random.Range(0, bestLocations.Count);
            return bestLocations[randomIndex];
        }
    
        return new Vector2();
    }

    void BuildRoadPlan()
    {
        Vector2 roadPosition = new Vector2();
        bool rotated = (Random.Range(0, 2) == 1);
        if (rotated)
            roadPosition = FindNewRoadLocationVertical();
        else
            // TO DO
            // Make the function for horizontal roads
            roadPosition = FindNewRoadLocationHorizontal();
        if (roadPosition.Equals(Vector2.zero))
        {
            if (!rotated)
                roadPosition = FindNewRoadLocationVertical();
            else
                // TO DO
                // Make the function for horizontal roads
                roadPosition = FindNewRoadLocationHorizontal();
            rotated = !rotated;
        }
        GameObject newRoad = GameObject.Instantiate(roadPrefab, new Vector3(roadPosition.x, roadPosition.y, 0), Quaternion.identity, transform.GetChild(0).transform);
        if (rotated)
            newRoad.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
        // We synchronise after instantiating the prefab, so that its collider will be used during calculations and so we avoid any problem cause by it being instantiated the same frame on which a calculation runs.
        Physics2D.SyncTransforms();
        // We update the navmesh for the roads of the city.
        cityNavMeshSurface.UpdateNavMesh(cityNavMeshSurface.navMeshData);
        roads.Add(newRoad);
    }

    // Checks and find if there is available space around the city's roads to place a new vertical road.
    // Also returns the best space (ex: spot with less trees)
    Vector2 FindNewRoadLocationVertical()
    {
        int roadWidth = 20;
        int roadLength = 4;

        // Create a list of all possible locations for the road
        // Also check for each road if it is rotated or not to change the calculations
        List<Vector2> newRoadLocations = new List<Vector2>();
        foreach(GameObject road in roads)
        {
            // The road we check from isn't rotated, so the available spots are for new roads that are perpendicular to that road
            if (road.transform.rotation.z == 0)
            {
                float x = road.transform.position.x - (roadWidth / 2) - (roadLength / 2);
                for (float y = road.transform.position.y + (roadWidth / 2) - (roadLength / 2); y > road.transform.position.y - (roadWidth / 2) + (roadLength / 2); y--)
                {
                    newRoadLocations.Add(new Vector2(x, y));

                    if (y == road.transform.position.y - (roadWidth / 2) + (roadLength / 2) && x < road.transform.position.x)
                    {
                        y = road.transform.position.y + (roadWidth / 2) - (roadLength / 2);
                        x = road.transform.position.x + (roadWidth / 2) + (roadLength / 2);
                    }
                }
            }
            // The road we check from is rotated, so the available spots are for new roads that follow the same direction
            else
            {
                newRoadLocations.Add(new Vector2(road.transform.position.x, road.transform.position.y + roadWidth));
                newRoadLocations.Add(new Vector2(road.transform.position.x, road.transform.position.y - roadWidth));
            }
        }
    
        // Check each locations for obstacles and create a list with the best locations in it
        List<Vector2> bestLocations = new List<Vector2>();
        int minObstacles = int.MaxValue - 1;
        foreach (Vector2 location in newRoadLocations)
        {
            int obstacles = 0;
            Collider2D[] hits = Physics2D.OverlapAreaAll(new Vector2(location.x - (roadLength / 2) + 0.1f, location.y + (roadWidth / 2) - 0.1f),
                                                         new Vector2(location.x + (roadLength / 2) - 0.1f, location.y - (roadWidth / 2) + 0.1f));
            foreach (Collider2D hit in hits)
            {
                // TO DO
                // Modify to manage more tags
                if (hit.CompareTag("Structure"))
                {
                    obstacles = int.MaxValue;
                    break;
                }
                if (hit.CompareTag("Tree"))
                    obstacles++;
            }
            if (obstacles <= minObstacles)
            {
                minObstacles = obstacles;
                bestLocations.Add(location);
            }
        }

        // Return one of the random best positions
        if (bestLocations.Count > 0)
        {
            int randomIndex = Random.Range(0, bestLocations.Count);
            return bestLocations[randomIndex];
        }
    
        return new Vector2();
    }

    // Checks and find if there is available space around the city's roads to place a new horizontal road.
    // Also returns the best space (ex: spot with less trees)
    Vector2 FindNewRoadLocationHorizontal()
    {
        int roadWidth = 20;
        int roadLength = 4;

        // Create a list of all possible locations for the road
        // Also check for each road if it is rotated or not to change the calculations
        List<Vector2> newRoadLocations = new List<Vector2>();
        foreach(GameObject road in roads)
        {
            // The road we check from is rotated, so the available spots are for new roads that follow the same direction
            if (road.transform.rotation.z != 0)
            {
                float y = road.transform.position.y + (roadWidth / 2) + (roadLength / 2);
                for (float x = road.transform.position.x - (roadWidth / 2) + (roadLength / 2); x <= road.transform.position.x + (roadWidth / 2) - (roadLength / 2); x++)
                {
                    newRoadLocations.Add(new Vector2(x, y));

                    if (x == road.transform.position.x + (roadWidth / 2) - (roadLength / 2) && y > road.transform.position.y)
                    {
                        x = road.transform.position.x - (roadWidth / 2) + (roadLength / 2) - 1;
                        y = road.transform.position.y - (roadWidth / 2) - (roadLength / 2);
                    }
                }
            }
            // The road we check from isn't rotated, so the available spots are for new roads that are perpendicular to that road
            else
            {
                newRoadLocations.Add(new Vector2(road.transform.position.x + roadWidth, road.transform.position.y));
                newRoadLocations.Add(new Vector2(road.transform.position.x - roadWidth, road.transform.position.y));
            }
        }
    
        // Check each locations for obstacles and create a list with the best locations in it
        List<Vector2> bestLocations = new List<Vector2>();
        int minObstacles = int.MaxValue - 1;
        foreach (Vector2 location in newRoadLocations)
        {
            int obstacles = 0;
            Collider2D[] hits = Physics2D.OverlapAreaAll(new Vector2(location.x - (roadWidth / 2) + 0.1f, location.y + (roadLength / 2) - 0.1f),
                                                         new Vector2(location.x + (roadWidth / 2) - 0.1f, location.y - (roadLength / 2) + 0.1f));
            foreach (Collider2D hit in hits)
            {
                // TO DO
                // Modify to manage more tags
                if (hit.CompareTag("Structure"))
                {
                    obstacles = int.MaxValue;
                    break;
                }
                if (hit.CompareTag("Tree"))
                    obstacles++;
            }
            if (obstacles <= minObstacles)
            {
                minObstacles = obstacles;
                bestLocations.Add(location);
            }
        }

        // Return one of the random best positions
        if (bestLocations.Count > 0)
        {
            int randomIndex = Random.Range(0, bestLocations.Count);
            return bestLocations[randomIndex];
        }
    
        return new Vector2();
    }

    public void removeBuildPlan(GameObject buildPlan, GameObject newStructure)
    {
        buildPlans.Remove(buildPlan);
        structures.Add(newStructure);

        // We update both the pyshics and the navmesh
        Physics2D.SyncTransforms();
        cityNavMeshSurface.UpdateNavMesh(cityNavMeshSurface.navMeshData);
    }

    public void SpawnNpc()
    {
        GameObject npc = GameObject.Instantiate(npcPrefab, transform.position, Quaternion.identity, transform.GetChild(2).transform);
        npc.GetComponent<NPC>().JoinCity(this);
        npcs.Add(npc.GetComponent<NPC>());
    }

    public void AssignBuilderToBuildPlan(GameObject buildPlan)
    {
        
    }


    public enum CityType
    {
        Circler
    }
}
