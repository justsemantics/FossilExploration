using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InfoText : MonoBehaviour, IPointerDownHandler {


    [SerializeField]
    RectTransform panelRectTransform; 
        
    RectTransform headerTextRectTransform, contentTextRectTransform;

    [SerializeField]
    float smoothTime = 0.2f;

    LayoutElement panelLayoutElement;
    [SerializeField]
    Text headerText = null, contentText = null;

    public PointOfInterest POI;

    public TouchRotate positionController;

    public string HeaderTextString { get { return headerText.text; }
        set
        {
            shouldUpdateHeights = true;
            headerText.text = value;
        }
    }

    public string ContentTextString { get { return headerText.text; }
    set
        {
            shouldUpdateHeights = true;
            contentText.text = value;
        }
    }

    float closedHeight, openHeight;

    bool shouldUpdateHeights = true;

    bool open = false;
    public bool Open { get { return open; } set
        {
            open = value;
            resizeVelocity = 0;

            if(open == true)
            {
                //close all other InfoTexts
                foreach(InfoText t in FindObjectsOfType<InfoText>())
                {
                    if( t != this)
                    {
                        t.Open = false;
                    }
                }

                positionController.RotateToPOI(POI);
            }

            if(open == false)
            {

            }
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("touch");
        Open = !Open;
    }

    // Use this for initialization
    void Start () {
        panelLayoutElement = panelRectTransform.GetComponent<LayoutElement>();

        headerTextRectTransform = headerText.GetComponent<RectTransform>();
        contentTextRectTransform = contentText.GetComponent<RectTransform>();
	}

    public void setTextFields(string _headerText, string _contentText)
    {
        HeaderTextString = _headerText;
        ContentTextString = _contentText;
    }
	
	// Update is called once per frame
	void Update () {
        UpdateHeights();
        ResizePanel();
	}

    void UpdateHeights()
    {
        if (shouldUpdateHeights)
        {
            closedHeight = headerTextRectTransform.sizeDelta.y;
            openHeight = contentTextRectTransform.sizeDelta.y + closedHeight;

            shouldUpdateHeights = false;
        }
    }

    float resizeVelocity;
    void ResizePanel()
    {
        panelLayoutElement.minHeight = Mathf.SmoothDamp(panelLayoutElement.minHeight,
            Open ? openHeight : closedHeight,
            ref resizeVelocity,
            smoothTime);
    }

    private void OnRectTransformDimensionsChange()
    {
        shouldUpdateHeights = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Open = !Open;
    }
}
