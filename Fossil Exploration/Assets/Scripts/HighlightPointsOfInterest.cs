using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class HighlightPointsOfInterest : MonoBehaviour {

    [SerializeField]
    PointOfInterestCircle pointOfInterestCirclePrefab;

    [SerializeField]
    InfoText infoTextPrefab;

    Fossil currentFossil;

    [SerializeField]
    float sensitivity = 0.01f;

    [SerializeField]
    RectTransform circlePanel, infoTextPanel;

    [SerializeField]
    TouchRotate touchRotate;

    [SerializeField]
    Camera cam;

    List<InfoText> currentInfoTexts = new List<InfoText>();
    List<PointOfInterestCircle> currentPOICircles = new List<PointOfInterestCircle>();

    bool shouldRefresh = false;

	// Use this for initialization
	void Start () {
	}

    /// <summary>
    /// Makes sure that Refresh is only called after the new Fossil is done being set up
    /// </summary>
    void LateUpdate()
    {
        if (shouldRefresh)
        {
            Refresh();
        }
    }

    /// <summary>
    /// Don't call this directly. Instead, set the shouldRefresh flag and let LateUpdate handle it.
    /// Deletes old InfoTexts and PointOfInterestCircles and makes new ones based on the currentFossil.
    /// </summary>
    void Refresh()
    {
        //Delete old stuff, clear out lists
        foreach (InfoText i in currentInfoTexts)
        {
            Destroy(i.gameObject);
        }
        currentInfoTexts.Clear();

        foreach (PointOfInterestCircle c in currentPOICircles)
        {
            Destroy(c.gameObject);
        }
        currentPOICircles.Clear();

        //Make new stuff
        foreach (PointOfInterest POI in currentFossil.PointsOfInterest)
        {
            InfoText i = Instantiate<InfoText>(infoTextPrefab, infoTextPanel, false);

            i.HeaderTextString = POI.Header;
            i.ContentTextString = POI.Content;
            i.POI = POI;
            i.positionController = touchRotate;
            i.Position = POI.InfoPanelLocation;

            currentInfoTexts.Add(i);

            PointOfInterestCircle c = Instantiate<PointOfInterestCircle>(pointOfInterestCirclePrefab, circlePanel);

            c.POI = POI;
            c.cam = cam;
            c.Info = i;
            c.sensitivity = sensitivity;

            currentPOICircles.Add(c);
        }

        shouldRefresh = false;
    }

    public void SelectFossil(Fossil fossil)
    {
        currentFossil = fossil;
        shouldRefresh = true;      
    }
	
	// Update is called once per frame
	void Update () {
	}
}
