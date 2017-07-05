using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowTarget : MonoBehaviour {

    public GameObject m_target;


    private NavMeshAgent m_navAgent;


    void Start()
    {
        NavMeshSurface[] navMeshSurface = FindObjectsOfType<NavMeshSurface>();

        for (int i = 0; i < navMeshSurface.Length; i++)
        {
            navMeshSurface[i].enabled = false;
        }

        m_navAgent = GetComponent<NavMeshAgent>();

        for (int i = 0; i < navMeshSurface.Length; i++)
        {
            navMeshSurface[i].enabled = true;
        }
    }

    void Update () {

        if (m_navAgent.isOnNavMesh)
        {
            m_navAgent.SetDestination(m_target.transform.position);
        }
	}
}
