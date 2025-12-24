using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sem3laba3
{
    public interface IDamageStrategy
    {
        public int CalculateDamage(IBattleable attacker, IBattleable defender);
    }
}
