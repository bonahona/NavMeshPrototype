using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "PursueState", menuName = "States/Pursue")]
public class PursueState : BaseState
{
    public float DistanceTreshold;
    public float RepathTreshold;
    public float RepathTimer;

    public override StateInstance CreateInstance(Agent agent)
    {
        var target = FindObjectOfType<Player>();
        var result =  new StateInstance { Agent = agent, Target = target.GetComponent<Agent>() };

        Repath(result);

        return result;
    }

    public void Repath(StateInstance stateInstance)
    {
        NavMesh.CalculatePath(stateInstance.Agent.transform.position, stateInstance.Target.transform.position, int.MaxValue, stateInstance.Path);
        stateInstance.Index = 0;
        stateInstance.Timer = RepathTimer;
    }

    public bool ShouldRepath(StateInstance stateInstance)
    {
        if((stateInstance.Target.transform.position - stateInstance.Agent.transform.position).sqrMagnitude > (RepathTreshold * RepathTreshold)) {
            return true;
        }

        return false;
    }

    public override Steering GetSteering(StateInstance stateInstance)
    {
        stateInstance.Timer -= Time.deltaTime;
        if(stateInstance.Timer <= 0) {
            if (ShouldRepath(stateInstance)) {
                Repath(stateInstance);
            }
        }

        if(stateInstance.Path != null && stateInstance.Path.corners.Length > 1) {
            var corners = stateInstance.Path.corners;
            for (int i = 0; i < corners.Length - 1; i ++) {
                Debug.DrawLine(corners[i], corners[i + 1], Color.magenta);
            }
        }

        var position = stateInstance.Agent.transform.position;
        var targetPosition = position;
        if (stateInstance.Path.status == NavMeshPathStatus.PathComplete) {

            var distance = (stateInstance.Path.corners[stateInstance.Index] - position);

            if (distance.sqrMagnitude < DistanceTreshold * DistanceTreshold) {
                stateInstance.Index++;
            }

            if(stateInstance.Index == stateInstance.Path.corners.Length) {
                Repath(stateInstance);
            }

            targetPosition = stateInstance.Path.corners[stateInstance.Index];
        }

        var direction = (targetPosition - stateInstance.Agent.transform.position).normalized;
        var result = new Steering {
            MovementDirection = direction,
            RotationDirection = direction
        };

        if (UseAvoidObstacles) {
            AvoidObstacles(stateInstance, result);
        }

        return result;
    }
}
