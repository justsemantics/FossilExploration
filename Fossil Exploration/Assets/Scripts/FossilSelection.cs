using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class FossilSelection : MonoBehaviour {

    [SerializeField]
    TouchRotate leftScreenControl, rightScreenControl;

    [SerializeField]
    Transform leftScreenFossilLocation, rightScreenFossilLocation;

    [SerializeField]
    Fossil leftScreenInitialFossilSelection, rightScreenInitialFossilSelection;

    [SerializeField]
    HighlightPointsOfInterest leftScreenHighlighter, rightScreenHighlighter;

    [SerializeField]
    InputStateManager touchManager;

    [SerializeField]
    EventSystem eventSystem;

    [SerializeField]
    GraphicRaycaster raycaster;

    Dictionary<int, FossilIcon> activeIcons = new Dictionary<int, FossilIcon>();

	// Use this for initialization
	void Start () {
        //leftScreenControl.SelectFossil(leftScreenInitialFossilSelection);
        //rightScreenControl.SelectFossil(rightScreenInitialFossilSelection);

        //leftScreenInitialFossilSelection.Setup(leftScreenFossilLocation);
        //rightScreenInitialFossilSelection.Setup(rightScreenFossilLocation);

        touchManager.OnTouchAdded += TouchAdded;
        touchManager.OnTouchRemoved += TouchRemoved;
	}
	
	// Update is called once per frame
	void Update () {
        foreach(KeyValuePair<int, FossilIcon> kvp in activeIcons)
        {
            kvp.Value.SetPosition(touchManager.Touches[kvp.Key].touch.position);
        }
	}

    void TouchAdded(int fingerId, touchType type)
    {
        if(type == touchType.selection)
        {
            PointerEventData eventData = new PointerEventData(eventSystem);
            eventData.position = touchManager.Touches[fingerId].touch.position;
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(eventData, results);

            foreach(RaycastResult result in results)
            {
                FossilIcon icon = result.gameObject.GetComponent<FossilIcon>();

                if(icon != null)
                {
                    activeIcons.Add(fingerId, icon);
                    icon.Pickup();
                }
            }
        }
    }

    void TouchRemoved(int fingerId)
    {
        if (activeIcons.ContainsKey(fingerId))
        {
            if(activeIcons[fingerId].GetPosition().x < Screen.width * 0.4)
            {
                selectFossil(activeIcons[fingerId].fossil, true);
            }
            else if(activeIcons[fingerId].GetPosition().x > Screen.width * 0.6)
            {
                selectFossil(activeIcons[fingerId].fossil, false);
            }

            activeIcons[fingerId].Return();

            activeIcons.Remove(fingerId);
        }
    }

    private void selectFossil(Fossil fossil, bool leftScreen)
    {
        Fossil newFossil = Instantiate<Fossil>(fossil);
        if (leftScreen)
        {
            leftScreenControl.SelectFossil(newFossil);
            newFossil.Setup(leftScreenFossilLocation);
            leftScreenHighlighter.SelectFossil(newFossil);
        }
        else
        {
            rightScreenControl.SelectFossil(newFossil);
            newFossil.Setup(rightScreenFossilLocation);
            rightScreenHighlighter.SelectFossil(newFossil);
        }
    }
}
