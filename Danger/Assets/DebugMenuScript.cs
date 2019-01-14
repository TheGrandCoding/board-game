using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMenuScript : MonoBehaviour
{
    /// <summary>
    /// Confirmed to click and control any territory on the map
    /// </summary>
    public void ClickToControl()
    {
        UIHelper.RunOnTerritoryConfirmed(new TerritoryDisplayCriteria(), (Territory t, TerritoryDisplayCriteria crit, object state) =>
        {
            Player owner = (Player)state;
            Debug.Log($"Setting owner of {t.Name} to {owner.Name}");
            t.Owner = owner;
        }, GameManager.StartPlayer);
    }

    public void ClickToControlAllContinent()
    {
        UIHelper.RunOnTerritoryConfirmed(new TerritoryDisplayCriteria(), (Territory t, TerritoryDisplayCriteria crit, object state) =>
        {
            Debug.Log($"Setting {(Player)state} to control all of {t.Continent.Name}");
            foreach(var terr in t.Continent.Territories)
            {
                terr.Owner = (Player)state;
            }
        }, GameManager.StartPlayer);
    }

    /// <summary>
    /// Gives new armies to players as though a new turn has just occured
    /// </summary>
    public void DebugStartTurn()
    {
        foreach(var p in GameManager.Players)
        {
            p.IncreaseArmies();
        }
        Debug.Log(GameManager.StartPlayer.ArmiesToGive.Count);
    }
}
