using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InfoText : MonoBehaviour, IPointerDownHandler {

    [SerializeField]        
    RectTransform headerPanelRectTransform, contentPanelRectTransform;

    RectTransform headerTextRectTransform, contentTextRectTransform;

    ContentSizeFitter test;

    [SerializeField]
    Text headerText = null, contentText = null;

    [SerializeField]
    Vector2 headerPadding, contentPadding;

    public PointOfInterest POI;

    public TouchRotate positionController;

    [Range(0, 2)]
    public float OpenAmount = 0;

    Animator animator;

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

    Vector2 position;
    public Vector2 Position { get { return position; }
    set {
            position = value;
            GetComponent<RectTransform>().anchoredPosition = value;
        }
    }

    float headerHeight, contentHeight;

    bool shouldUpdateHeights = true;

    bool open = false;
    public bool Open { get { return open; } set
        {
            open = value;
            animator.SetBool("Open", open);

            if(open == true)
            {
                //close all other InfoTexts
                foreach(InfoText t in transform.parent.gameObject.GetComponentsInChildren<InfoText>())
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
        headerTextRectTransform = headerText.GetComponent<RectTransform>();
        contentTextRectTransform = contentText.GetComponent<RectTransform>();
        animator = GetComponent<Animator>();
        shouldUpdateHeights = true;

        //InfoTexts should appear on top of other UI elements
        transform.SetAsLastSibling();
	}


    public void setTextFields(string _headerText, string _contentText)
    {
        HeaderTextString = _headerText;
        ContentTextString = _contentText;
    }
	
	// Update is called once per frame
	void Update () {
        UpdateHeights();
        ResizePanels();
	}

    void ResizePanels()
    {
        headerTextRectTransform.anchoredPosition = headerPadding;
        contentTextRectTransform.anchoredPosition = contentPadding;

        headerPanelRectTransform.sizeDelta = new Vector2(
            (headerTextRectTransform.sizeDelta.x + headerPadding.x * 2) * Mathf.Clamp01(OpenAmount),
            headerHeight);

        contentPanelRectTransform.anchoredPosition = new Vector2(0, -headerHeight);
        contentPanelRectTransform.sizeDelta = new Vector2(
            contentTextRectTransform.sizeDelta.x + contentPadding.x * 2,
            contentHeight * Mathf.Clamp01(OpenAmount - 1));
    }

    void UpdateHeights()
    {
        if (shouldUpdateHeights)
        {
            headerHeight = headerTextRectTransform.sizeDelta.y + headerPadding.y * 2;
            contentHeight = contentTextRectTransform.sizeDelta.y + contentPadding.y * 2;
            //shouldUpdateHeights = false;
        }
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
