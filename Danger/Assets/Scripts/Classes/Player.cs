using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player {

    public readonly string Name;
    public Player(string name)
    {
        Name = name;
    }
    public List<DangerCard> OwnedCards = new List<DangerCard>();
    public List<Territory> Territories { get
        {
            return GameManager.Territories.Where(x => x.Owner == this).ToList();
        } }
    public List<Army> ArmiesToGive = new List<Army>();
    public Territory CapitalCity;
    public int ChoiceIndex;


    public void IncreaseArmies()
    {
        if (ArmiesToGive.Count > 0)
        {
            //throw new System.InvalidOperationException($"{Name} still has armies remaining that they have not yet distributed");
            // im not sure if you can 'store' armies until later
        }

        int armiesToGain = (int)(Territories.Count / 3);
        if (armiesToGain < 3)
            armiesToGain = 3;

        Debug.Log($"{this.Name} base is {armiesToGain} (with {Territories.Count} controlled territories)");

        var continents = Territories.Select(x => x.Continent).Distinct(); // Distinct removes duplicate values

        Debug.Log($"{this.Name} controls territories in {string.Join(", ", continents.Select(x => x.Name))}");

        List<Continent> fullyControlledContinents = new List<Continent>();
        foreach(var cont in continents)
        {
            var who = cont.WhoControlsAll();
            if(who.HasValue && who.Value == this)
            {
                Debug.Log($"{this.Name} fully controls {cont.Name}");
                fullyControlledContinents.Add(cont);
            }
        }

        foreach(var fully in fullyControlledContinents)
        {
            Debug.Log($"{this.Name} gains {fully.FullControlAddition} from {fully.Name}");
            armiesToGain += fully.FullControlAddition;
        }

        Debug.Log($"{this.Name} will therefore gain {armiesToGain} armies");
        for(int i = 0; i < armiesToGain; i++)
        {
            var type = RandomGens.RndHelp.Choose(ArmyType.Artillery, ArmyType.Cavalry, ArmyType.Infantry);
            var newA = new Army(type);
            newA.Owner = this;
            ArmiesToGive.Add(newA);
        }
    }

    public override string ToString()
    {
        return Name;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
