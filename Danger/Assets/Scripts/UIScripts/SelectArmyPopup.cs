using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SelectArmyPopup : BaseUIPopup
{
    public Territory Territory;
    public Text Status;
    public Text InfLabel;
    public Text CavLabel;
    public Text ArtLabel;

    public int InfAmount;
    public int CavAmount;
    public int ArtAmount;

    public int MaxInfAmount => Territory.DefendingArmies.Where(x => x.Type == ArmyType.Infantry).Count();
    public int MaxCavAmount => Territory.DefendingArmies.Where(x => x.Type == ArmyType.Cavalry).Count();
    public int MaxArtAmount => Territory.DefendingArmies.Where(x => x.Type == ArmyType.Artillery).Count();

    public void ModifyAmount(ArmyType type, int amount)
    {
        if (type == ArmyType.Infantry)
            _modifyAmount(ref InfAmount, MaxInfAmount, amount);
        else if (type == ArmyType.Cavalry)
            _modifyAmount(ref CavAmount, MaxCavAmount, amount);
        else if (type == ArmyType.Artillery)
            _modifyAmount(ref InfAmount, MaxInfAmount, amount);
        Display(Territory);
    }

    void _modifyAmount(ref int current, int max, int howMuchBy)
    {
        if (current + howMuchBy <= max && current + howMuchBy >= 0) // account for max and also 0
            current += howMuchBy;
    }


    public static SelectArmyPopup Script;

    public override void Display(Territory territory)
    {
        Territory = territory;
        InfLabel.text = $"Inf: {InfAmount}/{MaxInfAmount}";
        CavLabel.text = $"Cav: {CavAmount}/{MaxCavAmount}";
        ArtLabel.text = $"Art: {ArtAmount}/{MaxArtAmount}";
        InfLabel.enabled = Territory != null;
        CavLabel.enabled = Territory != null;
        ArtLabel.enabled = Territory != null;
    }

    public override void UpdateForNew()
    {/*
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
            criteriaText = " Click on any territory";*/
        Status.text = $"Move armies (from)\r\nClick on your territory";
        InfLabel.enabled = Territory != null;
        CavLabel.enabled = Territory != null;
        ArtLabel.enabled = Territory != null;
    }

    void Awake()
    {
        SelectArmyPopup.Script = this;
    }
}
