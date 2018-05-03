using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Unused in the current build. This was a temporary class that resizes and repositions the FossilIcons in
/// the central selection area. They are automatically arranged in an evenly-spaced circle, and can be moved
/// closer together or farther apart, as well as resized.
/// 
/// This class is mostly useful for testing on multiple screen resolutions, as the scale of the UI can be different
/// from one screen to the next.
/// </summary>
public class IconResizing : MonoBehaviour {

    [SerializeField]
    private RectTransform[] fossilIconBGImages, fossilIconFGImages;

    [SerializeField]
    private FossilIcon[] fossilIcons;

    [SerializeField]
    private GameObject increaseSize, decreaseSize, increaseDistance, decreaseDistance;

    [SerializeField]
    private InputStateManager touchManager;

    [SerializeField]
    private EventSystem eventSystem;

    [SerializeField]
    private GraphicRaycaster raycaster;

    //totally arbitrary, happens to look good on 1920 x 1080
    private int size = 130;
    private int position = 140;

	// Use this for initialization
	private void Start () {
        touchManager.OnTouchAdded += TouchAdded;

        UpdateIcons();
	}
	
	// Update is called once per frame
	private void Update () {
		
	}

    /// <summary>
    /// Checks if new touches are on any of the control buttons
    /// </summary>
    /// <param name="fingerId"></param>
    /// <param name="type"></param>
    private void TouchAdded(int fingerId, touchType type)
    {
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.position = touchManager.Touches[fingerId].touch.position;
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(eventData, results);

        //if the raycast hits a button, increment the corresponding value
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

    //sets icon positions based on the size and distance settings, then calls Init() to save the new values
    private void UpdateIcons()
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
    private void PositionAndSizeIcons(RectTransform[] icons)
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
