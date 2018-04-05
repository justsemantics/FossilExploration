using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
