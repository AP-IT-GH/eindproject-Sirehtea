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
    [SerializeField] private float rotationSpeed = 100f;
    private Rigidbody rb;

    // Last known position of the prey
    private Vector3 lastKnownPosition;
    private bool knowsPreyPosition = false;

    // Boundaries of the environment
    private float envBoundary = 48f;

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

        // Reset last known position
        lastKnownPosition = Vector3.zero;
        knowsPreyPosition = false;
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
        sensor.AddObservation(lastKnownPosition);
        sensor.AddObservation(knowsPreyPosition);

        // Additional observations
        Vector3 directionToPrey = (knowsPreyPosition ? lastKnownPosition : prey.transform.localPosition) - transform.localPosition;
        sensor.AddObservation(directionToPrey.normalized);
        sensor.AddObservation(Vector3.Distance(transform.localPosition, knowsPreyPosition ? lastKnownPosition : prey.transform.localPosition));
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveRotate = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        float moveForward = Mathf.Clamp(actions.ContinuousActions[1], 0f, 1f);

        if (knowsPreyPosition)
        {
            Vector3 directionToPrey = lastKnownPosition - transform.localPosition;
            directionToPrey.y = 0; // Ignore y-axis
            Quaternion lookRotation = Quaternion.LookRotation(directionToPrey);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.Rotate(0f, moveRotate * rotationSpeed * Time.deltaTime, 0f, Space.Self); // rotate only on Y-axis
        }

        rb.MovePosition(transform.position + transform.forward * moveForward * moveSpeed * Time.deltaTime);

        // Update last known position if the prey is in sight
        if (CanSeePrey())
        {
            lastKnownPosition = prey.transform.localPosition;
            knowsPreyPosition = true;
            AddReward(0.1f); // Reward for seeing the prey
        }

        // Penalize for staying near walls or corners
        if (IsNearWallOrCorner())
        {
            AddReward(-0.05f);
        }

        // Penalize for excessive rotation
        if (!knowsPreyPosition && Mathf.Abs(moveRotate) > 0.5f)
        {
            AddReward(-0.01f);
        }

        // Penalize for not moving
        if (moveForward == 0)
        {
            AddReward(-0.01f);
        }
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

    private bool CanSeePrey()
    {
        RaycastHit hit;
        Vector3 directionToPrey = prey.transform.localPosition - transform.localPosition;
        if (Physics.Raycast(transform.localPosition, directionToPrey, out hit))
        {
            if (hit.collider.gameObject == prey)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsNearWallOrCorner()
    {
        // Check if the hunter is near the walls or corners
        return transform.localPosition.x < -envBoundary + 5f || transform.localPosition.x > envBoundary - 5f ||
               transform.localPosition.z < -envBoundary + 5f || transform.localPosition.z > envBoundary - 5f;
    }


}