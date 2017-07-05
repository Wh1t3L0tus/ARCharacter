using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnCharacterOnInteract : MonoBehaviour {

    public GameObject m_characterPrefab;

    private GameObject m_characterInstance;

    private TangoPointCloud m_pointCloud;

	void Start () {

        m_pointCloud = FindObjectOfType<TangoPointCloud>();
        if (m_pointCloud == null)
        {
            Debug.Log("Cannot find point cloud !");
        }
	}

    void RebuildNavmesh(GameObject go)
    {
        NavMeshSurface navSurf = go.GetComponent<NavMeshSurface>();
        navSurf.Bake();
    }
	
	void Update () {

        Vector2 interactionPos = new Vector3(0, 0, 0);
        if (UserIsInteracting(ref interactionPos))
        {
            Vector3 worldPos;
            if (FindMeshPointFromScreenCoordinates(interactionPos, out worldPos))
            {
                PlaceCharacter(worldPos);
            }
        }
	}

    bool FindMeshPointFromScreenCoordinates(Vector2 screenPos, out Vector3 worldPos)
    {
#if UNITY_EDITOR
        RaycastHit raycastHit;
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        worldPos = new Vector3(0, 0, 0);
        if (Physics.Raycast(ray, out raycastHit))
        {
            RebuildNavmesh(raycastHit.collider.gameObject);
            worldPos = raycastHit.point;
            return true;
        }

        return false;
#else
        Camera cam = Camera.main;
        Vector3 planeCenter;
        Plane plane;

        worldPos = new Vector3(0, 0, 0);

        if (m_pointCloud.FindPlane(cam, screenPos, out planeCenter, out plane) && Vector3.Angle(plane.normal, Vector3.up) < 30.0f)
        {
            worldPos = planeCenter;
            return true;
        }

        return false;
#endif
    }

    void PlaceCharacter(Vector3 pos)
    {
        m_characterInstance = Instantiate(m_characterPrefab);
        m_characterInstance.transform.position = pos + new Vector3(0, 1, 0) * m_characterInstance.GetComponent<Collider>().bounds.extents.y;

        FollowTarget follower = m_characterInstance.AddComponent<FollowTarget>();
        follower.m_target = Camera.main.gameObject;
    }

    bool UserIsInteracting(ref Vector2 interactionPos)
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0))
        {
            interactionPos = Input.mousePosition;
            return true;
        }
#else

            if (Input.touchCount == 1)
            {
                Touch t = Input.GetTouch(0);
                if (t.phase == TouchPhase.Ended)
                {
                    interactionPos = t.position;
                    return true;
                }
            }
#endif

        return false;
    }
}
