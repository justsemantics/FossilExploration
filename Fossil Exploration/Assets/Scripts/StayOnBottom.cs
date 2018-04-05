using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayOnBottom : MonoBehaviour {

    [SerializeField]
    Transform lastTransform;

    bool shouldMoveTransform = false;

    int numChildren = 0;

    private void OnTransformChildrenChanged()
    {
        if (numChildren != transform.childCount)
        {
            numChildren = transform.childCount;
            shouldMoveTransform = true;
        }
    }

    private void Update()
    {
        if (shouldMoveTransform)
        {
            lastTransform.SetAsLastSibling();
            shouldMoveTransform = false;
        }
        
    }
}
