﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryDisplayCriteria
{
    /// <summary>
    /// Territory must be within this continent
    /// </summary>
    public Optional<Continent> MustBeInContinent;
    /// <summary>
    /// Territory must NOT be within this continent
    /// </summary>
    public Optional<Continent> MustNOTBeInContinent;
    /// <summary>
    /// Territory must be owned by this player
    /// </summary>
    public Optional<Player> MustBeOwnedBy;
    /// <summary>
    /// Territory must not be owned by this player
    /// </summary>
    public Optional<Player> MustNOTBeOwnedBy;
    /// <summary>
    /// Territory must have atleast this many armies
    /// </summary>
    public Optional<int> MustHaveAtleastArmies;
    /// <summary>
    /// Territory must not have more than this many armies
    /// </summary>
    public Optional<int> MustHaveNoMoreThanArmies;

    /// <summary>
    /// Territory must have a route to this field.
    /// MustBeOwnedBy must also be specified
    /// </summary>
    public Optional<Territory> MoveableFromTerritory;

    /// <summary>
    /// Allows incursion into enemy territory.
    /// </summary>
    public bool AllowInvasion;

    /// <summary>
    /// Default criteria that accepts ANY territory clicked
    /// </summary>
    public TerritoryDisplayCriteria()
    {
        MustBeInContinent = new Optional<Continent>();
        MustNOTBeInContinent = new Optional<Continent>();
        MustBeOwnedBy = new Optional<Player>();
        MustNOTBeOwnedBy = new Optional<Player>();
        MustHaveAtleastArmies = new Optional<int>();
        MustHaveNoMoreThanArmies = new Optional<int>();
        MoveableFromTerritory = new Optional<Territory>();
    }

    public TerritoryDisplayCriteria(Continent mustBeIn = null, Continent mustNotBeIn = null, Player ownedBy = null, Player notOwnedBy = null, Territory mustBeMoveableFrom = null, int defendingArmiesAtleast = int.MinValue, int defendingArmiesAtMost = int.MaxValue, bool allowInvade = false)
    {
        MustBeInContinent = new Optional<Continent>(mustBeIn);
        MustNOTBeInContinent = new Optional<Continent>(mustNotBeIn);
        MustBeOwnedBy = new Optional<Player>(ownedBy);
        MustNOTBeOwnedBy = new Optional<Player>(notOwnedBy);
        MustHaveAtleastArmies = new Optional<int>(defendingArmiesAtleast);
        MustHaveNoMoreThanArmies = new Optional<int>(defendingArmiesAtMost);
        MoveableFromTerritory = new Optional<Territory>(mustBeMoveableFrom);
        AllowInvasion = allowInvade;
    }

    public bool DoesSatisfy(Territory territory)
    {
        if(MustBeInContinent.HasValue)
        {
            if (territory.Continent.Name != MustBeInContinent.Value.Name)
                return false;
        }
        if(MustNOTBeInContinent.HasValue)
        {
            if (territory.Continent.Name == MustNOTBeInContinent.Value.Name)
                return false;
        }
        if(MustBeOwnedBy.HasValue)
        {
            if (territory.Owner.Name != MustBeOwnedBy.Value.Name)
                return false;
        }
        if(MustNOTBeOwnedBy.HasValue)
        {
            if (territory.Owner.Name == MustNOTBeOwnedBy.Value.Name)
                return false;
        }
        if(MustHaveAtleastArmies.HasValue && MustHaveAtleastArmies.Value > 0)
        {
            if (territory.DefendingArmies.Count <= MustHaveAtleastArmies.Value)
                return false;
        }
        if(MustHaveNoMoreThanArmies.HasValue && MustHaveNoMoreThanArmies.Value > 0)
        {
            if (territory.DefendingArmies.Count > MustHaveNoMoreThanArmies.Value)
                return false;
        }
        if(MoveableFromTerritory.HasValue)
        {
            var t = MoveableFromTerritory.Value;
            if(Territory.AttemptOrFailToMove(t.Owner, t, territory, AllowInvasion))
            {
                // success
            } else
            { // unable to move there.
                return false;
            }
        }
        return true;
    }


}
