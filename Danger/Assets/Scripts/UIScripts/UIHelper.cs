using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIHelper : MonoBehaviour
{
    private static UIHelper instance;
    void Awake() { instance = this; }
    public static BaseUIPopup CurrentPopUp;
    public static TerritoryDisplayCriteria CurrentCriteria;
    private static Action<Territory, TerritoryDisplayCriteria, object> CurrentAction;
    private static object SavedObject;


    public TerritorySelectPopup TerritorySelect;
    public SelectArmyPopup MoveArmy;

    public static void ConfirmedSoRunTask(Territory t)
    {
        CurrentCriteria = null;
        CurrentPopUp.IsDisplayed = false;
        Debug.Log(t.Name);
        CurrentAction.Invoke(t, CurrentCriteria, SavedObject);
    }

    public static void SelectTerritory(TerritoryDisplayCriteria criteria, Action<Territory, TerritoryDisplayCriteria, object> function, object passedParam = null)
    {
        if(CurrentCriteria != null)
        {
            Debug.LogWarning($"New popup may override an old one");
        }
        CurrentAction = function;
        SavedObject = passedParam;
        CurrentCriteria = criteria;
        CurrentPopUp = instance.TerritorySelect;
        CurrentPopUp.SetCriteria(criteria);
        CurrentPopUp.IsDisplayed = true;
    }

    public static void SelecyArmyAmount(TerritoryDisplayCriteria criteria, Action<Territory, TerritoryDisplayCriteria, object> callback, object passedParam = null)
    {
        if(CurrentCriteria != null)
        {
            Debug.LogWarning($"New popup may override an old one");
        }
        CurrentAction = callback;
        SavedObject = passedParam;
        CurrentCriteria = criteria;
        CurrentPopUp = instance.MoveArmy;
        CurrentPopUp.SetCriteria(criteria);
        CurrentPopUp.IsDisplayed = true;
    }

    public static void TerritoryClicked(Territory t)
    {
        if (CurrentPopUp != null)
            CurrentPopUp.DisplayIfSatisfy(t);
    }

}
