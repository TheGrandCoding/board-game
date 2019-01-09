using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RandomGens
{
    public static class RndHelp
    {
        public static System.Random random = new System.Random();
        public static int Next(int min, int max)
        {
            return random.Next(min, max);
        }
        public static int Next()
        {
            return random.Next();
        }
        public static int Next(int max)
        {
            return random.Next(max);
        }
        public static T Choose<T>(params T[] paramItems)
        {
            List<T> items = new List<T>();
            foreach(var item in paramItems)
            {
                items.Add(item);
            }
            int index = Next(0, items.Count - 1);
            return items[index];
        }
        public static T Choose<T>(IEnumerable<T> items)
        {
            int index = Next(0, items.Count() - 1);
            return items.ElementAt(index);
        }
    }

    /// <summary>
    /// Class that holds numerous dice
    /// </summary>
    public static class Dice {
        /// <summary>
        /// Rolls a dice with n number of sides
        /// </summary>
        /// <param name="n">Number of sides of the dice</param>
        /// <returns>Random number between 1 and n (excl)</returns>
        public static int RollDiceOfSides(int n)
        {
            return RndHelp.Next(1, n+1);
        }
        /// <summary>
        /// Rolls a standard six sided dice
        /// </summary>
        public static int RollSixSided()
        {
            return RollDiceOfSides(6);
        }
        /// <summary>
        /// Rolls a standard eight sided dice (1 to 8 incl)
        /// </summary>
        public static int RollEightSided()
        {
            return RollDiceOfSides(8);
        }
        /// <summary>
        /// Rolls a six sided dice that is weighted/unfair
        /// </summary>
        /// <returns></returns>
        public static int RollWeightedFourSidedDice()
        {
            return RndHelp.Choose(1, 2, 3, 3, 4, 4);
        }
    }

    public static class ArmyTypes
    {
        public static ArmyType ThrowDice()
        {
            var dice = Dice.RollSixSided();
            switch (dice)
            {
                case 1:
                case 2:
                    return ArmyType.Infantry;
                case 3:
                case 4:
                    return ArmyType.Cavalry;
                case 5:
                case 6:
                    return ArmyType.Artillery;
                default:
                    return ArmyType.NotSet;
            }
        }
    }
}
