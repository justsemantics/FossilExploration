using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FossilIcon : MonoBehaviour {

    [SerializeField]
    public Fossil fossil;

    RectTransform rectTransform;

    Vector2 initialPosition;

    // Use this for initialization
    void Start () {
        rectTransform = GetComponent<RectTransform>();
        initialPosition = rectTransform.anchoredPosition;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetPosition(Vector2 position)
    {
        Vector2 halfScreen = new Vector2(Screen.width / 2, Screen.height / 2);
        rectTransform.anchoredPosition = position - halfScreen;
    }

    public Vector2 GetPosition()
    {
        Vector2 halfScreen = new Vector2(Screen.width / 2, Screen.height / 2);
        return rectTransform.anchoredPosition + halfScreen;
    }

    public void Return()
    {
        rectTransform.anchoredPosition = initialPosition;
    }
}
