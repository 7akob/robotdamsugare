using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class Controller : Agent
{
    public float moveSpeed = 2f;
    public float rotSpeed = 2f;
    Vector3 StartPosition;
    public GameObject Dust;
    float PreviousDistance = 0;

    public override void OnEpisodeBegin()
    {
        Reset();
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float CurrentDistance = Vector3.Distance(transform.localPosition, Dust.transform.localPosition);
        float Delta = PreviousDistance - CurrentDistance;

        if(PreviousDistance != 0)
            AddReward(Delta * 0.01f);

        PreviousDistance = CurrentDistance;

        AddReward(-0.0001f);

        var action = actions.DiscreteActions;
        if (action[0] == 1)
        {
            MoveForward();
        }
        if (action[0] == 2)
        {
            MoveBackward();
        }
        if (action[1] == 1)
        {
            TurnRight();
        }
        if (action[1] == 2)
        {
            TurnLeft();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var action = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.UpArrow)) action[0] = 1;
        else if (Input.GetKey(KeyCode.DownArrow)) action[0] = 2;

        if (Input.GetKey(KeyCode.LeftArrow)) action[1] = 2;
        else if (Input.GetKey(KeyCode.RightArrow)) action[1] = 1; 
    }

    void Start()
    {
        StartPosition = transform.position;
        Reset();
    }

    void Reset()
    {
        transform.position = StartPosition;
        SpawnDust();
    }

    void SpawnDust()
    {
        Dust.transform.localPosition = new Vector3(Random.Range(8f, 18f), Dust.transform.localPosition.y, Random.Range(-0.8f, 8.8f));
    }

    void Update()
    {
        SteeringControl();
    }

    void SteeringControl()
    {
        if (Input.GetKey(KeyCode.UpArrow)) MoveForward();
        else if (Input.GetKey(KeyCode.DownArrow)) MoveBackward();

        if (Input.GetKey(KeyCode.LeftArrow)) TurnLeft();
        else if (Input.GetKey(KeyCode.RightArrow)) TurnRight();
    }

    void MoveForward()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
    }
    

    void MoveBackward()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
    }

    void TurnRight()
    {
        transform.Rotate(0, rotSpeed * Time.deltaTime, 0);
    }

    void TurnLeft()
    {
        transform.Rotate(0, -rotSpeed * Time.deltaTime, 0);
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Wall")
        {
            Debug.Log("Hit wall");
            AddReward(-1.0f);
            EndEpisode();
        }
        if (collision.gameObject.name == "Dust")
        {
            Debug.Log("Success!");
            AddReward(1.0f);
            EndEpisode();
        }
    }
}
