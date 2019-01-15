using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Territory {

    public readonly string Name;
    public readonly Continent Continent;
    public int ID;
    public List<Territory> WhereCanMove = new List<Territory>();

    public Territory(string name, Continent continent)
    {
        Name = name;
        Continent = continent;
    }

    public Player Owner = GameManager.NotOwned; // default is Neutral until it is later claimed

    public List<Army> DefendingArmies = new List<Army>();
    public List<Army> AttackingArmies = new List<Army>();

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

    // Some statics
    class Node
    {
        public Territory Territory;
        public List<Node> Neighbours;
        public int Distance;
        public bool Visited;
        public Node(Territory t)
        {
            Territory = t;
            Distance = int.MaxValue;
            Visited = false;
        }

        public override string ToString()
        {
            return $"{Territory.Name} {Visited} {Distance}";
        }
    }

    static List<Node> allNodes = null;
    static List<Node> unvisited = null;

    /// <summary>
    /// Returns the next node to go to, or null
    /// </summary>
    /// <param name="current">Current node we are at</param>
    /// <param name="to">Node of destination we are going to</param>
    /// <returns></returns>
    static Node HandleNode(Node current, Node to)
    {
        var toConsider = current.Neighbours.Where(x => !x.Visited);
        Debug.Log($"Neighbours of {current.Territory.Name}: {string.Join("\r\n- ", current.Neighbours)}");
        foreach (var neighbour in toConsider)
        {
            int distance = current.Distance;
            if (neighbour.Distance == int.MaxValue)
                distance += 1;
            else
                distance += neighbour.Distance;
            Debug.Log($"For {current}, on neighbour {neighbour}: {distance} vs {neighbour.Distance}");
            if (neighbour.Distance > distance)
                neighbour.Distance = distance;
        }
        current.Visited = true;
        unvisited.Remove(current);

        if (to.Visited == true)
        {
            throw new System.Exception("Done!");
        }
        Node lowest = null;
        int lowestDistance = int.MaxValue;
        foreach (var nodes in toConsider)
        {
            if (nodes.Distance < lowestDistance)
            {
                lowest = nodes;
                lowestDistance = nodes.Distance;
            }
        }
        Debug.Log($"For {current}, returning {lowest}");
        return lowest;
    }

    public static bool CouldMoveArmyThroughPath(Army army, Territory from, Territory to,  List<Territory> path)
    {
        var owner = army.Owner;
        for(int index =0; index < path.Count; index++)
        {
            var t = path.ElementAtOrDefault(index);
            if (t == null || t == to)
                break;
            if (index == 0 && t.Name != from.Name)
                throw new InvalidOperationException("First territory in 'path' variable is expected to match 'from' variable");
            if (index == path.Count - 1 && t.Name != to.Name)
                throw new InvalidOperationException("Last territory in 'path' variable is expected to match 'to' variable");
            var next = path.ElementAtOrDefault(index + 1);
            if (next == null && t.Name != to.Name)
                throw new InvalidOperationException("Path ends before it reaches 'to' territory - path never connects to it");
            if (!t.WhereCanMove.Contains(next))
                throw new InvalidOperationException($"Cannot move from {t} to {next} in path (no connection)");
            if(t.Owner != next.Owner)
                throw new InvalidOperationException($"Cannot move from {t} to {next} in path (owners different)");
            if(t.Owner != owner)
                throw new InvalidOperationException($"Cannot move from {t} to {next} in path (army owner different)");
        }
        return true;
    }

    public static bool AttemptOrFailToMove(Territory from, Territory to)
    { // This doesn't work! Attempted to implement https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm but couldn't
        unvisited = new List<Node>();
        allNodes = new List<Node>();
        foreach (var t in GameManager.Territories)
        {
            var nNode = new Node(t);
            unvisited.Add(nNode);
            allNodes.Add(nNode);
        }
        foreach (var node in unvisited)
        {
            node.Neighbours = unvisited.Where(x => node.Territory.WhereCanMove.Contains(x.Territory)).ToList();
        }

        var currentNode = unvisited.FirstOrDefault(x => x.Territory.Name == from.Name);
        currentNode.Distance = 0;
        var destination = unvisited.FirstOrDefault(x => x.Territory.Name == to.Name);

        Debug.Log($"Move {currentNode.Territory.Name} -> {destination.Territory.Name}");

        List<Node> Path = new List<Node>() { currentNode };

        do
        {
            var nextNode = HandleNode(currentNode, destination);
            Path.Add(nextNode);
            currentNode = nextNode;
            Debug.Log($"Next node is {currentNode}, remaining: {unvisited.Count}");
        } while (currentNode.Territory.Name != to.Name && (unvisited.Count > 1 || unvisited[0].Distance != int.MaxValue));

        return Path.LastOrDefault().Territory.Name == to.Name;
    }
}
