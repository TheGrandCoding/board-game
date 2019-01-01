using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
            return Continent.GetTerritory(InternalID);
        } }

    public override void Clicked()
    {
        Debug.Log("Clicked on " + Continent.Name + " " + Territory.Name);
    }

    public override void Startup()
    {
    }
}
