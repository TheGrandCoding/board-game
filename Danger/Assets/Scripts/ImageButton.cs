using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class ImageButton : DangerGameObject, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	
    public void OnPointerClick(PointerEventData eventData)
    {
        // TODO: get pixel at clicked place, ensure its alpha is set?
        // so that we only trigger if they clicked on somewhere that isnt a background
        // ie: only trigger if they actually click the image
        Clicked();
    }

    public abstract void Clicked();

    public abstract void MouseOver();

    public abstract void MouseExit();

    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseOver();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseExit();
    }
}
