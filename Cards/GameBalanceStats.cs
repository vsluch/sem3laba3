using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sem3laba3.Cards
{
    public class GameBalanceStats
    {
        public static class Knight
        {
            public const int HP = 16;
            public const int Damage = 4;
        }

        public static class Assassin
        {
            public const int HP = 8;
            public const int Damage = 6;
        }

        public static class Wizard
        {
            public const int HP = 8;
            public const int Damage = 5;
        }


        public static class Fireball
        {
            public const int Strenght = 3;
        }
        public static class Booster
        {
            public const int Strenght = 2;
        }
        public static class HeallSpell
        {
            public const int Strenght = 4;
        }

        public static class Market
        {
            public const int MaxHandPower = 100;
        }
    }
}
