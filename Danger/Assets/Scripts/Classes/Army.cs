using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Army {

    public Player Owner;
    public ArmyType Type;
    public Territory Location;

    public Army(ArmyType type)
    {
        Type = type;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
