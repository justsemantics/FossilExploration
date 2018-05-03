using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A draggable image of a fossil.
/// When users drag a FossilIcon onto their side of the screen, that fossil appears so
/// they can examine it
/// </summary>
public class FossilIcon : MonoBehaviour {

    [SerializeField]
    [Tooltip("The Fossil prefab to spawn when this FossilIcon is selected")]
    public Fossil fossil;
    
    /// <summary>
    /// RectTransform of this GameObject
    /// </summary>
    private RectTransform rectTransform;

    /// <summary>
    /// Position to return to when users "let go" of the FossilIcon
    /// </summary>
    private Vector2 initialPosition;

    // Use this for initialization
    private void Start () {
        rectTransform = GetComponent<RectTransform>();
        initialPosition = rectTransform.anchoredPosition;
	}

    public void Init()
    {
        initialPosition = rectTransform.anchoredPosition;
    }

    // Update is called once per frame
    private void Update () {
		
	}

    /// <summary>
    /// Takes a pixel position and sets the anchored position properly
    /// This is an artifact of when all the FossilIcons were placed procedurally around a central anchor
    /// </summary>
    /// <param name="position">Pixel position</param>
    public void SetPosition(Vector2 position)
    {
        Vector2 halfScreen = new Vector2(Screen.width / 2, Screen.height / 2);
        rectTransform.anchoredPosition = position - halfScreen;
    }

    /// <summary>
    /// Returns an accurate pixel position for the center of the FossilIcon
    /// </summary>
    /// <returns>Pixel position</returns>
    public Vector2 GetPosition()
    {
        Vector2 halfScreen = new Vector2(Screen.width / 2, Screen.height / 2);
        return rectTransform.anchoredPosition + halfScreen;
    }

    /// <summary>
    /// Called when a touch releases this FossilIcon
    /// </summary>
    public void Return()
    {
        rectTransform.anchoredPosition = initialPosition;
    }

    /// <summary>
    /// Called when a touch begins to drag this FossilIcon
    /// </summary>
    public void Pickup()
    {
        //probably useful in the future, but this does nothing now
    }
}
