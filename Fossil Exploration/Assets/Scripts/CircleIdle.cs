using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleIdle : MonoBehaviour {

    [SerializeField]
    RectTransform outerImage, innerImage;

    [SerializeField]
    float time, interval, range;

    [SerializeField]
    AnimationCurve animationCurve;

    float timeOfLastPulse;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time - timeOfLastPulse > interval)
        {
            StartCoroutine(pulse());
        }
	}

    IEnumerator pulse()
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
