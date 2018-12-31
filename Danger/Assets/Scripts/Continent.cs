using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Extensions;

public class Continent {

    /// <summary>
    /// Name of the Continent
    /// </summary>
    public readonly string Name;
    /// <summary>
    /// Additional armies granted for holding all the Continent's territories at the start of the game
    /// </summary>
    public readonly int StartedAdded;
    public List<Territory> Territories = new List<Territory>();

    public Continent(string name, int addedAtStart, params Territory[] territories)
    {
        Name = name;
        StartedAdded = addedAtStart;
        Territories.AddRange(territories);
    }
    public Continent(string name, int addedAtStart)
    {
        StartedAdded = addedAtStart;
        Name = name;
    }
    public void SetIds()
    {
        int id = 0;
        foreach(var t in Territories)
        {
            id++;
            t.ID = id;
        }
    }
    public Territory GetTerritory(int id)
    {
        return Territories.FirstOrDefault(x => x.ID == id);
    }
    public Territory GetTerritory(string name)
    {
        return Territories.FirstOrDefault(x => x.Name == name);
    }
    public void PairMove(int id1, int id2)
    {
        var t1 = GetTerritory(id1);
        var t2 = GetTerritory(id2);
        t1.WhereCanMove.Add(t2);
        t2.WhereCanMove.Add(t1);
    }
    public void PairMove(string st1, string st2)
    {
        var t1 = GetTerritory(st1);
        var t2 = GetTerritory(st2);
        t1.WhereCanMove.Add(t2);
        t2.WhereCanMove.Add(t1);
    }

    public void AllMove(int id1, params int[] otherIds)
    {
        var t1 = GetTerritory(id1);
        foreach(int id in otherIds)
        {
            var terr = GetTerritory(id);
            t1.WhereCanMove.UniqueAdd(terr);
            terr.WhereCanMove.UniqueAdd(t1);

        }
    }    

    public void PairMove(int id1, Continent continent, int id2)
    {
        var t1 = GetTerritory(id1);
        var t2 = continent.GetTerritory(id2);
        t1.WhereCanMove.UniqueAdd(t2);
        t2.WhereCanMove.UniqueAdd(t1);
    }

    public void PairMove(int id1, Continent otherContinent, params int[] ids)
    {
        var t1 = GetTerritory(id1);
        foreach(int id in ids)
        {
            var t2 = otherContinent.GetTerritory(id);
            t1.WhereCanMove.UniqueAdd(t2);
            t2.WhereCanMove.UniqueAdd(t1);
        }
    }

	public string Display()
    {
        return string.Format("{0}: {1}", Name, string.Join(", ", Territories.Select(x => x.Display()).ToArray()));
    }
}
