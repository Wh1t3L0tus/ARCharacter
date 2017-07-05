using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshUpdater : MonoBehaviour {

    private NavMeshSurface m_navSurface;
    private float m_elapsedTime;

	void Start () {
        m_navSurface = GetComponent<NavMeshSurface>();

        if (m_navSurface == null)
        {
            Debug.Log("Cannot find navmesh surface");
        }
	}
	
	void Update () {

        m_elapsedTime += Time.deltaTime;

        if (m_elapsedTime > 2.0f)
        {
            m_elapsedTime = 0.0f;
            m_navSurface.Bake();
        }
	}
}
