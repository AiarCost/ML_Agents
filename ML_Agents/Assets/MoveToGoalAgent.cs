using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent
{

    [SerializeField] private Transform targetTransform;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorRenderer;

    public override void OnEpisodeBegin()
    {
       transform.localPosition = new Vector3(Random.Range(-6f, -1.5f),4.5f, Random.Range(-4f, +2f));
    //    targetTransform.localPosition = new Vector3(Random.Range(2f, 5f), 4.5f, Random.Range(-4.25f, 2f));
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        Debug.Log(actions.ContinuousActions[0]);

        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        float moveSpeed = 3f;

        transform.position += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
       ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    public void OnTriggerEnter(Collider col)
    {

        if (col.TryGetComponent<Goal>(out Goal goal))
        {
            SetReward(1f);
            floorRenderer.material = winMaterial;
            EndEpisode();
        }
        if (col.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-1f);
            floorRenderer.material = loseMaterial;
            EndEpisode();
        }


    }
}
