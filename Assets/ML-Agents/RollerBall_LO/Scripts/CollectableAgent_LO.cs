using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Collections.Generic;


public class CollectableAgent_LO : Agent
{
    private Rigidbody rBody;
    private QuestZone questManager;


    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        questManager = GetComponentInParent<QuestZone>();
    }

    [SerializeField] private Transform Target;
    [SerializeField] private Transform returnPosition;

    public override void OnEpisodeBegin()
    {
        // If the Agent fell, zero its momentum
        if (this.transform.localPosition.y < 0)
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.linearVelocity = Vector3.zero;
            this.transform.localPosition = this.returnPosition.localPosition;
        }                
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Si el Target no existe, enviamos Vector3.zero para evitar el error
        sensor.AddObservation(Target != null ? 1.0f : 0.0f);
        sensor.AddObservation(Target != null ? Target.localPosition : Vector3.zero);
        sensor.AddObservation(returnPosition.localPosition);
        sensor.AddObservation(this.transform.localPosition);
        // Agent velocity
        sensor.AddObservation(rBody.linearVelocity.x);
        sensor.AddObservation(rBody.linearVelocity.z);

        sensor.AddObservation(rBody.angularVelocity.x);
        sensor.AddObservation(rBody.angularVelocity.z);
    }

    public float forceMultiplier = 10;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        rBody.AddForce(controlSignal * forceMultiplier);

        AddReward(-0.001f);
        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.localPosition,
        (Target != null ? Target : returnPosition).localPosition);
        
        // Reached current destination
        if (distanceToTarget < 1.42f)
        {
            if (Target != null) // We reached the collectable target
            {
                AddReward(1.0f); // Reward for finding a target
                CollectableItem newTarget = questManager.Collect(Target.GetComponent<CollectableItem>());
                Target = newTarget != null ? newTarget.transform : null;
            }
            else // We reached the return position
            {
                if (rBody.linearVelocity.magnitude < 0.5f)
                {
                    AddReward(1.0f); // Final reward for bringing them back
                    EndEpisode();
                }
            }
        }
        // Fell off platform
        else if (this.transform.localPosition.y < 0)
        {
            AddReward(-1f);
            EndEpisode();
        }
    }

    public void ChangeTarget(Transform newTarget)
    {
        if(Target != null) return; // We already have a target, ignore the request

        EndEpisode();
        Target = newTarget;
        newTarget.parent = this.transform.parent;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }
}
