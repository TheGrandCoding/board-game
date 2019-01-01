using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public abstract class ImageButton : MonoBehaviour, IPointerClickHandler
{
	void Start () {
        Startup();
	}

    private Vector2Int GetPixelPoint(PointerEventData dat)
    {
        Vector3 localPos = transform.InverseTransformPoint(dat.pressPosition);

        // i'm not sure what's going wrong here
        // but clicking in the bottom left (where [0,0] should be) gives something like [402, 196]
        // which im like wtf??
        // so i'm not sure if you need to add/remove the size of the image or what
        // or wtf is going on
        // but i cant figure it out as of currently
        // so im gonna commit this and leave

        // i think the above also refers to the position as per the Canvas itself
        // so maybe you need to deduct something from that? idk

        return new Vector2Int((int)localPos.x, (int)localPos.y);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // TODO: get pixel at clicked place, ensure its alpha is set?
        // so that we only trigger if they clicked on somewhere that isnt a background
        // ie: only trigger if they actually click the image
        var pixelPos = GetPixelPoint(eventData);
        Debug.Log(pixelPos);

        Clicked();
    }


    public abstract void Clicked();
    public abstract void Startup();
}
