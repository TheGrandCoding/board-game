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

	public int RollAttack()
    {
        return RandomGens.Dice.RollSixSided();
    }

    public int RollDefence()
    {
        return RandomGens.Dice.RollSixSided();
    }
}
