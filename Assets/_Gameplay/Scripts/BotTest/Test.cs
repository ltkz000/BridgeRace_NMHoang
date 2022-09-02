using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test : MonoBehaviour
{
    [SerializeField] private Transform test;
    [SerializeField] private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        agent.SetDestination(test.position);
    }
}
