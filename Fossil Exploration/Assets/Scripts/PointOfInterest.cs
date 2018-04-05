using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointOfInterest : MonoBehaviour{

    [SerializeField]
    string header, content;

    public InfoText Info;

    public string Header { get { return header; } }
    public string Content { get { return content; } }

    public Quaternion Rotation;

    private void Start()
    {
        Rotation = Quaternion.FromToRotation(transform.forward, Vector3.forward);
    }
}
