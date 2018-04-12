﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputStateManager : MonoBehaviour
{
    public delegate void TouchAddedHandler(int fingerId, touchType type);

    public delegate void TouchRemovedHandler(int fingerId);

    public event TouchAddedHandler OnTouchAdded;
    public event TouchRemovedHandler OnTouchRemoved;

    public Dictionary<int, FossilTouch> Touches = new Dictionary<int, FossilTouch>();

    [SerializeField]
    Canvas UICanvas;

    [SerializeField]
    RectTransform leftScreenArea, rightScreenArea, selectionScreenArea;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (Touch t in Input.touches)
        {
            HandleTouch(t);
        }
    }

    private void HandleTouch(Touch t)
    {
        //if this is a new touch
        if (!Touches.ContainsKey(t.fingerId))
        {
            touchType type = DetermineTouchType(t);
            Touches.Add(t.fingerId, new FossilTouch(t, type));

            if (OnTouchAdded != null)
                OnTouchAdded.Invoke(t.fingerId, type);
        }
        else
        {
            Touches[t.fingerId].touch = t;
        }

        //if the touch is finished this frame but we are tracking it
        if ((t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled) && Touches.ContainsKey(t.fingerId))
        {
            Touches.Remove(t.fingerId);

            if (OnTouchRemoved != null)
                OnTouchRemoved.Invoke(t.fingerId);

        }
    }

    private touchType DetermineTouchType(Touch t)
    {
        if(t.position.x < Screen.width * 0.4)
        {
            return touchType.left;
        }
        else if(t.position.x > Screen.width * 0.6)
        {
            return touchType.right;
        }
        else
        {
            return touchType.selection;
        }
    }
}


public class FossilTouch
{
    public Touch touch;
    public touchType type;

    public FossilTouch(Touch _touch, touchType _type)
    {
        touch = _touch;
        type = _type;
    }
}

public enum touchType
{
    left,
    selection,
    right
}