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

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
