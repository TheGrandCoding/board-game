using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class ImageButton : MonoBehaviour, IPointerClickHandler
{
	void Start () {
        GameManager.Ready += GameReady;
	}

    void GameReady(object sender, GameManager.ReadyEventArgs e)
    {
        GameManager.Ready -= GameReady;
        Startup();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // TODO: get pixel at clicked place, ensure its alpha is set?
        // so that we only trigger if they clicked on somewhere that isnt a background
        // ie: only trigger if they actually click the image
        Clicked();
    }


    public abstract void Clicked();
    /// <summary>
    /// Only triggered when the GameManager has finished
    /// </summary>
    public abstract void Startup();
}
