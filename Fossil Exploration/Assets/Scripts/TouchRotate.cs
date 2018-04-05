using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRotate : MonoBehaviour {

    [SerializeField]
    Camera cam;

    Coroutine rotationCoroutine;

    private Fossil currentFossil = null;

    public Fossil CurrentFossil { get { return currentFossil; } private set { currentFossil = value; } }

	// Use this for initialization
	void Start () {
        //cam = FindObjectOfType<Camera>();


	}
	
	// Update is called once per frame
	void Update () {
        UpdateRotation();
        UpdateZoom();
    }

    float zoomSmoothDampVelocity;

    [SerializeField]
    float touchRotSpeed = 3, mouseRotSpeed = 100, rotDecay, rotMinThreshold, orthoZoomSpeed, perspectiveZoomSpeed;

    Vector2 oldRotation = new Vector2();

    //returns Vector2 of rotation based on mouse or touch input
    Vector2 GetRotationInput()
    {
        float rotX = 0, rotY = 0;
        if (Input.GetMouseButton(0))
        {
            rotX = Input.GetAxis("Mouse X") * mouseRotSpeed * Mathf.Deg2Rad;
            rotY = Input.GetAxis("Mouse Y") * mouseRotSpeed * Mathf.Deg2Rad;
        }
        if (Input.touchCount == 1)
        {
            rotX = Input.GetTouch(0).deltaPosition.x * touchRotSpeed * Mathf.Deg2Rad;
            rotY = Input.GetTouch(0).deltaPosition.y * touchRotSpeed * Mathf.Deg2Rad;
        }

        return new Vector2(rotX, rotY);
    }

    public void SelectFossil(Fossil fossil)
    {
        CurrentFossil = fossil;
    }

    void UpdateRotation()
    {
        Vector2 newRotation = GetRotationInput();

        if (Mathf.Sign(Vector2.Dot(newRotation, oldRotation)) == -1)
        {
            oldRotation = Vector3.zero;
        }
        else
        {
            oldRotation = oldRotation * rotDecay;
        }

        CurrentFossil.transform.RotateAround(transform.position, Vector3.up, -newRotation.x - oldRotation.x);
        CurrentFossil.transform.RotateAround(transform.position, Vector3.right, newRotation.y + oldRotation.y);

        if (newRotation.magnitude > rotMinThreshold)
        {
            oldRotation = newRotation;
            CancelRotateToPOI();
        }
    }

    void UpdateZoom()
    {
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // If the cam is orthographic...
            if (cam.orthographic)
            {
                // ... change the orthographic size based on the change in distance between the touches.
                cam.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

                // Make sure the orthographic size never drops below zero.
                cam.orthographicSize = Mathf.Max(cam.orthographicSize, 0.1f);
            }
            else
            {
                // Otherwise change the field of view based on the change in distance between the touches.
                cam.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

                // Clamp the field of view to make sure it's between 0 and 180.
                cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 20, 120);
            }
        }
        else
        {
            float clampedFOV = Mathf.Clamp(cam.fieldOfView, 40, 100);

            cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, clampedFOV, ref zoomSmoothDampVelocity, .3f);
        }
    }

    public void RotateToPOI(PointOfInterest POI)
    {
        Quaternion initialRotation = CurrentFossil.transform.rotation;
        Quaternion targetRotation = Quaternion.identity * POI.Rotation;

        CancelRotateToPOI();
        rotationCoroutine = StartCoroutine(RotateToPOICoroutine(initialRotation, targetRotation, 1));
    }

    IEnumerator RotateToPOICoroutine(Quaternion initialRotation, Quaternion targetRotation, float time)
    {
        float timeStarted = Time.time;

        while (Time.time - timeStarted < time)
        {
            Quaternion rotation = Quaternion.Slerp(initialRotation, targetRotation, (Time.time - timeStarted) / time);
            CurrentFossil.transform.rotation = rotation;
            yield return null;
        }
    }

    void CancelRotateToPOI()
    {
        if(rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
        }
    }
}
