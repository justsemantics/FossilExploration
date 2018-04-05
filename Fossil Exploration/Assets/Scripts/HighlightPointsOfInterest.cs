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

    [SerializeField]
    Fossil currentFossil;

    [SerializeField]
    float sensitivity;

    [SerializeField]
    RectTransform circlePanel, infoTextPanel;

    [SerializeField]
    TouchRotate touchRotate;

    [SerializeField]
    Camera cam;

    bool testing = true;

    List<InfoText> currentInfoTexts = new List<InfoText>();
    List<PointOfInterestCircle> currentPOICircles = new List<PointOfInterestCircle>();

	// Use this for initialization
	void Start () {
       
	}

    public void SelectFossil(Fossil fossil)
    {
        currentFossil = fossil;
        if(currentFossil.PointsOfInterest.Length > 0)
        {
            testing = false;
        }

        foreach(InfoText i in currentInfoTexts)
        {
            Destroy(i.gameObject);
        }
        currentInfoTexts.Clear();

        foreach(PointOfInterestCircle c in currentPOICircles)
        {
            Destroy(c.gameObject);
        }
        currentPOICircles.Clear();

        foreach (PointOfInterest POI in currentFossil.PointsOfInterest)
        {
            InfoText i = Instantiate<InfoText>(infoTextPrefab, infoTextPanel, false);

            i.HeaderTextString = POI.Header;
            i.ContentTextString = POI.Content;
            i.POI = POI;
            i.positionController = touchRotate;

            currentInfoTexts.Add(i);

            PointOfInterestCircle c = Instantiate<PointOfInterestCircle>(pointOfInterestCirclePrefab, circlePanel);

            c.POI = POI;
            c.cam = cam;
            c.Info = i;

            currentPOICircles.Add(c);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (testing)
        {
            SelectFossil(currentFossil);
        }
	}
}
