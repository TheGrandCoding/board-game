﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using RandomGens;

public class GameManager : MonoBehaviour {

    public static List<Player> Players;
    public static List<Continent> Continents;
    public static List<Territory> Territories { get
        {
            return Continents.SelectMany(x => x.Territories).ToList();
        } }
    public static List<DangerCard> RemainingCards;
    private static GameManager Instance;
    void Awake()
    {
        Instance = this;
    }

    public static Player NotOwned = new Player("Neutral");

    public static Player StartPlayer;

    public class ReadyEventArgs : System.EventArgs
    {
        public readonly bool Success;
        public ReadyEventArgs(bool success) : base()
        {
            Success = success;
        }
    }
    public static System.EventHandler<ReadyEventArgs> Ready;


    public static Continent NorthAmerica { get { return Continents.FirstOrDefault(x => x.Name == "North America"); } }
    public static Continent SouthAmerica { get { return Continents.FirstOrDefault(x => x.Name == "South America"); } }
    public static Continent Europe { get { return Continents.FirstOrDefault(x => x.Name == "Europe"); } }
    public static Continent Africa { get { return Continents.FirstOrDefault(x => x.Name == "Africa"); } }
    public static Continent Australia { get { return Continents.FirstOrDefault(x => x.Name == "Australia"); } }
    public static Continent Asia { get { return Continents.FirstOrDefault(x => x.Name == "Asia"); } }

    public static Territory GetTerritory(string name)
    {
        return Territories.FirstOrDefault(x => x.Name == name);
    }

    private static bool started = false;
    private static bool startedSuccessfully = false;
    public static void Initialise()
    {
        if (started)
            return;
        started = true;

        Players = new List<Player>();
        Continents = new List<Continent>();
        RemainingCards = new List<DangerCard>();
        CreateContinents();
        SetInternalMovement();
        SetExternalMovement();
        CreateDangerCards();
        CheckPlayers();

        if (Ready == null)
        {
        } else
        {
            Debug.LogWarning("Invoked");
            startedSuccessfully = true;
            Ready.Invoke(null, new ReadyEventArgs(startedSuccessfully));
        }

        InitialStartGame();
    }
    private static void CreateContinents()
    {
        var northAmerica = new Continent("North America", 5);
        northAmerica.Territories = new List<Territory>()
        {
            new Territory("Alaska", northAmerica),
            new Territory("Alberta", northAmerica),
            new Territory("Central America", northAmerica),
            new Territory("Eastern US", northAmerica),
            new Territory("Greenland", northAmerica),
            new Territory("Central America", northAmerica),
            new Territory("Northwest Territory", northAmerica),
            new Territory("Ontario", northAmerica),
            new Territory("Quebec", northAmerica),
            new Territory("Western US", northAmerica),
        };
        northAmerica.SetIds();
        Continents.Add(northAmerica);

        var southAmerica = new Continent("South America", 2);
        southAmerica.Territories = new List<Territory>()
        {
            new Territory("Argentina", southAmerica),
            new Territory("Brazil", southAmerica),
            new Territory("Peru", southAmerica),
            new Territory("Venezuela", southAmerica),
        };
        southAmerica.SetIds();
        Continents.Add(southAmerica);

        var europe = new Continent("Europe", 5);
        europe.Territories = new List<Territory>()
        {
            new Territory("Great Britain", europe),
            new Territory("Iceland", europe),
            new Territory("Northern Europe", europe),
            new Territory("Scandinavia", europe),
            new Territory("Southern Europe", europe),
            new Territory("Ukraine", europe),
            new Territory("Western Europe", europe),
        };
        europe.SetIds();
        Continents.Add(europe);

        var africa = new Continent("Africa", 3);
        africa.Territories = new List<Territory>()
        {
            new Territory("Congo", africa),
            new Territory("East Africa", africa),
            new Territory("Egypt", africa),
            new Territory("Madagascar", africa),
            new Territory("West Africa", africa),
            new Territory("South Africa", africa),
        };
        africa.SetIds();
        Continents.Add(africa);

        var asia = new Continent("Asia", 7);
        asia.Territories = new List<Territory>()
        {
            new Territory("Afganistan", asia),
            new Territory("China", asia),
            new Territory("India", asia),
            new Territory("Irkutsk", asia),
            new Territory("Japan", asia),
            new Territory("Kamchatka", asia),
            new Territory("Middle East", asia),
            new Territory("Mongolia", asia),
            new Territory("Siam", asia),
            new Territory("Siberia", asia),
            new Territory("Ural", asia),
            new Territory("Yakutsk", asia),
        };
        asia.SetIds();
        Continents.Add(asia);

        var australia = new Continent("Australia", 2);
        australia.Territories = new List<Territory>()
        {
            new Territory("Eastern Australia", australia),
            new Territory("Indonesia", australia),
            new Territory("New Guinea", australia),
            new Territory("Western Australia", australia),
            new Territory("New Zealand", australia) // since liliana added it
        };
        australia.SetIds();
        Continents.Add(australia);

    }

    private static void SetInternalMovement()
    {
        // since each Territory can only move to a set list of other Territories
        // we need to define where you can move, from each
        // BUT: this function only deals with movement WITHIN their own continent

        // North America //
        NorthAmerica.AllMove(1, 6, 2);
        NorthAmerica.AllMove(2, 6, 7, 9); // doesnt matter if it repeats
        NorthAmerica.AllMove(3, 9, 4);
        NorthAmerica.AllMove(4, 3, 9, 7, 8);
        NorthAmerica.AllMove(5, 6, 7, 8);
        NorthAmerica.AllMove(6, 1, 2, 7, 5);
        NorthAmerica.AllMove(7, 6, 2, 9, 4, 5);
        NorthAmerica.AllMove(8, 6, 7, 5, 4);
        NorthAmerica.AllMove(9, 2, 7, 4, 3);

        // South Africa //
        SouthAmerica.AllMove(1, 2, 3);
        SouthAmerica.AllMove(2, 1, 3, 4);
        SouthAmerica.AllMove(3, 1, 2, 4);
        SouthAmerica.AllMove(4, 3, 2);

        // Europe //
        Europe.AllMove(1, 2, 4, 3, 7);
        Europe.AllMove(2, 1, 4);
        Europe.AllMove(3, 1, 4, 5, 6, 7);
        Europe.AllMove(4, 1, 2, 3, 6);
        Europe.AllMove(5, 7, 3, 6);
        Europe.AllMove(6, 4, 3, 5);
        Europe.AllMove(7, 1, 3, 5);

        // Africa // - idk why im doing t hese
        Africa.AllMove(1, 2, 5, 6);
        Africa.AllMove(2, 3, 5, 1, 6, 4);
        Africa.AllMove(3, 5, 2);
        Africa.AllMove(4, 6, 2);
        Africa.AllMove(5, 3, 1);
        Africa.AllMove(6, 1, 2, 4);

        // Asia //
        Asia.AllMove(1, 2, 3, 11, 7);
        Asia.AllMove(2, 10, 11, 1, 3, 9, 8);
        Asia.AllMove(3, 7, 1, 2, 9);
        Asia.AllMove(4, 10, 12, 6, 8);
        Asia.AllMove(5, 8, 6);
        Asia.AllMove(6, 12, 4, 8, 5);
        Asia.AllMove(7, 1, 3);
        Asia.AllMove(8, 10, 4, 5, 2);
        Asia.AllMove(9, 3, 2);
        Asia.AllMove(10, 11, 2, 8, 4, 12);
        Asia.AllMove(11, 1, 2, 10);
        Asia.AllMove(12, 10, 4, 6);

        // Australia
        Australia.AllMove(1, 3, 4);
        Australia.AllMove(2, 3, 4);
        Australia.AllMove(3, 1, 2, 4);
        Australia.AllMove(4, 2, 1, 3, 5);
        Australia.AllMove(5, 4);
    }

    private static void SetExternalMovement()
    {
        // movement allowed intercontinenally
        // some pairs have been put twice
        // others have been ommited if previously done

        // North America
        NorthAmerica.PairMove(1, Asia, 6); // sea
        NorthAmerica.PairMove(5, Europe, 2); // sea
        NorthAmerica.PairMove(3, SouthAmerica, 4); // land

        // South America
        SouthAmerica.PairMove(2, Africa, 5); // sea
        SouthAmerica.PairMove(4, NorthAmerica, 3); // land

        // Europe
        Europe.PairMove(2, NorthAmerica, 5); // sea
        Europe.PairMove(7, Africa, 5); // s
        Europe.PairMove(5, Africa, 3); // s
        Europe.PairMove(5, Asia, 7); // l
        Europe.PairMove(6, Asia, 7, 1, 11); // l

        // Africa
        Africa.PairMove(5, SouthAmerica, 2); // l
        Africa.PairMove(3, Europe, 5); // l
        Africa.PairMove(3, Asia, 7); // l 
        Africa.PairMove(2, Asia, 7); // s

        // Asia
        Asia.PairMove(9, Australia, 2);
    }

    private static void CreateDangerCards()
    {
        // each territory has 1 card, 
        var dangerCards = new List<DangerCard>();
        foreach (var territory in Territories)
        {
            var newCard = new DangerCard(territory, RandomGens.ArmyTypes.ThrowDice());
            dangerCards.Add(newCard);
        }
        // plus two 'Wild' cards, that can act as any army type
        dangerCards.Add(new DangerCard());
        dangerCards.Add(new DangerCard());

        RemainingCards = dangerCards;
    }

    private static void CheckPlayers()
    {
        Players = PlayerManager.Players;
        foreach (var p in Players)
        {
            Debug.Log(p.Name);
        }
    }

    private static void InitialStartGame()
    {
#if DEBUG
        if (Players == null || Players.Count == 0)
        {
            Players = new List<Player>() {
                new Player("Alex"),
                new Player("Bob")
            };

        }
#endif
        Start_ChosePlayer();
#if DEBUG
        //Debug_Start_RandomCapitals();
        //#else
        Start_SelectCapitals();
#endif
    }

    private static void Start_ChosePlayer()
    {
        Dictionary<Player, int> DiceRolls = new Dictionary<Player, int>();
        foreach (var player in Players)
        {
            var roll = Dice.RollSixSided();
            DiceRolls.Add(player, roll);
        }
        Player highestPlayer = null;
        int highestRoll = 0;

        foreach (var keypair in DiceRolls)
        {
            if (keypair.Value > highestRoll)
            {
                highestRoll = keypair.Value;
                highestPlayer = keypair.Key;
            }
        }

        StartPlayer = highestPlayer;
        int startIndex = Players.IndexOf(StartPlayer);
        int newIndexOfPlayers = 0;
        while (newIndexOfPlayers < Players.Count)
        {
            var player = Players[startIndex];
            player.ChoiceIndex = newIndexOfPlayers;
            newIndexOfPlayers++;
            startIndex++;
            if (startIndex >= Players.Count)
                startIndex = 0;
        }
        Players = Players.OrderBy(x => x.ChoiceIndex).ToList();
        Debug.Log("Start player: " + highestPlayer.Name);

    }

    private static void Debug_Start_RandomCapitals()
    {
        foreach (var player in Players)
        {
            while (player.CapitalCity == null)
            {
                var randomTerritory = RndHelp.Choose<Territory>(Territories);
                Start_SetCapital(player, randomTerritory);
            }
        }
    }


    private static void Start_SetCapital(Player p, Territory capital)
    {
        if (p.CapitalCity == null)
        {
            Debug.Log("Setting capital of " + p.Name + " to " + capital.ToString());
            p.CapitalCity = capital;
            capital.SetOwner(p);
        } else
        {
            Debug.LogWarning(string.Format("Attempted to set {0}'s capital to {1}, but it was already set as {2}", p.Name, capital.Name, p.CapitalCity.Name));
        }
    }
    private static void Start_SetCapital(string p, string name) // since im not sure how we'll be implementing it, i'll add this in too
    {
        Start_SetCapital(Players.FirstOrDefault(x => x.Name == p), GetTerritory(name));
    }
    private static void Start_RandomiseRemainingTerritories()
    {
        Debug.Log("Start rand remaining");
        var remaining = Territories.Where(x => x.Owner == null || x.Owner.Name == NotOwned.Name).ToList();

        while (remaining.Count > 0)
        {
            foreach (var player in Players)
            {
                var terr = remaining.First();
                remaining.RemoveAt(0);
                terr.SetOwner(player);
            }
            remaining = Territories.Where(x => x.Owner == null || x.Owner.Name == NotOwned.Name).ToList();
        }
    }

    private static void Start_SelectCapitals()
    {
        var p1 = Players[0];
        UIHelper.RunOnTerritoryConfirmed(
            new TerritoryDisplayCriteria(ownedBy:NotOwned),
            playerConfirmedTerritory, p1);

    }

    private static void playerConfirmedTerritory(Territory t, TerritoryDisplayCriteria c, object state)
    {
        var player = (Player)state;
        Start_SetCapital(player, t);
        var nextid = player.ChoiceIndex + 1;
        var nextPlayer = Players.ElementAtOrDefault(nextid);
        if(nextPlayer == null)
        {
            Debug.Log("All capitals done! Randomising territories..");
            Start_RandomiseRemainingTerritories();
        } else
        {
            UIHelper.RunOnTerritoryConfirmed(
                new TerritoryDisplayCriteria(ownedBy: NotOwned),
                playerConfirmedTerritory, nextPlayer);
        }
    }


    public static bool CanMove(Territory t1, Territory t2)
    {
        return t1.WhereCanMove.Contains(t2) || t2.WhereCanMove.Contains(t1); 
        // shouldn't need second statement, but got it just in case
        // it wont be executed anyway if the first half returns true
    }

    // Use this for initialization
    void Start () {
        GameManager.Ready += GameStart;
        Initialise();
        //Debug.Log(Europe.Display());
        //Debug.Log(Europe.GetTerritory(1).Display(true));

    }

    public void GameStart(object sender, ReadyEventArgs e)
    {
        GameManager.Ready -= GameStart;
        foreach (var continent in Continents)
        {
            string str = continent.Name + ".";
            str += continent.Territories.Count.ToString() + ".";
            foreach (var t in continent.Territories)
            {
                str += t.Display(true) + ", ";
            }
            Debug.Log(str);
        }
    }
	
	void FixedUpdate () {
        if (Ready != null && startedSuccessfully)
            Ready.Invoke(null, new ReadyEventArgs(true));
	}
}
