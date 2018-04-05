using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FossilSelection : MonoBehaviour {

    [SerializeField]
    TouchRotate leftScreenControl;

    [SerializeField]
    Transform fossilLocation;

    [SerializeField]
    Fossil initialFossilSelection;

    [SerializeField]
    HighlightPointsOfInterest leftScreenHighlighter, rightScreenHighlighter;

	// Use this for initialization
	void Start () {
        leftScreenControl.SelectFossil(initialFossilSelection);

        foreach(Fossil f in FindObjectsOfType<Fossil>())
        {
            f.Store();
        }

        initialFossilSelection.Setup(fossilLocation);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
