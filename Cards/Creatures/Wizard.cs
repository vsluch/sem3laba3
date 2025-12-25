using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sem3laba3.Cards.Creatures
{
    public class Wizard : Creature
    {
        public Wizard() : 
            base(GameBalanceStats.Wizard.HP, GameBalanceStats.Wizard.Damage, new WizardDamageStrategy()) { }

        public override Creature Clone()
        {
            return new Wizard();
        }
    }
}
