using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateInstance
{
    public Agent Agent;
    public Agent Target;
    public NavMeshPath Path = new NavMeshPath();
    public int Index;
    public float Timer;
}
