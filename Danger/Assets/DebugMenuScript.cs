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

    struct MoveStructure
    {
        public Territory From;
        public Army Army;
    }

    /// <summary>
    /// Moves an army from one territory to another
    /// </summary>
    public void DebugMoveArmy()
    {
        UIHelper.RunOnTerritoryConfirmed(new TerritoryDisplayCriteria(ownedBy: GameManager.StartPlayer), (Territory from, TerritoryDisplayCriteria crit, object state) =>
        {
            Debug.Log("Moving armies from " + from.Name);
            if (from.DefendingArmies == null || from.DefendingArmies.Count == 0)
            from.DefendingArmies = new List<Army>() { new Army(ArmyType.Infantry) };
            var army = from.DefendingArmies[0];

            UIHelper.RunOnTerritoryConfirmed(new TerritoryDisplayCriteria(ownedBy: GameManager.StartPlayer), (Territory destination, TerritoryDisplayCriteria criteria, object oState) =>
            {
                Debug.Log("Moving to " + destination.Name);
                var moving = (MoveStructure)oState;
                Territory.AttemptOrFailToMove(GameManager.StartPlayer, moving.From, destination);
            }, new MoveStructure() { From = from, Army = army });
        }, null);
        /*
        var from = GameManager.GetTerritory("Great Britain");
        var to = GameManager.GetTerritory("Ukraine");
        var path = new List<Territory>()
            {
                from, GameManager.GetTerritory("Scandinavia"), to
            };

        var couldMove = Territory.CouldMoveArmyThroughPath(new Army(ArmyType.Infantry) { Owner = GameManager.StartPlayer }, from, to, path
            );
        if (couldMove)
            Debug.Log($"Able to move from {from} to {to} via: {string.Join(" -> ", path)}");*/
    }
}
