using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerCard  {

    public Territory Territory; // despite being present, this does not mean the Card 'owns' the territory - its purely cosmetic
    public ArmyType ArmyType;
    public bool IsWild { get
        {
            return (Territory == null && ArmyType == ArmyType.NotSet);
        } }

    public DangerCard(Territory associatedTerritory, ArmyType army)
    {
        Territory = associatedTerritory;
        ArmyType = army;
    }
    public DangerCard()
    {
    }

}
