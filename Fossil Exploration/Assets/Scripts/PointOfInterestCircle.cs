using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// This class controls a circle that indicates where the fossil's Points of Interest are.
/// When the Point of Interest is behind the fossil, the circle shrinks into a small dot.
/// </summary>
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(RectTransform))]
public class PointOfInterestCircle : MonoBehaviour, IPointerClickHandler {

    Image image;

    /// <summary>
    /// Camera used for raycasting.
    /// </summary>
    [HideInInspector]
    public Camera cam;

    /// <summary>
    /// Associated PointOfInterest.
    /// </summary>
    [HideInInspector]
    public PointOfInterest POI;

    /// <summary>
    /// How precise to be when checking whether POI is visible to the camera.
    /// Lower numbers are more precise
    /// </summary>
    [HideInInspector]
    public float sensitivity = 0.01f;

    [Tooltip("Opacity when POI is hidden.")]
    public float minOpacity = 0.2f;

    [Tooltip("Opacity when POI is shown.")]
    public float maxOpacity = 1f;

    [Tooltip("RectTransform of dot image.")]
    public RectTransform innerCircle;

    /// <summary>
    /// Associated InfoText.
    /// </summary>
    [HideInInspector]
    public InfoText Info;

    RectTransform rectTransform;

    //The image may change sizes at runtime, but the limits are set between 
    //the original size of the large circle and the original size of the small dot
    private float rectOriginalSize, innerCircleSize;

	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        rectOriginalSize = rectTransform.sizeDelta.x;
        innerCircleSize = innerCircle.sizeDelta.x;

        //Other UI elements should appear on top of the POI Circle
        transform.SetAsFirstSibling();
	}
	
	// Update is called once per frame
	void Update () {
        UpdatePosition();
        CheckVisibility();
	}

    /// <summary>
    /// Shows the default PointOfInterestCircle graphic
    /// </summary>
    void Show()
    {
        image.color = new Color(1, 1, 1, maxOpacity);
        innerCircle.sizeDelta = Vector2.zero;
        rectTransform.sizeDelta = new Vector2(rectOriginalSize, rectOriginalSize);
    }

    /// <summary>
    /// Shows the small dot to indicate the POI is hidden behind the fossil
    /// </summary>
    void Hide()
    {
        image.color = Color.clear;
        rectTransform.sizeDelta = Vector2.zero;
        innerCircle.sizeDelta = new Vector2(innerCircleSize, innerCircleSize);
    }

    /// <summary>
    /// Moves the image to the position so you don't have to deal with RectTransform stuff every time
    /// </summary>
    /// <param name="position">The position in pixel coordinates to move to</param>
    public void MoveTo(Vector2 position)
    {
        //Account for the parent's position as the Image is anchored within it
        RectTransform parent = rectTransform.parent.GetComponent<RectTransform>();
        float x = parent.anchorMin.x * Screen.width + parent.offsetMin.x;
        float y = parent.anchorMin.y * Screen.height + parent.offsetMin.y;
        Vector2 parentPosition = new Vector2(x, y);

        rectTransform.anchoredPosition = position - parentPosition;
    }

    /// <summary>
    /// Changes the size of the circle when the point of interest is being viewed at an oblique angle
    /// </summary>
    /// <param name="normal">Either the normal of a raycasthit or the forward vector of a POI's transform</param>
    /// <param name="comparison">Direction to compare normal with, usually forward vector of the camera</param>
    void AdjustSize(Vector3 normal, Vector3 comparison)
    {
        float amount = Vector3.Dot(comparison, normal);

        //boost the value so that the circle is "full size" as long as the angle is close enough
        amount = Mathf.Clamp01(amount * 1.3f);

        float size = Mathf.Lerp(innerCircleSize, rectOriginalSize, amount);
        rectTransform.sizeDelta = new Vector2(size, size);
    }

    /// <summary>
    /// Moves the image to stay over the POI
    /// </summary>
    void UpdatePosition()
    {
        MoveTo(cam.WorldToScreenPoint(POI.transform.position));
    }

    /// <summary>
    /// Does a raycast to decide whether the camera can see the POI
    /// </summary>
    void CheckVisibility()
    {
        RaycastHit hitInfo = new RaycastHit();

        Ray ray = new Ray(cam.transform.position, POI.transform.position - cam.transform.position);

        bool hit = Physics.Raycast(ray, out hitInfo);

        if (hit)
        {
            float distance = Vector3.Distance(POI.transform.position, hitInfo.point);

            if (distance < sensitivity)
            {
                Show();
            }
            else //ray hit the object before getting near the POI, so the POI is blocked by geometry
            {
                Hide();
            }

            AdjustSize(POI.transform.forward, cam.transform.forward);
        }
        else
        {
            Hide();
        }
    }

    //Using this instead of rolling it into the InputStateManager because this captures only taps or clicks,
    //so it doesn't trigger when you are trying to zoom / rotate / drag
    public void OnPointerClick(PointerEventData eventData)
    {
        Info.Open = !Info.Open;
    }
}
