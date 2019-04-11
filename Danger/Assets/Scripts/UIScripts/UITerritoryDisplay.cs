﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class UITerritoryDisplay : MonoBehaviour
{
    private static UITerritoryDisplay instance; // for static usage

    public TextMeshProUGUI Name;
    public TextMeshProUGUI Owner;
    public TextMeshProUGUI Infantry;
    public TextMeshProUGUI Cavalery;
    public TextMeshProUGUI Artillery;

    public Button PlaceButton;
    public Button MoveOrAttackBtn;
    public TextMeshProUGUI MoveAttackText;

    public Territory Territory;

    private RectTransform Transform;
    private Vector3 OriginalPosition;

    // Start is called before the first frame update
    void Start()
    {
        Transform = GetComponent<RectTransform>();
        OriginalPosition = Transform.localPosition;
        display(null);
    }

    public void CloseButtonClicked()
    {
        display(null, true);
    }

    public void MoveButtonClicked()
    {
        UIHelper.SelecyArmyAmount(new TerritoryDisplayCriteria(ownedBy: Territory.Owner), (Territory from, TerritoryDisplayCriteria crit, object callback) =>
         {
             // TODO: implement
             SelectArmyPopup.ArmySelectionDetails details = (SelectArmyPopup.ArmySelectionDetails)callback;
             Debug.Log($"{details.InfAmount} & {details.CavAmount} & {details.ArtAmount} moving from {this.Territory.Name} ({from.Name})");

             // now we need to select where to move them.
             UIHelper.SelectTerritory(new TerritoryDisplayCriteria(mustBeMoveableFrom:from, allowInvade:GameManager.CurrentStage == GameStage.Attack), (Territory terr, TerritoryDisplayCriteria criter, object obj) =>
             {
                 // move things.
                 Debug.Log($"Moving to " + terr.Name);
                 List<Army> armiesToMove = new List<Army>();
                 for(int i = 0; i < details.InfAmount; i++)
                 {
                     var army = from.RemoveArmy(ArmyType.Infantry);
                     armiesToMove.Add(army);
                 }
                 for (int i = 0; i < details.CavAmount; i++)
                 {
                     var army = from.RemoveArmy(ArmyType.Cavalry);
                     armiesToMove.Add(army);
                 }
                 for (int i = 0; i < details.ArtAmount; i++)
                 {
                     var army = from.RemoveArmy(ArmyType.Artillery);
                     armiesToMove.Add(army);
                 }
                 Debug.Log($"Owners: {from.Owner} vs {terr.Owner}");
                 if(terr.Owner != from.Owner)
                 { // invading this
                     Debug.Log("Invadion has begun");
                     armiesToMove = terr.GetAttacked(armiesToMove);
                     Debug.Log("Invasion concluded...");
                     if(terr.TotalDefendingArmies == 0 && armiesToMove.Count > 0)
                     {
                         // successful invasion.
                         terr.SetOwner(from.Owner);
                         Debug.Log("Invasion was a success");
                     } else
                     {
                         Debug.Log("Invasion failled.");
                     }
                 }
                 foreach(var army in armiesToMove)
                 {
                     terr.AddArmy(army);
                 }
             }, details);
         }, null);
    }

    public void PlaceButtonClicked()
    {
        Debug.Log($"Clicked, territory is: {Territory}");
        if(Territory != null)
        {
            Debug.Log($"Owner armies: {Territory.Owner.ArmiesToGive.Count}");
            if(Territory.Owner.ArmiesToGive.Count > 0)
            {
                var army = Territory.Owner.ArmiesToGive[0];
                Territory.Owner.ArmiesToGive.RemoveAt(0);
                Territory.AddArmy(army);
                display(Territory); // refresh
            }
        }
    }

    static bool LockedToTerritory = false;

    bool shouldMoveBtnBeVisible()
    {
        if (GameManager.CurrentStage != GameStage.Attack && GameManager.CurrentStage != GameStage.Movement)
            return false;
        if ((Territory?.TotalDefendingArmies ?? 0) == 0)
            return false;
        return true;
    }

    private void display(Territory t, bool preventOtherRemove = false)
    {
        if(LockedToTerritory && !preventOtherRemove)
        {// we are locked, and we force another lock, so we don't update
        } else
        { // not locked, or are allowed to override
            Territory = t;
        }
        if(preventOtherRemove)
        { // allow us to set the lock
            // lock is set to True if territory is somethting, or False if it is null
            LockedToTerritory = Territory != null;
        }

        // run UI update as normal, in case armies have changed or something
        if (Territory == null)
        {
            // no territory, so we hide the display (move off screen)
            Transform.localPosition = OriginalPosition + new Vector3(0, OriginalPosition.y * 2);
        }
        else
        {
            // show display and update text
            Transform.localPosition = OriginalPosition;
            this.gameObject.SetActive(true);
            Name.text = Territory.Name;
            Owner.text = Territory.Owner.Name;
            Infantry.text = $"Infantry:  {Territory.NumInfantry}";
            Cavalery.text = $"Cavalery:  {Territory.NumCavalery}";
            Artillery.text = $"Artillery: {Territory.NumArtillery}";

            MoveAttackText.text = GameManager.CurrentStage == GameStage.Attack ? "Attack" : "Move";
            PlaceButton.gameObject.SetActive(GameManager.CurrentStage == GameStage.Draft);
            MoveOrAttackBtn.gameObject.SetActive(shouldMoveBtnBeVisible());

            
            // Extra things that only make sense if the player can click on them.
            if(LockedToTerritory)
            { // Set their text

            }
        }
        UIScript.UpdateUI(); // also update ui in case other things have changed
    }

    public static void DisplayOnClick(Territory t)
    {
        instance.display(t, true);
    }

    public static void Display(Territory t)
    {
        instance.display(t);
    }

    private void Awake()
    {
        instance = this;
    }
}