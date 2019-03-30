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
        UIHelper.ConfirmedSoRunTask(Context);
    }

    public abstract void Display(Territory t);

    public virtual bool DisplayIfSatisfy(Territory t)
    {
        if (this.Criteria == null)
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
