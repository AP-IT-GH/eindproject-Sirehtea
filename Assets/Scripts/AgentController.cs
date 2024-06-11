using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Integrations.Match3;

public class AgentController : Agent
{
    // Target variables
    [SerializeField] private Transform target;
    public int targetCount;
    public GameObject food;
    [SerializeField] private List<GameObject> spawnedTargetList = new List<GameObject>();

    // Agent variables
    [SerializeField] private float moveSpeed = 4f;
    private Rigidbody rb;

    // Environment variables
    [SerializeField] private Transform environmentLocation;
    Material envMaterial;
    public GameObject env;

    //Time keeping variables
    [SerializeField] private int timeForEpisode;
    private float timeLeft;

    //Enemy Agent
    public HunterController classObject;


    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        envMaterial = env.GetComponent<Renderer>().material;
    }

    public override void OnEpisodeBegin()
    {
        //Agent
        transform.localPosition = new Vector3(Random.Range(-48, 48f), 0.3f, Random.Range(-48f, 48f)); //will spawn it randomly on this position (the position is decided by the maze size)

        //Target
        createTarget();
        //target.localPosition = new Vector3(Random.Range(-4f, 4f), 0.3f, Random.Range(-4f, 4f)); //will spawn it randomly on this position

        //Timer to determine if Agent is taking too long and needs to be punished
        EpisodeTimerNew();
    }

    private void Update()
    {
        CheckRemainingTime(); //  every frame time will be checked
    }

    private void createTarget()
    {
        if (spawnedTargetList.Count != 0)
        {
            RemoveTarget(spawnedTargetList);
        }

        for (int i = 0; i < targetCount; i++)
        {
            int counter = 0;
            bool distanceGood;
            bool alreadyDecremeneted = false;

            // Spawning target
            GameObject newTarget = Instantiate(food);
            // Make target child of the environment
            newTarget.transform.parent = environmentLocation;
            // Give random spawn location
            Vector3 targetLocation = new Vector3(Random.Range(-48f, 48f), 0.3f, Random.Range(-48f, 48f));

            if (spawnedTargetList.Count != 0)
            {
                for (int k = 0; k < spawnedTargetList.Count; k++)
                {
                    if (counter > 10)
                    {
                        distanceGood = CheckOverlap(targetLocation, spawnedTargetList[k].transform.localPosition, 5f);
                        if (distanceGood == false)
                        {
                            targetLocation = new Vector3(Random.Range(-48f, 48f), 0.3f, Random.Range(-48f, 48f));
                            k--;
                            alreadyDecremeneted = true;
                            Debug.Log("Too close to other Target");
                        }

                        distanceGood = CheckOverlap(targetLocation, transform.localPosition, 5f);
                        if (distanceGood == false)
                        {
                            Debug.Log("Too close to Agent");
                            targetLocation = new Vector3(Random.Range(-48f, 48f), 0.3f, Random.Range(-48f, 48f));
                            if (alreadyDecremeneted == false)
                            {
                                k--;
                            }
                        }
                        counter++;
                    }
                    else
                    {
                        k = spawnedTargetList.Count;
                    }
                }
            }

            // Spawn in new location
            newTarget.transform.localPosition = targetLocation;
            // Add to list
            spawnedTargetList.Add(newTarget);
        }
    }

    public bool CheckOverlap(Vector3 objectWeWantToAvoidOverlapping, Vector3 alreadyExistingObject, float minDistanceWanted)
    {
        float distanceBetweenObjects = Vector3.Distance(objectWeWantToAvoidOverlapping, alreadyExistingObject); // get distance between the objects
        if (minDistanceWanted <= distanceBetweenObjects)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void RemoveTarget(List<GameObject> toBeDeletedGameObjectList)
    {
        foreach (GameObject item in toBeDeletedGameObjectList)
        {
            Destroy(item.gameObject);
        }
        toBeDeletedGameObjectList.Clear();
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
        if (other.gameObject.tag == "Target")
        {
            // Remove from list
            spawnedTargetList.Remove(other.gameObject);
            Destroy(other.gameObject);

            AddReward(10f);
            if (spawnedTargetList.Count == 0)
            {
                envMaterial.color = Color.green;
                RemoveTarget(spawnedTargetList);
                AddReward(5f);
                classObject.AddReward(-5f);
                classObject.EndEpisode();
                EndEpisode();
            }
        }
        if (other.gameObject.tag == "Wall")
        {
            envMaterial.color = Color.red;
            RemoveTarget(spawnedTargetList);
            AddReward(-15f);
            classObject.EndEpisode();
            EndEpisode();
        }
    }

    private void EpisodeTimerNew()
    {
        timeLeft = Time.time + timeForEpisode;
    }

    private void CheckRemainingTime()
    {
        if (Time.time >= timeLeft)
        {
            envMaterial.color = Color.blue;
            AddReward(-15f);
            classObject.AddReward(-15f);
            RemoveTarget(spawnedTargetList);
            classObject.EndEpisode();
            EndEpisode();
        }
    }
}