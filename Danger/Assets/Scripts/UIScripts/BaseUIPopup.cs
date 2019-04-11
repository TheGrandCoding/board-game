using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class BaseUIPopup : MonoBehaviour
{
    public Territory Context;
    public TerritoryDisplayCriteria Criteria;

    public object SavedObject; // persistent object, perhaps to allow for remembering of what it actually is
    // or for the popup to give arguments back to the callback function

    public bool IsDisplayed
    {
        get
        {
            return gameObject.activeInHierarchy;
        }
        set
        {
            gameObject.SetActive(value);
        }
    }
    public Button ConfirmBtn;

    /// <summary>
    /// Confirm button being clicked raises this event
    /// </summary>
    public void ClickConfirm()
    {
        PriorConfirm();
        UIHelper.ConfirmedSoRunTask(Context);
    }

    public virtual void PriorConfirm() { }

    public abstract void Display(Territory t);

    public virtual bool DisplayIfSatisfy(Territory t)
    {
        if (this.Criteria == null)
            return false;
        if (IsDisplayed == false)
            return false;
        if (this.Criteria.DoesSatisfy(t))
        {
            this.Display(t);
            return true;
        }
        return false;
    }

    public virtual void SetCriteria(TerritoryDisplayCriteria crit)
    {
        Criteria = crit;
        UpdateForNew();
    }

    public abstract void UpdateForNew();
}
