using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRotate : MonoBehaviour {

    [SerializeField]
    Camera cam;

    Coroutine rotationCoroutine;

    private Fossil currentFossil = null;

    public Fossil CurrentFossil { get { return currentFossil; } private set { currentFossil = value; } }

    private int primaryTouch = -1;
    private int secondaryTouch = -1;

    InputStateManager touchManager = null;

    [SerializeField]
    touchType screenType = touchType.left;


	// Use this for initialization
	void Start () {
        //cam = FindObjectOfType<Camera>();
        touchManager = FindObjectOfType<InputStateManager>();

        touchManager.OnTouchAdded += TouchAdded;
        touchManager.OnTouchRemoved += TouchRemoved;
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

    ////returns Vector2 of rotation based on mouse or touch input
    //Vector2 GetRotationInput()
    //{
    //    float rotX = 0, rotY = 0;
    //    if (Input.GetMouseButton(0))
    //    {
    //        rotX = Input.GetAxis("Mouse X") * mouseRotSpeed * Mathf.Deg2Rad;
    //        rotY = Input.GetAxis("Mouse Y") * mouseRotSpeed * Mathf.Deg2Rad;
    //    }
    //    if (Input.touchCount == 1)
    //    {
    //        rotX = Input.GetTouch(0).deltaPosition.x * touchRotSpeed * Mathf.Deg2Rad;
    //        rotY = Input.GetTouch(0).deltaPosition.y * touchRotSpeed * Mathf.Deg2Rad;
    //    }

    //    return new Vector2(rotX, rotY);
    //}

    //returns Vector2 of rotation based on mouse or touch input
    Vector2 GetRotationInput()
    {
        float rotX = 0, rotY = 0;
        //if (Input.GetMouseButton(0))
        //{
        //    rotX = Input.GetAxis("Mouse X") * mouseRotSpeed * Mathf.Deg2Rad;
        //    rotY = Input.GetAxis("Mouse Y") * mouseRotSpeed * Mathf.Deg2Rad;
        //}
        if (primaryTouch != -1 && secondaryTouch == -1)
        {
            rotX = touchManager.Touches[primaryTouch].touch.deltaPosition.x * touchRotSpeed * Mathf.Deg2Rad;
            rotY = touchManager.Touches[primaryTouch].touch.deltaPosition.y * touchRotSpeed * Mathf.Deg2Rad;
        }

        return new Vector2(rotX, rotY);
    }

    public void SelectFossil(Fossil fossil)
    {
        if(CurrentFossil != null)
            Destroy(CurrentFossil.gameObject);

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

        if (CurrentFossil != null)
        {
            CurrentFossil.transform.RotateAround(transform.position, Vector3.up, -newRotation.x - oldRotation.x);
            CurrentFossil.transform.RotateAround(transform.position, Vector3.right, newRotation.y + oldRotation.y);
        }

        if (newRotation.magnitude > rotMinThreshold)
        {
            oldRotation = newRotation;
            CancelRotateToPOI();
        }
    }

    //void UpdateZoom()
    //{
    //    if (Input.touchCount == 2)
    //    {
    //        // Store both touches.
    //        Touch touchZero = Input.GetTouch(0);
    //        Touch touchOne = Input.GetTouch(1);

    //        // Find the position in the previous frame of each touch.
    //        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
    //        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

    //        // Find the magnitude of the vector (the distance) between the touches in each frame.
    //        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
    //        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

    //        // Find the difference in the distances between each frame.
    //        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

    //        // If the cam is orthographic...
    //        if (cam.orthographic)
    //        {
    //            // ... change the orthographic size based on the change in distance between the touches.
    //            cam.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

    //            // Make sure the orthographic size never drops below zero.
    //            cam.orthographicSize = Mathf.Max(cam.orthographicSize, 0.1f);
    //        }
    //        else
    //        {
    //            // Otherwise change the field of view based on the change in distance between the touches.
    //            cam.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

    //            // Clamp the field of view to make sure it's between 0 and 180.
    //            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 20, 120);
    //        }
    //    }
    //    else
    //    {
    //        float clampedFOV = Mathf.Clamp(cam.fieldOfView, 40, 100);

    //        cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, clampedFOV, ref zoomSmoothDampVelocity, .3f);
    //    }
    //}

    void UpdateZoom()
    {
        if (primaryTouch != -1 && secondaryTouch != -1)
        {
            // Store both touches.
            Touch touchZero = touchManager.Touches[primaryTouch].touch;
            Touch touchOne = touchManager.Touches[secondaryTouch].touch;

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


            ////////////////////////////////////
            //Stuff for rotating gesture
            ////////////////////////////////////
            if (CurrentFossil != null)
            {
                float angle, prevAngle;

                angle = angleBetweenFingerPositions(touchZero.position, touchOne.position);
                prevAngle = angleBetweenFingerPositions(touchZeroPrevPos, touchOnePrevPos);

                float angleDelta = (angle - prevAngle) % 360;
                CurrentFossil.transform.RotateAround(transform.position, Vector3.forward, angleDelta);
            }
        }
        else
        {
            float clampedFOV = Mathf.Clamp(cam.fieldOfView, 40, 100);

            cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, clampedFOV, ref zoomSmoothDampVelocity, .3f);
        }
    }

    float angleBetweenFingerPositions(Vector2 posZero, Vector2 posOne)
    {
        float a = posOne.x - posZero.x;
        float o = posOne.y - posZero.y;
        float h = Vector2.Distance(posOne, posZero);
        float angle = Mathf.Asin(o / h) * Mathf.Rad2Deg;

        if(a <= 0)
        {
            angle = 180 - angle;
        }
        else if(o <= 0)
        {
            angle = 360 + angle;
        }

        return angle;
    }

    public void RotateToPOI(PointOfInterest POI)
    {
        CancelRotateToPOI();
        rotationCoroutine = StartCoroutine(RotateToPOICoroutine(POI, 1));
    }

    IEnumerator RotateToPOICoroutine(PointOfInterest POI, float time)
    {
        Quaternion initialRotation = CurrentFossil.transform.rotation;
        Quaternion targetRotation = Quaternion.identity * POI.Rotation;
        float timeStarted = Time.time;
        while (Time.time - timeStarted < time)
        {
            Quaternion rotation = Quaternion.Slerp(initialRotation, targetRotation, (Time.time - timeStarted) / time);
            Vector3 position = Vector3.Lerp(CurrentFossil.transform.position, (CurrentFossil.transform.position - POI.transform.position) + transform.position, 0.1f);
            CurrentFossil.transform.rotation = rotation;
            CurrentFossil.transform.position = position;
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

    private void TouchAdded(int fingerId, touchType type)
    {
        if (type == screenType)
        {
            if (primaryTouch == -1)
            {
                primaryTouch = fingerId;
            }
            else if(secondaryTouch == -1)
            {
                secondaryTouch = fingerId;
            }
        }
    }

    private void TouchRemoved(int fingerId)
    {
        if(primaryTouch == fingerId)
        {
            primaryTouch = secondaryTouch;
            secondaryTouch = -1;
        }
        else if(secondaryTouch == fingerId)
        {
            secondaryTouch = -1;
        }
    }
}
