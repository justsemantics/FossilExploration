using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// "pulsing" effect on center circle around FossilIcons and PointOfInterestCircles
/// </summary>
public class CircleIdle : MonoBehaviour {

    [SerializeField]
    private RectTransform outerImage, innerImage;

    [SerializeField]
    [Tooltip("Length in seconds of each pulse")]
    private float time;

    [SerializeField]
    [Tooltip("Time in seconds between pulses")]
    private float interval;

    [SerializeField]
    [Tooltip("Percent increase in max size of circle, i.e. 0.1 = 10% increase")]
    private float range;

    [SerializeField]
    [Tooltip("Curve of size and opacity over the course of the pulse")]
    private AnimationCurve animationCurve;

    private float timeOfLastPulse;

    private Coroutine pulseCoroutine;

	// Use this for initialization
	private void Start () {
		
	}
	
	// Update is called once per frame
	private void Update () {
		if(Time.time - timeOfLastPulse > interval)
        {
            pulseCoroutine = StartCoroutine(pulse());
        }
	}

    /// <summary>
    /// Coroutine that pulses the circle
    /// </summary>
    /// <returns></returns>
    private IEnumerator pulse()
    {
        timeOfLastPulse = Time.time;
        while(Time.time - timeOfLastPulse < time)
        {
            float progress = (Time.time - timeOfLastPulse) / time;
            float t = animationCurve.Evaluate(progress);

            float minSize = outerImage.rect.width;
            float maxSize = minSize + minSize * range;
            float size = Mathf.Lerp(minSize, maxSize, t);

            Color minColor = Color.white;
            Color maxColor = new Color(1, 1, 1, 0);
            Color color = Color.Lerp(minColor, maxColor, t);

            innerImage.sizeDelta = new Vector2(size, size);
            innerImage.GetComponent<Image>().color = color;

            yield return null;
        }
    }
}
