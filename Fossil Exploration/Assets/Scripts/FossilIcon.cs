using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FossilIcon : MonoBehaviour {

    [SerializeField]
    public Fossil fossil;

    RectTransform rectTransform;

    Vector2 initialPosition;

    FossilIcon spareIcon;

    Image fossilImage;

    // Use this for initialization
    void Start () {
        fossilImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        initialPosition = rectTransform.anchoredPosition;
	}

    public void Init()
    {
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

    public void Return(bool lerp = false)
    {
        rectTransform.anchoredPosition = initialPosition;
        Destroy(spareIcon.gameObject);
    }

    public void Pickup()
    {
        if(spareIcon == null)
        {
            spareIcon = Instantiate(this);

            spareIcon.FadeIn();
        }
    }

    public void FadeIn()
    {
        StartCoroutine(fadeInAfterSeconds(1, 1));
    }

    private void OnDestroy()
    {
        Destroy(spareIcon.gameObject);
    }

    IEnumerator fadeInAfterSeconds(float time, float fadeTime)
    {
        fossilImage.color = Color.clear;
        float startTime = Time.time;
        while(Time.time - startTime < time)
        {
            yield return null;
        }
        fossilImage.color = Color.white;
    }
}
