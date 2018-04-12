using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IconResizing : MonoBehaviour {

    [SerializeField]
    RectTransform[] upperFossilImages, lowerFossilImages;

    [SerializeField]
    FossilIcon[] fossilIcons;

    [SerializeField]
    GameObject increaseSize, decreaseSize, increaseDistance, decreaseDistance;

    [SerializeField]
    InputStateManager touchManager;

    [SerializeField]
    EventSystem eventSystem;

    [SerializeField]
    GraphicRaycaster raycaster;

    int size = 300;
    int position = 100;

	// Use this for initialization
	void Start () {
        touchManager.OnTouchAdded += TouchAdded;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void TouchAdded(int fingerId, touchType type)
    {
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.position = touchManager.Touches[fingerId].touch.position;
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(eventData, results);


        foreach (RaycastResult result in results)
        {
            if (result.gameObject == increaseDistance)
            {
                position += 10;
            }
            else if (result.gameObject == decreaseDistance)
            {
                position -= 10;
            }
            else if (result.gameObject == increaseSize)
            {
                size += 10;
            }
            else if (result.gameObject == decreaseSize)
            {
                size -= 10;
            }
        }

        UpdateIcons();
    }

    void UpdateIcons()
    {
        foreach(RectTransform t in upperFossilImages)
        {
            t.anchoredPosition = new Vector2(0, position);
            t.sizeDelta = new Vector2(size, size);
        }
        foreach(RectTransform t in lowerFossilImages)
        {
            t.anchoredPosition = new Vector2(0, -position);
            t.sizeDelta = new Vector2(size, size);
        }

        foreach(FossilIcon f in fossilIcons)
        {
            f.Init();
        }
    }
}
