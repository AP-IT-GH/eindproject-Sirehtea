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

    private Rigidbody rBody;
    private bool isChasing;
    private float chaseTime;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        // Freeze Y position and rotations to keep the agent grounded
        rBody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    public override void OnEpisodeBegin()
    {
        // Set the agent's position to the center of the maze
        transform.localPosition = new Vector3(0f, 0.5f, 0f);

        // Randomize the target's position ensuring it is not too close to the agent
        Vector3 targetPosition;
        float minDistance = 10f;  // Set the minimum distance between agent and target

        do
        {
            targetPosition = new Vector3(Random.Range(-4f, 4f), 0.5f, Random.Range(-4f, 4f));
        } while (Vector3.Distance(transform.localPosition, targetPosition) < minDistance);

        target.localPosition = targetPosition;

        isChasing = false;
        chaseTime = 0f;
        rBody.velocity = Vector3.zero;  // Reset velocity to prevent unintended movements
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Agent position
        sensor.AddObservation(transform.localPosition);
        // Target position
        sensor.AddObservation(target.localPosition);
        // Agent velocity
        sensor.AddObservation(rBody.velocity);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        Vector3 move = new Vector3(moveX, 0, moveZ) * (isChasing ? chaseSpeed : moveSpeed) * Time.deltaTime;
        rBody.AddForce(move, ForceMode.VelocityChange);

        // Reset vertical velocity to prevent flying
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
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 10f))
        {
            if (hit.collider.CompareTag("Player"))
            {
                StartChasing();
            }
        }
    }
} 
