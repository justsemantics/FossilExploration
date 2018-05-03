using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointOfInterest : MonoBehaviour{

    [SerializeField]
    string header, content;

    [SerializeField]
    Vector2 infoPanelLocation;

    public InfoText Info;

    public string Header { get { return header; } }
    public string Content { get { return content; } }
    public Vector2 InfoPanelLocation { get { return infoPanelLocation; } }

    public Quaternion Rotation;

    private void Start()
    {
        Rotation = Quaternion.FromToRotation(transform.forward, Vector3.forward);
    }
}
