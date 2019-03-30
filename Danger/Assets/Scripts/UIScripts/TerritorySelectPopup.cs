using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerritorySelectPopup : BaseUIPopup
{
    public Text Status;
    public Text Name;
    public Text Continent;
    public Text Owner;

    public static TerritorySelectPopup Script;

    public override void Display(Territory territory)
    {
        Status.text = "Confirm, or click another territory..";
        Context = territory;
        Debug.Log(Context.Name);
        Name.text = Context.Name;
        Continent.text = Context.Continent.Name;
        Owner.text = Context.Owner == GameManager.NotOwned ? "N/A" : "Owner: " + Context.Owner.Name;
    }

    public override void UpdateForNew()
    {
        string criteriaText = "Click on a territory that:";
        if (Criteria.MustBeInContinent.HasValue)
        {
            criteriaText += "\r\nIs in " + Criteria.MustBeInContinent.Value.Name;
        }
        if (Criteria.MustBeOwnedBy.HasValue)
        {
            criteriaText += "\r\nIs owned by " + Criteria.MustBeOwnedBy.Value.Name;
        }
        if (criteriaText == "Click on a territory that:")
            criteriaText = " Click on any territory";
        Status.text = criteriaText;
        Name.text = "...";
        Continent.text = "...";
        Owner.text = "...";
    }

    // Start is called before the first frame update
    void Start()
    {
        TerritorySelectPopup.Script = this;
    }
}
