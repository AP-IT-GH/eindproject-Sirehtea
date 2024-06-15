using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using static UnityEngine.GraphicsBuffer;

public class GuardController : Agent
{
    // Hunter variables
    [SerializeField] private float moveSpeed = 6f;
    private Rigidbody rb;

    public GameObject prey;
    public List<GameObject> walls; // List to store all the wall objects
    public GameObject interiorWallsParent; // Parent GameObject for interior walls
    public GameObject exteriorWallsParent; // Parent GameObject for exterior walls

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        // Initialize the list of walls
        walls = new List<GameObject>();

        // Add all the walls from the Interior_walls and Exterior_walls parent objects
        foreach (Transform wallTransform in interiorWallsParent.GetComponentsInChildren<Transform>(true))
        {
            if (wallTransform.gameObject != interiorWallsParent)
            {
                walls.Add(wallTransform.gameObject);
            }
        }

        foreach (Transform wallTransform in exteriorWallsParent.GetComponentsInChildren<Transform>(true))
        {
            if (wallTransform.gameObject != exteriorWallsParent)
            {
                walls.Add(wallTransform.gameObject);
            }
        }
    }

    public override void OnEpisodeBegin()
    {
        // Hunter
        Vector3 spawnLocation = new Vector3(Random.Range(-48f, 48f), 0.3f, Random.Range(-48f, 48f)); // will spawn it randomly on this position (the position is decided by the maze size)

        bool distanceGood = CheckOverlap(prey.transform.localPosition, spawnLocation, 5f);
        bool wallGood = !CheckOverlapWithWalls(spawnLocation, 5f); // Check if the spawn location is not too close to any wall

        while (!distanceGood || !wallGood)
        {
            spawnLocation = new Vector3(Random.Range(-48f, 48f), 0.3f, Random.Range(-48f, 48f)); // will spawn it randomly on this position (the position is decided by the maze size)
            distanceGood = CheckOverlap(prey.transform.localPosition, spawnLocation, 5f);
            wallGood = !CheckOverlapWithWalls(spawnLocation, 5f); // Check again for walls
        }

        transform.localPosition = spawnLocation;
    }

    public bool CheckOverlap(Vector3 objectWeWantToAvoidOverlapping, Vector3 alreadyExistingObject, float minDistanceWanted)
    {
        float distanceBetweenObjects = Vector3.Distance(objectWeWantToAvoidOverlapping, alreadyExistingObject); // get distance between the objects
        return minDistanceWanted <= distanceBetweenObjects;
    }

    public bool CheckOverlapWithWalls(Vector3 spawnLocation, float minDistanceWanted)
    {
        foreach (GameObject wall in walls)
        {
            if (!CheckOverlap(wall.transform.localPosition, spawnLocation, minDistanceWanted))
            {
                return true;
            }
        }
        return false;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveRotate = actions.ContinuousActions[0];
        float moveForward = actions.ContinuousActions[1];

        rb.MovePosition(transform.position + transform.forward * moveForward * moveSpeed * Time.deltaTime);
        transform.Rotate(0f, moveRotate * moveSpeed, 0f, Space.Self); // rotate only on Y-axis
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Agent")
        {
            AddReward(10f);
            EndEpisode();
        }
        if (other.gameObject.tag == "Wall")
        {
            AddReward(-15f);
            EndEpisode();
        }
    }
}
