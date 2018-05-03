using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Handles the process of selecting fossils,
/// from dragging FossilIcons into the left or right screen
/// to setting up connections between other scripts and new fossils
/// </summary>
public class FossilSelection : MonoBehaviour {

    [SerializeField]
    private TouchRotate leftScreenControl, rightScreenControl;

    [SerializeField]
    private Transform leftScreenFossilLocation, rightScreenFossilLocation;

    [SerializeField]
    private HighlightPointsOfInterest leftScreenHighlighter, rightScreenHighlighter;

    [SerializeField]
    private InputStateManager touchManager;

    [SerializeField]
    private EventSystem eventSystem;

    [SerializeField]
    private GraphicRaycaster raycaster;

    [SerializeField]
    private GameObject LeftReset, RightReset;

    [SerializeField]
    private GameObject LeftText, RightText;

    private FossilIcon lastLeftIcon, lastRightIcon;

    /// <summary>
    /// Icons that are currently being dragged.
    /// Key: fingerId,
    /// Value: FossilIcon
    /// </summary>
    Dictionary<int, FossilIcon> activeIcons = new Dictionary<int, FossilIcon>();

	// Use this for initialization
	private void Start () {
        touchManager.OnTouchAdded += TouchAdded;
        touchManager.OnTouchRemoved += TouchRemoved;
	}
	
	// Update is called once per frame
	private void Update () {
        foreach(KeyValuePair<int, FossilIcon> kvp in activeIcons)
        {
            kvp.Value.SetPosition(touchManager.Touches[kvp.Key].touch.position);
        }
	}

    /// <summary>
    /// Handles the OnTouchAdded event from the InputStateManager.
    /// </summary>
    /// <param name="fingerId"></param>
    /// <param name="type"></param>
    private void TouchAdded(int fingerId, touchType type)
    {
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.position = touchManager.Touches[fingerId].touch.position;
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(eventData, results);

        foreach(RaycastResult result in results)
        {
            if(result.gameObject == LeftReset)
            {
                selectFossil(lastLeftIcon.fossil, true);
            }
            if(result.gameObject == RightReset)
            {
                selectFossil(lastRightIcon.fossil, false);
            }

            FossilIcon icon = result.gameObject.GetComponent<FossilIcon>();

            if(icon != null)
            {
                activeIcons.Add(fingerId, icon);
                icon.Pickup();
            }
        }
    }

    /// <summary>
    /// Handles the OnTouchRemoved event from the InputStateManager
    /// </summary>
    /// <param name="fingerId"></param>
    private void TouchRemoved(int fingerId)
    {
        if (activeIcons.ContainsKey(fingerId))
        {
            if(activeIcons[fingerId].GetPosition().x < Screen.width * 0.4)
            {
                selectFossil(activeIcons[fingerId].fossil, true);
                lastLeftIcon = activeIcons[fingerId];
            }
            else if(activeIcons[fingerId].GetPosition().x > Screen.width * 0.6)
            {
                selectFossil(activeIcons[fingerId].fossil, false);
                lastRightIcon = activeIcons[fingerId];
            }

            activeIcons[fingerId].Return();

            activeIcons.Remove(fingerId);
        }
    }

    /// <summary>
    /// Creates a new instance of fossil and sets it up on the selected side of the screen.
    /// </summary>
    /// <param name="fossil">fossil prefab</param>
    /// <param name="leftScreen">true if fossil should be on the left, false on the right</param>
    private void selectFossil(Fossil fossil, bool leftScreen)
    {
        Fossil newFossil = Instantiate<Fossil>(fossil);
        if (leftScreen)
        {
            LeftText.SetActive(false);
            leftScreenControl.SelectFossil(newFossil);
            newFossil.Setup(leftScreenFossilLocation);
            leftScreenHighlighter.SelectFossil(newFossil);
        }
        else
        {
            RightText.SetActive(false);
            rightScreenControl.SelectFossil(newFossil);
            newFossil.Setup(rightScreenFossilLocation);
            rightScreenHighlighter.SelectFossil(newFossil);
        }
    }
}
