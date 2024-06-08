using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using static UnityEngine.GraphicsBuffer;

public class HunterController : Agent
{
    //Hunter variables
    [SerializeField] private float moveSpeed = 6f;
    private Rigidbody rb;

    // Environment variables
    Material envMaterial;
    public GameObject env;

    public GameObject prey;
    public AgentController classObject;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        envMaterial = env.GetComponent<Renderer>().material;
    }

    public override void OnEpisodeBegin()
    {

        //Hunter
        Vector3 spawnLocation = new Vector3(Random.Range(-4f, 4f), 0.3f, Random.Range(-4f, 4f)); //will spawn it randomly on this position (the position is decided by the maze size)

        bool distanceGood = classObject.CheckOverlap(prey.transform.localPosition, spawnLocation, 5f);

        while (!distanceGood)
        {
            spawnLocation = new Vector3(Random.Range(-4f, 4f), 0.3f, Random.Range(-4f, 4f)); //will spawn it randomly on this position (the position is decided by the maze size)
            distanceGood = classObject.CheckOverlap(prey.transform.localPosition, spawnLocation, 5f);
        }

        transform.localPosition = spawnLocation;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        //sensor.AddObservation(target.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveRotate = actions.ContinuousActions[0];
        float moveForward = actions.ContinuousActions[1];

        rb.MovePosition(transform.position + transform.forward * moveForward * moveSpeed * Time.deltaTime);
        transform.Rotate(0f, moveRotate * moveSpeed, 0f, Space.Self); //rotate only on Y-axis

        //Vector3 velocity = new Vector3(moveX, 0f, moveZ);
        //velocity = velocity.normalized * Time.deltaTime * moveSpeed;

        //transform.localPosition += velocity;

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

}
