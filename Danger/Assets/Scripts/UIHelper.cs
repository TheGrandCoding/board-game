using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class UIHelper : MonoBehaviour
{
    private static UIHelper instance;
    void Awake() { instance = this; }
    public static UIPopupScript CurrentPopUp;
    public static TerritoryDisplayCriteria CurrentCriteria;
    private static Action<Territory, TerritoryDisplayCriteria, object> CurrentAction;
    private static object SavedObject;

    public static void ConfirmedSoRunTask(Territory t)
    {
        CurrentCriteria = null;
        CurrentPopUp.IsDisplayed = false;
        CurrentAction.Invoke(t, CurrentCriteria, SavedObject);
    }

    public static void RunOnTerritoryConfirmed(TerritoryDisplayCriteria criteria, Action<Territory, TerritoryDisplayCriteria, object> function, object passedParam = null)
    {
        if(CurrentCriteria != null)
        {
            Debug.LogWarning("New RunOnTerritory may override an old one");
        }
        CurrentAction = function;
        SavedObject = passedParam;
        CurrentCriteria = criteria;
        CurrentPopUp = CurrentPopUp ?? GameObject.FindGameObjectWithTag("UIPopup").GetComponent<UIPopupScript>();
        CurrentPopUp.WantingTerritory(criteria);
        CurrentPopUp.IsDisplayed = true;
    }
}
