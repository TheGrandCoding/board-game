using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using Extensions;

public abstract class ImageButton : MonoBehaviour, IPointerClickHandler
{
    public UnityEngine.UI.Text text;
    public UnityEngine.UI.Text text2;
    public UnityEngine.UI.Text text3;
    public Canvas canvas;
    private RectTransform canvasRect;
    public Texture2D Image;
	void Start ()
    {
        Startup();
        canvasRect = canvas.GetComponent<RectTransform>();
	}

    private Vector3 MoveFromCentreToLocal(Vector3 relativeToCentre)
    {
        // Pointx - Centrex = Actualx
        int x = (int)relativeToCentre.x;
        int y = (int)relativeToCentre.y;

        // we need to deduct from the position of the transform
        Debug.Log(transform.localPosition.ToString());
        x -= (int)transform.localPosition.x;
        y -= (int)transform.localPosition.y;


        return new Vector3(x, y);
    }

    private Vector2Int GetPixelPoint(PointerEventData dat)
    {
        Vector3 localPos = transform.InverseTransformPoint(dat.pressPosition);
        Debug.Log("Centre: " + localPos.ToString());
        localPos = MoveFromCentreToLocal(localPos);
        var clr = Image.GetPixel((int)localPos.x, (int)localPos.y);
        Debug.Log(clr);
        text3.color = clr;

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
        text2.text = pixelPos.ToString();

        Clicked();
    }

   
    void Update()
    {
        Vector3 localPos = Input.mousePosition;
        text.text = canvas.ScreenToCanvasPosition(localPos).ToString();
    }


    public abstract void Clicked();
    public abstract void Startup();
}
