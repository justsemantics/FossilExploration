using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(RectTransform))]
public class PointOfInterestCircle : MonoBehaviour, IPointerClickHandler {

    Image image;

    public Camera cam;

    public PointOfInterest POI;

    public float sensitivity = 0.1f;

    public float minOpacity = 0.2f;
    public float maxOpacity = 1f;

    public RectTransform innerCircle;

    public InfoText Info;

    RectTransform rectTransform;

    private float rectOriginalSize, innerCircleSize;

	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        rectOriginalSize = rectTransform.sizeDelta.x;
        innerCircleSize = innerCircle.sizeDelta.x;
	}
	
	// Update is called once per frame
	void Update () {
        CheckVisibility();
	}

    public void Show()
    {
        image.color = Color.white;
        innerCircle.sizeDelta = Vector2.zero;
        rectTransform.sizeDelta = new Vector2(rectOriginalSize, rectOriginalSize);
    }

    public void Hide()
    {
        image.color = Color.clear;
        rectTransform.sizeDelta = new Vector2(innerCircleSize, innerCircleSize);
        innerCircle.sizeDelta = new Vector2(innerCircleSize, innerCircleSize);
    }

    public void MoveTo(Vector2 position)
    {
        RectTransform parent = rectTransform.parent.GetComponent<RectTransform>();
        float x = parent.anchorMin.x * Screen.width + parent.offsetMin.x;
        float y = parent.anchorMin.y * Screen.height + parent.offsetMin.y;
        Vector2 parentPosition = new Vector2(x, y);

        rectTransform.anchoredPosition = position - parentPosition;
    }

    void AdjustSize(Vector3 normal, Vector3 comparison)
    {
        float amount = Vector3.Dot(comparison, normal);

        amount = Mathf.Clamp01(amount * 1.3f);

        float size = Mathf.Lerp(innerCircleSize, rectOriginalSize, amount);
        rectTransform.sizeDelta = new Vector2(size, size);
    }

    void CheckVisibility()
    {
        MoveTo(cam.WorldToScreenPoint(POI.transform.position));

        RaycastHit hitInfo = new RaycastHit();

        Ray ray = new Ray(cam.transform.position, POI.transform.position - cam.transform.position);

        bool hit = Physics.Raycast(ray, out hitInfo);

        if (!hit)
        {
            Hide();
        }
        else
        {
            float distance = Vector3.Distance(POI.transform.position, hitInfo.point);

            Debug.DrawLine(cam.transform.position, hitInfo.point);

            if (distance < sensitivity)
            {
                Show();
            }
            else
            {
                Hide();
            }

            AdjustSize(POI.transform.forward, transform.forward);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Info.Open = !Info.Open;
    }
}
