using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;

public class CarBrain : Agent
{
    Vector3 startPosition;
    private PrometeoCarController carController;

    public Transform Target;
    float PreviousDistance;


    public override void OnEpisodeBegin()
    {
        Reset();
        PreviousDistance = Vector3.Distance(transform.position, Target.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var action = actions.DiscreteActions;
        if (action[0] == 1)
        {
            carController.GoForward();
        }
        if (action[0] == 2)
        {
            carController.GoReverse();
        }
        if (action[1] == 1)
        {
            carController.TurnRight();
        }
        if (action[1] == 2)
        {
            carController.TurnLeft();
        }

        AddReward(-0.001f);

        float distanceToTarget = Vector3.Distance(transform.position, Target.position);
        float progressReward = PreviousDistance - distanceToTarget;
        AddReward(progressReward * 0.1f);
        PreviousDistance = distanceToTarget;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var action = actionsOut.DiscreteActions;
        action[0] = 0;
        action[1] = 0;

        if (Input.GetKey(KeyCode.W)) action[0] = 1;
        else if (Input.GetKey(KeyCode.S)) action[0] = 2;

        if (Input.GetKey(KeyCode.D)) action[1] = 1;
        else if (Input.GetKey(KeyCode.A)) action[1] = 2;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
        carController = GetComponent<PrometeoCarController>();
        Reset();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Reset()
    {
        transform.position = startPosition;
        transform.rotation = Quaternion.identity;

        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cone"))
        {
            Debug.Log("Hit Cone");
            AddReward(-1.0f);
            EndEpisode();
        }
        if (collision.gameObject.CompareTag("Finish"))
        {
            Debug.Log("Success!");
            AddReward(1.0f);
            EndEpisode();
        }
    }
}
