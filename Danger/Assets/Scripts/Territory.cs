using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Territory {

    public readonly string Name;
    public readonly Continent Continent;
    public int ID;
    public List<Territory> WhereCanMove = new List<Territory>();

    public Territory(string name, Continent continent)
    {
        Name = name;
        Continent = continent;
    }

    public Player Owner = GameManager.NotOwned; // default is Neutral until it is later claimed

    public List<Army> DefendingArmies = new List<Army>();
    public List<Army> AttackingArmies = new List<Army>();

    public string Display(bool displayWhereCanMove = false)
    {
        string where = "";
        if(displayWhereCanMove)
        {
            where = " / Moves: " + string.Join(", ", WhereCanMove.Select(x => x.Continent.Name + " " + x.Name).ToArray());
        }
        return string.Format("[{0}] {1} - {2} vs {3} | {4}{5}", ID, Name, DefendingArmies.Count, AttackingArmies.Count, Owner.Name, displayWhereCanMove ? where : "");
    }
}
