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
    Fossil secondFossilSelection;

    [SerializeField]
    HighlightPointsOfInterest highlighter;

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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            leftScreenControl.SelectFossil(secondFossilSelection);
            initialFossilSelection.Store();
            secondFossilSelection.Setup(fossilLocation);
            highlighter.SelectFossil(secondFossilSelection);
        }
	}
}
