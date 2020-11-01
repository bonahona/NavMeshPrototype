using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pursue", menuName = "State/Pursue")]
public class PursueState : BehaviourState
{
    public float RepathTimer = 0.25f;

    public class PursueStateInstance: StateInstance {
        public Agent Target;
        public float CurrentRepathTimer;

        public PursueStateInstance(BehaviourState state, Agent target): base(state)
        {
            Target = target;
        }
    }

    public override StateInstance CreateInstance(BehaviourContext context)
    {
        return new PursueStateInstance(this, context.Target);
    }

    public override AgentInput Execute(StateInstance instance, BehaviourContext context)
    {
        var pursueInstance = instance as PursueStateInstance;
        pursueInstance.CurrentRepathTimer -= Time.deltaTime;

        if(pursueInstance.CurrentRepathTimer <= 0) {
            pursueInstance.CurrentRepathTimer = RepathTimer;
            context.SetTarget(pursueInstance.Target.transform.position);
        }

        var direction = (context.AiAgent.ProxyObject.nextPosition - context.Agent.transform.position).normalized;
        var result = new AgentInput {
            MovementDirection = direction,
            RotationDirection = direction,
            Done = false
        };

        return result;
    }


}
