using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class GuardAgent : Agent
{
    public float moveSpeed = 2f;
    public float chaseSpeed = 4f;
    public float chaseDuration = 2f;
    public Transform target;
    public float scanRotationSpeed = 2f;
    public LayerMask raycastLayerMask;

    private Rigidbody rBody;
    private bool isChasing;
    private float chaseTime;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        rBody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    public override void OnEpisodeBegin()
    {
        // Maze size and offsets
        float mazeSize = 100f;
        float offset = 0.1f; // To avoid spawning right on the edge or overlapping
        float minDistance = 1f; // Minimum distance between agent and target

        // Set agent position to the center of the maze
        transform.localPosition = new Vector3(0, 0.5f, 0);

        Vector3 targetPosition;

        // Ensure the target is not too close to the agent
        do
        {
            targetPosition = new Vector3(Random.Range(-mazeSize / 2 + offset, mazeSize / 2 - offset), 0.5f, Random.Range(-mazeSize / 2 + offset, mazeSize / 2 - offset));
        } while (Vector3.Distance(transform.localPosition, targetPosition) < minDistance);

        target.localPosition = targetPosition;

        isChasing = false;
        chaseTime = 0f;
        rBody.velocity = Vector3.zero;  // Reset velocity to prevent unintended movements
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(target.localPosition);
        sensor.AddObservation(rBody.velocity);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        Vector3 move = new Vector3(moveX, 0, moveZ) * (isChasing ? chaseSpeed : moveSpeed) * Time.deltaTime;
        rBody.AddForce(move, ForceMode.VelocityChange);

        Vector3 velocity = rBody.velocity;
        velocity.y = 0;
        rBody.velocity = velocity;

        if (isChasing)
        {
            chaseTime -= Time.deltaTime;
            if (chaseTime <= 0)
            {
                isChasing = false;
            }
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
        continuousActions[1] = Input.GetAxis("Vertical");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            AddReward(-1f);
            EndEpisode();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AddReward(1f);
            StartChasing();
        }
    }

    private void StartChasing()
    {
        isChasing = true;
        chaseTime = chaseDuration;
    }

    void Update()
    {
        // Scanning logic
        transform.Rotate(0, scanRotationSpeed * Time.deltaTime, 0);

        // Raycast logic
        Vector3[] rayDirections = { transform.forward, transform.right, -transform.right };
        foreach (var direction in rayDirections)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, 10f, raycastLayerMask))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    AddReward(0.5f);
                    StartChasing();
                }
            }
        }
    }
}