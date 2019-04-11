using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Extensions;
using UnityEngine.UI;

public class Territory {

    public readonly string Name;
    public readonly Continent Continent;
    public int ID;
    public List<Territory> WhereCanMove = new List<Territory>();
    public Image Image;
    public Color ModColor {  get
        {
            return Owner.PlayerColor;
        } }

    public Territory(string name, Continent continent)
    {
        Name = name;
        Continent = continent;
    }

    public void SetOwner(Player newOwner)
    {
        try
        {
            newOwner = newOwner ?? GameManager.NotOwned;
            Owner = newOwner;
            if (Image == null)
                Debug.LogWarning(this.ToString() + ": Image null");
            if (ModColor == null)
                Debug.LogWarning(this.ToString() + ": ModColor null");
            Image.color = ModColor;
        } catch (Exception ex)
        {
            Debug.LogError("Errored in " + this.ToString() + ", ex: " + ex.ToString());
        }
    }

    public Player Owner { get; private set; } = GameManager.NotOwned; // default is Neutral until it is later claimed

    public List<Army> DefendingArmies = new List<Army>();
    public List<Army> AttackingArmies = new List<Army>();

    // Cache/store some information to prevent endless repeating of the above.
    public int NumInfantry = 0;
    public int NumCavalery = 0;
    public int NumArtillery = 0;
    public int TotalDefendingArmies => NumInfantry + NumArtillery + NumCavalery;

    public void AddArmy(Army toAdd)
    {
        DefendingArmies.Add(toAdd);
        if (toAdd.Type == ArmyType.Artillery)
            NumArtillery++;
        else if (toAdd.Type == ArmyType.Cavalry)
            NumCavalery++;
        else if (toAdd.Type == ArmyType.Infantry)
            NumInfantry++;
    }

    private void updateInternal()
    {
        NumInfantry = DefendingArmies.Where(x => x.Type == ArmyType.Infantry).Count();
        NumCavalery = DefendingArmies.Where(x => x.Type == ArmyType.Cavalry).Count();
        NumArtillery = DefendingArmies.Where(x => x.Type == ArmyType.Artillery).Count();
    }

    public Army RemoveArmy(ArmyType type = ArmyType.NotSet)
    {
        var army = DefendingArmies.FirstOrDefault(x => type == ArmyType.NotSet || x.Type == type);
        if(army != null)
            DefendingArmies.Remove(army);
        updateInternal();
        return army;
    }

    public string Display(bool displayWhereCanMove = false)
    {
        string where = "";
        if (displayWhereCanMove)
        {
            where = " / Moves: " + string.Join(", ", WhereCanMove.Select(x => x.Continent.Name + " " + x.Name).ToArray());
        }
        return string.Format("[{0}] {1} - {2} vs {3} | {4}{5}", ID, Name, DefendingArmies.Count, AttackingArmies.Count, Owner.Name, displayWhereCanMove ? where : "");
    }

    /// <summary>
    /// CN = Continent Name
    /// NM = Name
    /// ID = ID
    /// OWN = Owner Name
    /// </summary>
    public string ToString(string format)
    {
        format = format.Replace("CN", "{0}");
        format = format.Replace("NM", "{1}");
        format = format.Replace("ID", "{2}");
        format = format.Replace("OWN", "{3}");
        return string.Format(format, this.Continent.Name, this.Name, this.ID, this.Owner == null ? "" : this.Owner.Name);
    }
    public override string ToString()
    {
        return this.ToString("CN NM");
    }


    class LinkedNode : Node
    {
        public LinkedNode Parent;
        public List<LinkedNode> LinkedNeighbours
        {
            get
            {
                List<LinkedNode> toBe = new List<LinkedNode>();
                foreach(var node in Neighbours)
                {
                    if (node.Territory == Parent?.Territory)
                        continue;
                    toBe.Add(node.ToLinked(this));
                }
                return toBe;
                //Neighbours.Select(x => x.ToLinked(this)).ToList();
            }
        }
        public LinkedNode(Node node, LinkedNode parent) : base(node.Territory)
        {
            Parent = parent;
        }

        public List<LinkedNode> GetNodes(int depth, Player owner = null)
        {
            if (this == null)
                Debug.LogWarning($"this null");
            if (this.Territory == null)
                Debug.LogWarning($"this territory null");
            if (this.Neighbours == null)
                Debug.LogWarning($"{this.Territory.Name} this neighbours null");
            Debug.Log($"{this.Territory.Name} - {string.Join(", ", this.Neighbours)}");
            List<LinkedNode> nodes = new List<LinkedNode>();
            if (owner != null && this.Territory.Owner != owner)
                return nodes; // since they cant pass here anyway, dont even bother to try.
            if (depth == 0)
            {
                visited.UniqueAdd(this);
                Debug.Log($"D{depth} returning neighbours of {this.Territory.Name}: {string.Join(", ", this.Neighbours)}");
                nodes = LinkedNeighbours.ToList();
            }
            else
            {
                Debug.Log($"D{depth} returning depth below of {this.Territory.Name} neighbours: {string.Join(", ", this.Neighbours)}");
                foreach (var n in LinkedNeighbours)
                {
                    nodes.AddRange(n.GetNodes(depth - 1));
                }
            }
            return nodes;
        }

        public List<LinkedNode> GetParents()
        {
            var list = new List<LinkedNode>();
            if(Parent != null)
                list.AddRange(Parent.GetParents());
            list.Add(this);
            return list;
        }
    }

    // Some statics
    class Node
    {
        public Territory Territory;
        public List<Node> Neighbours;
        public Node(Territory t)
        {
            Territory = t;
        }

        public LinkedNode ToLinked(LinkedNode parent)
        {
            var ll = new LinkedNode(this, parent);
            ll.Neighbours = allNodes.Where(x => ll.Territory.WhereCanMove.Contains(x.Territory)).ToList();
            return ll;
        }

        public override string ToString()
        {
            return $"{Territory.Name}";
        }
    }

    static List<Node> allNodes = null;
    static List<LinkedNode> visited = null;
    static List<List<Node>> Paths = new List<List<Node>>();

    static bool CouldMoveArmyThroughPath(Player owner, Territory from, Territory to,  List<LinkedNode> path, bool invade)
    {
        for(int index =0; index < path.Count; index++)
        {
            var t = path.ElementAtOrDefault(index);
            if (t == null || t.Territory == to)
                break;
            if (index == 0 && t.Territory.Name != from.Name)
                throw new InvalidOperationException("First territory in 'path' variable is expected to match 'from' variable");
            if (index == path.Count - 1 && t.Territory.Name != to.Name)
                throw new InvalidOperationException("Last territory in 'path' variable is expected to match 'to' variable");
            var next = path.ElementAtOrDefault(index + 1);
            if (next == null && t.Territory.Name != to.Name)
                throw new InvalidOperationException("Path ends before it reaches 'to' territory - path never connects to it");
            if (!t.Territory.WhereCanMove.Contains(next.Territory))
                throw new InvalidOperationException($"Cannot move from {t} to {next} in path (no connection)");
            if(t.Territory.Owner != next.Territory.Owner && !invade)
                throw new InvalidOperationException($"Cannot move from {t} to {next} in path (owners different)");
            if(t.Territory.Owner != owner)
                throw new InvalidOperationException($"Cannot move from {t} to {next} in path (army owner different)");
        }
        return true;
    }

    public static bool AttemptOrFailToMove(Player owner, Territory from, Territory to, bool invade = false)
    { // Function uses the https://en.wikipedia.org/wiki/Breadth-first_search algorithm to search

        if (from.TotalDefendingArmies == 1)
            return false; // Unable to cause a territory to be undefended, so refuse.

        visited = new List<LinkedNode>();
        allNodes = new List<Node>();
        foreach (var t in GameManager.Territories)
        {
            var nNode = new Node(t);
            allNodes.Add(nNode);
        }
        foreach (var node in allNodes)
        {
            node.Neighbours = allNodes.Where(x => node.Territory.WhereCanMove.Contains(x.Territory)).ToList();
            Debug.Log($"{node.Territory.Name}: {string.Join(", ", node.Neighbours)}");
        }

        var StartNode = allNodes.FirstOrDefault(x => x.Territory.Name == from.Name);
        var StartLinked = StartNode.ToLinked(null);
        var destination = allNodes.FirstOrDefault(x => x.Territory.Name == to.Name);

        LinkedNode doneWith = null;
        int lastVisitedCount = visited.Count;
        int depth = -1;
        int limitDepth = 3;
        int RAW_MAX_AMOUNTS = 0; // final last ditch effort to break out of loop if all else fails
        do
        {
            RAW_MAX_AMOUNTS++;
            depth++;
            lastVisitedCount = visited.Count;
            Debug.Log($"-{depth}: {StartLinked.Territory.Name} {string.Join(", ", StartLinked.Neighbours)} ");
            var lookingAt = StartLinked.GetNodes(depth);
            foreach (var node in lookingAt)
            {
                Debug.Log($"#{depth}: {string.Join(" -> ", node.GetParents())}");
                visited.Add(node);
                if(node.Territory.Name == to.Name)
                {
                    try
                    {
                        if(CouldMoveArmyThroughPath(owner, from, to, node.GetParents(), invade))
                        {
                            Debug.LogWarning("Done! With above.");
                            doneWith = node;
                            break;
                        }
                    } catch (InvalidOperationException ex)
                    {
                        Debug.LogWarning("Considered the above, but unable to because: " + ex.Message);
                        limitDepth = depth + 3; // since we are around the target, we'll allow just three must varities to get there
                        // then we'll call it quits and say its not possible
                        if (limitDepth > 6)
                            limitDepth = 6; // prevent waaay over
                    }
                }
            }
            limitDepth--;
        } while (visited.Count > lastVisitedCount && doneWith == null && limitDepth > depth && RAW_MAX_AMOUNTS < 20);
        return doneWith != null;
    }

    List<Army> getArmiesForBattle(List<Army> totalArmiesPool)
    {
        List<Army> armies = new List<Army>();
        while(armies.Count < totalArmiesPool.Count && armies.Count <= 3)
        { // just randomly get three armies, or as many as possible
            // in the future: could select the 'best' armies
            armies.Add(RandomGens.RndHelp.Choose<Army>(totalArmiesPool));
        }
        return armies;
    }

    List<Army> DoBattle(List<Army> attackers)
    {
        var attackingRolls = attackers.Select(x => x.RollAttack());
        var orderedAttacking = attackingRolls.OrderByDescending(x => x).ToList();

        var defenders = getArmiesForBattle(this.DefendingArmies);
        var defenceRolls = defenders.Select(x => x.RollDefence());
        var orderedDefence = defenceRolls.OrderByDescending(x => x).ToList();

        // NOTE: this does not care about which units actually did the roll
        // so should probably be chagned (use a dict<int, Army>?)
        while(defenders.Count > 0 && attackers.Count > 0 && orderedAttacking.Count() > 0 && orderedDefence.Count() > 0)
        {
            var attack = orderedAttacking.ElementAt(0);
            var defence = orderedDefence.ElementAt(0);
            Debug.Log($"Battle: {attack} vs {defence}");
            if(attack > defence)
            {
                var lostDefender = defenders[0];
                this.DefendingArmies.Remove(lostDefender);
                defenders.Remove(lostDefender);
            } else
            { // less than OR EQUAL TO, favour defence
                var lostAttacker = attackers[0];
                attackers.Remove(lostAttacker);
            }
            orderedAttacking.RemoveAt(0);
            orderedDefence.RemoveAt(0);
        }
        this.DefendingArmies = defenders;
        Debug.Log($"Battle ends: {attackers.Count} vs {this.DefendingArmies.Count}");
        updateInternal();
        return attackers;
    }

    public List<Army> GetAttacked(List<Army> attackers)
    {
        while (this.TotalDefendingArmies > 0 && attackers.Count > 0)
        {
            attackers = getArmiesForBattle(attackers);
            Debug.Log($"Battle starting, {this.TotalDefendingArmies} vs {attackers.Count}");
            attackers = DoBattle(attackers);
        }
        Debug.Log($"Attack ends: {attackers.Count} vs {this.DefendingArmies.Count}");
        return attackers;
    }

}
