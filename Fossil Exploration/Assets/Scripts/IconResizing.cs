using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IconResizing : MonoBehaviour {

    [SerializeField]
    RectTransform[] fossilIconBGImages, fossilIconFGImages;

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

    int size = 130;
    int position = 140;

	// Use this for initialization
	void Start () {
        touchManager.OnTouchAdded += TouchAdded;

        UpdateIcons();
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

        PositionAndSizeIcons(fossilIconBGImages);
        PositionAndSizeIcons(fossilIconFGImages);

        foreach(FossilIcon f in fossilIcons)
        {
            f.Init();
        }
    }

    /// <summary>
    /// Place icons in a regularly spaced circle
    /// </summary>
    /// <param name="icons"></param>
    void PositionAndSizeIcons(RectTransform[] icons)
    {
        int num = icons.Length;
        float angleIncrement = Mathf.PI * 2 / num;
        float startAngle = Mathf.PI / 2;

        for(int i = 0; i < num; i++)
        {
            float x = Mathf.Cos(startAngle + i * angleIncrement) * position;
            float y = Mathf.Sin(startAngle + i * angleIncrement) * position;
            icons[i].anchoredPosition = new Vector2(x, y);

            icons[i].sizeDelta = new Vector2(size, size);
        }
    }
}
