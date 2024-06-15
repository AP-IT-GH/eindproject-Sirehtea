using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class HunterController : Agent
{
    // Hunter variables
    [SerializeField] private float moveSpeed = 6f;
    private Rigidbody rb;

    // Environment variables
    Material envMaterial;
    public GameObject env;

    public GameObject prey;
    public AgentController classObject;

    // Last known position of the prey
    private Vector3 lastKnownPosition;
    private bool knowsPreyPosition = false;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        envMaterial = env.GetComponent<Renderer>().material;
    }

    public override void OnEpisodeBegin()
    {
        // Hunter
        Vector3 spawnLocation = new Vector3(Random.Range(-48f, 48f), 0.3f, Random.Range(-48f, 48f)); // will spawn it randomly on this position (the position is decided by the maze size)

        bool distanceGood = classObject.CheckOverlap(prey.transform.localPosition, spawnLocation, 5f);

        while (!distanceGood)
        {
            spawnLocation = new Vector3(Random.Range(-48f, 48f), 0.3f, Random.Range(-48f, 48f)); // will spawn it randomly on this position (the position is decided by the maze size)
            distanceGood = classObject.CheckOverlap(prey.transform.localPosition, spawnLocation, 5f);
        }

        transform.localPosition = spawnLocation;

        // Reset last known position
        lastKnownPosition = Vector3.zero;
        knowsPreyPosition = false;
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
        float moveRotate = actions.ContinuousActions[0];
        float moveForward = actions.ContinuousActions[1];

        rb.MovePosition(transform.position + transform.forward * moveForward * moveSpeed * Time.deltaTime);
        transform.Rotate(0f, moveRotate * moveSpeed, 0f, Space.Self); // rotate only on Y-axis

        // Update last known position if the prey is in sight
        if (CanSeePrey())
        {
            lastKnownPosition = prey.transform.localPosition;
            knowsPreyPosition = true;
            AddReward(0.1f); // Reward for seeing the prey
        }

        // Guide hunter towards the last known position of the prey
        if (knowsPreyPosition)
        {
            Vector3 directionToPrey = lastKnownPosition - transform.localPosition;
            directionToPrey.y = 0; // Ignore y-axis
            directionToPrey.Normalize();

            rb.MovePosition(transform.position + directionToPrey * moveForward * moveSpeed * Time.deltaTime);

            // Reward for moving closer to the last known position
            float distanceToLastKnownPosition = Vector3.Distance(transform.localPosition, lastKnownPosition);
            AddReward(-distanceToLastKnownPosition * 0.01f); // Penalize for being far from the last known position
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
            classObject.AddReward(-13f);
            envMaterial.color = Color.yellow;
            classObject.EndEpisode();
            EndEpisode();
        }
        if (other.gameObject.tag == "Wall")
        {
            envMaterial.color = Color.red;
            AddReward(-15f);
            classObject.EndEpisode();
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
}
