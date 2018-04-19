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

    public InfoText Info;

    RectTransform rectTransform;

    private float rectSize;

	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        rectSize = rectTransform.sizeDelta.x;
	}
	
	// Update is called once per frame
	void Update () {
        CheckVisibility();
	}

    public void Show()
    {
        image.color = Color.red;
        rectTransform.sizeDelta = new Vector2(rectSize, rectSize);
    }

    public void Hide()
    {
        image.color = Color.clear;
        rectTransform.sizeDelta = Vector2.zero;
    }

    public void MoveTo(Vector2 position)
    {
        rectTransform.anchoredPosition = position;
    }

    void CheckVisibility()
    {
        MoveTo(cam.WorldToScreenPoint(POI.transform.position));

        RaycastHit hitInfo = new RaycastHit();

        Ray ray = new Ray(cam.transform.position, POI.transform.position - cam.transform.position);

        bool hit = Physics.Raycast(ray, out hitInfo);

        if (!hit)
        {
            Show();
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
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Info.Open = !Info.Open;
    }
}
