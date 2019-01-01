using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public static class Dice {
        public static int Roll()
        {
            return RndHelp.Next(1, 7);
        }
    }

    public static class ArmyTypes
    {
        public static ArmyType ThrowDice()
        {
            var dice = Dice.Roll();
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
