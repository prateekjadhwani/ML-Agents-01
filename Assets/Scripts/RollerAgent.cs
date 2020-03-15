using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RollerAgent : Agent
{
    Rigidbody rb;

    public Transform target;
    public float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void AgentReset()
    {
        if(this.transform.localPosition.y < 0)
        {
            this.rb.angularVelocity = Vector3.zero;
            this.rb.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0f, 0.5f, 0f);
        }

        target.localPosition = new Vector3(Random.value * 8 - 4,
                                      0.5f,
                                      Random.value * 8 - 4);
    }

    public override void CollectObservations()
    {
        AddVectorObs(target.localPosition);
        AddVectorObs(this.transform.localPosition);
        AddVectorObs(rb.velocity.x);
        AddVectorObs(rb.velocity.z);
    }

    public override void AgentAction(float[] vectorAction)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        rb.AddForce(controlSignal * speed);

        float distanceToTarget = Vector3.Distance(target.localPosition, this.transform.localPosition);

        if(distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            Done();
        }

        if(this.transform.localPosition.y < 0)
        {
            Done();
        }
    }

    public override float[] Heuristic()
    {
        var action = new float[2];
        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
        return action;
    }
}
