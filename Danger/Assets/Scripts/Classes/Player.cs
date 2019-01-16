using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player {

    public readonly string Name;
    public Player(string name)
    {
        Name = name;
    }
    public List<DangerCard> OwnedCards;
    public List<Territory> Territories { get
        {
            var terr = new List<Territory>();
            foreach(var item in OwnedCards)
            {
                if (item.Territory != null)
                    terr.Add(item.Territory);
            }
            return terr;
        } }
    public Territory CapitalCity;
    public int ChoiceIndex;
    public Color PlayerColor {  get
        {
            switch (ChoiceIndex) {
                case 0:
                    return Color.blue;
                case 1:
                    return Color.red;
                case 2:
                    return Color.yellow;
                case 3:
                    return Color.cyan;
                case 4:
                    return Color.magenta;
                case 5:
                    return Color.green;
                default:
                    return Color.grey;
            }
        } }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
