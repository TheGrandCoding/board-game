using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;

public class UIHelper : MonoBehaviour
{
    private static UIHelper instance;
    void Awake() { instance = this; }
    public static UIPopupScript CurrentPopUp;
    public static TerritoryDisplayCriteria CurrentCriteria;

    private static Territory lastClickedSatisfy = null;
    public static Territory NextTerritoryClicked(TimeSpan? timeout = null, TerritoryDisplayCriteria criteria = null)
    {
        timeout = timeout ?? TimeSpan.FromSeconds(180);
        criteria = criteria ?? new TerritoryDisplayCriteria();

        CurrentPopUp = CurrentPopUp ?? GameObject.FindGameObjectWithTag("UIPopup").GetComponent<UIPopupScript>();
        Debug.Log("Line19");
        CurrentPopUp.gameObject.SetActive(true);
        CurrentPopUp.WantingTerritory(criteria);

        var cd = new CoroutineWithData(instance, NextTerritoryAsync(timeout.Value, criteria));
        yield return cd.coroutine;
        lastClickedSatisfy = (Territory)cd.result;

        while(lastClickedSatisfy == null)
        {
            yield return null;
        }
        CurrentPopUp.gameObject.SetActive(false);
        var item = lastClickedSatisfy;
        lastClickedSatisfy = null;
        yield return item;
    }

    private static IEnumerator NextTerritoryAsync(TimeSpan timeout, TerritoryDisplayCriteria criteria)
    {
        CurrentCriteria = criteria;
        var eventTrigger = new TaskCompletionSource<Territory>();

        void Handler(object sender, Territory territory)
        {
            var result = CurrentCriteria.DoesSatisfy(territory);
            if (result)
                eventTrigger.SetResult(territory);
        }
        UIPopupScript.TerritoryConfirmed += Handler;

        var trigger = eventTrigger.Task;
        var delay = Task.Delay((int)timeout.TotalMilliseconds);
        var task = Task.WhenAny(trigger, delay).ConfigureAwait(false);

        while(!task.GetAwaiter().IsCompleted)
        {
            yield return new WaitForFixedUpdate();
        }
        UIPopupScript.TerritoryConfirmed -= Handler;
    }
}
