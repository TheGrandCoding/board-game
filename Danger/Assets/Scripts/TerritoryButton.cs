﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

[RequireComponent(typeof(Image))] 
public class TerritoryButton : ImageButton {

    public string Name;
    public string ContinentName;
    public int InternalID;

    public Continent Continent {  get
        {
            return GameManager.Continents.FirstOrDefault(x => x.Name == ContinentName);
        } }
    public Territory Territory {  get
        {
            return Continent.Territories.FirstOrDefault(x => x.Name == Name || x.ID == InternalID);
        } }

    public override void Clicked()
    {
        Debug.Log("Clicked on " + Territory.ToString("CN NM (ID) OWN"));
        UIPopupScript.DisplayIfSatisfy(Territory);
    }

    public override void Startup()
    {
        if (Continent == null)
        {
            Debug.LogError("No Continent for gameobject " + this.name); // small 'n' for name is intentional
        }
        if (Territory == null)
        {
            Debug.LogError("No territory for gameobject " + this.name);
        } else
        {
            Territory.Image = this.GetComponent<Image>();
            if (Territory.Image == null)
                Debug.LogError("TerritoryButton must have an Image attached to same gameobject");
        }
    }
}
