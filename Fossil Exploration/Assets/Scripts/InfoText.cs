using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Controller for textbox that appears when a user clicks on a PointOfInterest
/// </summary>
public class InfoText : MonoBehaviour, IPointerDownHandler {

    [SerializeField]
    [Tooltip("Container panels for the header and content")]        
    private RectTransform headerPanelRectTransform, contentPanelRectTransform;

    /// <summary>
    /// RectTransform of the Text in the header panel
    /// </summary>
    private RectTransform headerTextRectTransform;
    /// <summary>
    /// RectTransform of the Text in the content panel
    /// </summary>
    private RectTransform contentTextRectTransform;

    [SerializeField]
    [Tooltip("Header and Content Text components")]
    private Text headerText = null, contentText = null;

    [SerializeField]
    [Tooltip("Amount of space in pixels to leave between text and edge of panel.")]
    private Vector2 headerPadding, contentPadding;

    /// <summary>
    /// Connected PointOfInterest, used to determine position and text content
    /// </summary>
    [HideInInspector]
    public PointOfInterest POI;

    //TODO: this feels weird, i'm sure there's a more OOP way to do this without tangling everything into each other
    /// <summary>
    /// Connected TouchRotate, used to rotate the Fossil to showcase selected Points of Interest
    /// </summary>
    [HideInInspector]
    public TouchRotate positionController;

    /// <summary>
    /// Values between 0-1 open the Header horizontally, values between 1-2 drop open the Content vertically
    /// The animator adjusts this value smoothly to create an "animation" with adjustable parameters
    /// </summary>
    [Range(0, 2)]
    public float OpenAmount = 0;

    private Animator animator;


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

    private Vector2 position;
    public Vector2 Position { get { return position; }
    set {
            position = value;
            GetComponent<RectTransform>().anchoredPosition = value;
        }
    }

    private float headerHeight, contentHeight;

    /// <summary>
    /// This flag gets set any time we need to recalculate the height of the text
    /// </summary>
    private bool shouldUpdateHeights = true;

    private bool open = false;
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
        }
    }

    // Use this for initialization
    private void Start () {
        headerTextRectTransform = headerText.GetComponent<RectTransform>();
        contentTextRectTransform = contentText.GetComponent<RectTransform>();
        animator = GetComponent<Animator>();
        shouldUpdateHeights = true;

        //InfoTexts should appear on top of other UI elements
        transform.SetAsLastSibling();
	}

	// Update is called once per frame
	private void Update () {
        UpdateHeights();
        ResizePanels();
	}
	
    /// <summary>
    /// Lerp panel sizes based on "OpenAmount" (0 = closed, 1 = header only, 2 = all content)
    /// </summary>
    void ResizePanels()
    {
        headerTextRectTransform.anchoredPosition = headerPadding;
        contentTextRectTransform.anchoredPosition = contentPadding;

        //Header opens horizontally as OpenAmount goes from 0 to 1
        headerPanelRectTransform.sizeDelta = new Vector2(
            (headerTextRectTransform.sizeDelta.x + headerPadding.x * 2) * Mathf.Clamp01(OpenAmount),
            headerHeight);

        //Content opens vertically as OpenAmount goes from 1 to 2
        contentPanelRectTransform.anchoredPosition = new Vector2(0, -headerHeight);
        contentPanelRectTransform.sizeDelta = new Vector2(
            contentTextRectTransform.sizeDelta.x + contentPadding.x * 2,
            contentHeight * Mathf.Clamp01(OpenAmount - 1));
    }

    /// <summary>
    /// Recalculate headerHeight and contentHeight so that the panels fit the text they contain
    /// </summary>
    void UpdateHeights()
    {
        if (shouldUpdateHeights)
        {
            headerHeight = headerTextRectTransform.sizeDelta.y + headerPadding.y * 2;
            contentHeight = contentTextRectTransform.sizeDelta.y + contentPadding.y * 2;
            shouldUpdateHeights = false;
        }
    }

    /// <summary>
    /// Catch aspect ratio changes just in case
    /// </summary>
    private void OnRectTransformDimensionsChange()
    {
        shouldUpdateHeights = true;
    }

    /// <summary>
    /// Close if the user clicks on the window
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        Open = !Open;
    }
}
