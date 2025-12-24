using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sem3laba3.Cards.Creatures;

namespace sem3laba3
{
    public class KnightDamageStrategy : IDamageStrategy
    {
        public int CalculateDamage(IBattleable attacker, IBattleable defender)
        {
            if(defender is Assassin)
            {
                return attacker.GetDamage() + 2;
            }
            return attacker.GetDamage();
        }
    }

    public class AssassinDamageStrategy : IDamageStrategy
    {
        public int CalculateDamage(IBattleable attacker, IBattleable defender)
        {
            if(defender is Wizard) { return  attacker.GetDamage() + 2; }
            return attacker.GetDamage();
        }
    }

    public class WizardDamageStrategy : IDamageStrategy
    {
        public int CalculateDamage(IBattleable attacker, IBattleable defender)
        {
            if (defender is Knight) { return attacker.GetDamage() + 2; }
            return attacker.GetDamage();
        }
    }
}
