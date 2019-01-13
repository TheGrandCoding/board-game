﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Territory {

    public readonly string Name;
    public readonly Continent Continent;
    public int ID;
    public List<Territory> WhereCanMove = new List<Territory>();
    public Image Image;
    public Color ModColor {  get
        {
            return Owner.PlayerColor;
        } }

    public Territory(string name, Continent continent)
    {
        Name = name;
        Continent = continent;
    }

    public void SetOwner(Player newOwner)
    {
        try
        {
            newOwner = newOwner ?? GameManager.NotOwned;
            Owner = newOwner;
            if (Image == null)
                Debug.LogWarning(this.ToString() + ": Image null");
            if (ModColor == null)
                Debug.LogWarning(this.ToString() + ": ModColor null");
            Image.color = ModColor;
        } catch (Exception ex)
        {
            Debug.LogError("Errored in " + this.ToString() + ", ex: " + ex.ToString());
        }
    }

    public Player Owner { get; private set; } = GameManager.NotOwned; // default is Neutral until it is later claimed

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

    /// <summary>
    /// CN = Continent Name
    /// NM = Name
    /// ID = ID
    /// OWN = Owner Name
    /// </summary>
    public string ToString(string format)
    {
        format = format.Replace("CN", "{0}");
        format = format.Replace("NM", "{1}");
        format = format.Replace("ID", "{2}");
        format = format.Replace("OWN", "{3}");
        return string.Format(format, this.Continent.Name, this.Name, this.ID, this.Owner == null ? "" : this.Owner.Name);
    }
    public override string ToString()
    {
        return this.ToString("CN NM");
    }
}
