using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Fossil : MonoBehaviour {

    PointOfInterest[] pointsOfInterest = null;

    [SerializeField]
    string header, content;


    public PointOfInterest[] PointsOfInterest { get { return pointsOfInterest; } }
    public string Header { get { return header; } }
    public string Content { get { return content; } }

	// Use this for initialization
	void Start () {
        pointsOfInterest = GetComponentsInChildren<PointOfInterest>();

        Debug.Log(pointsOfInterest[0].name);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Setup(Transform targetLocation)
    {
        transform.position = targetLocation.position;
    }

    public void Store()
    {
        transform.position = GameObject.FindWithTag("Storage").transform.position;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Fossil))]
public class FossilEditor : Editor
{
    private bool addPOIMode = false;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (!addPOIMode)
        {
            if(GUILayout.Button("Add POIs"))
            {
                addPOIMode = true;
            }
        }
        else
        {
            if(GUILayout.Button("Done adding POIs"))
            {
                addPOIMode = false;
            }
        }
    }

    private bool CheckColliderParents(GameObject target, GameObject comparison)
    {
        //if they are the same
        if(target == comparison)
        {
            return true;
        }
        //otherwise check parents
        else
        {
            //if object has a parent, check it
            if(target.transform.parent != null)
            {
                return CheckColliderParents(target.transform.parent.gameObject, comparison);
            }
            //if no parents and no match, return false
            else
            {
                return false;
            }
        }
    }

    private void OnSceneGUI()
    {
        if (addPOIMode)
        {
            Event e = Event.current;

            // We use hotControl to lock focus onto the editor (to prevent deselection)
            int controlID = GUIUtility.GetControlID(FocusType.Passive);

            // put text by the mouse to remind user they are in POI adding mode
            Handles.Label(
                HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).GetPoint(1),
                "Click to add POI");

            switch (Event.current.GetTypeForControl(controlID))
            {
                case EventType.MouseDown:
                    //if right clicking while holding shift, place a new NavNode
                    if (e.button == 0)
                    {
                        GUIUtility.hotControl = controlID;
                        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                        {
                            if(CheckColliderParents(hit.collider.gameObject, Selection.activeGameObject))
                            {
                                Debug.Log(hit.point);

                                GameObject POI = Instantiate(Resources.Load<GameObject>("POI"), hit.point, Quaternion.identity, Selection.activeGameObject.transform);
                            }
                        }

                        Event.current.Use();
                    }
                    break;

                case EventType.MouseUp:
                    GUIUtility.hotControl = 0;
                    Event.current.Use();
                    break;
            }
        }
    }
}
#endif



