using sem3laba3.Cards.Creatures;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sem3laba3
{
    public interface IBattleable
    {
        void TakeDamage(int damage);
        void GetHeal(int added_hp);
        void IncreaseDamage(int added_damage);
        void Hit(IBattleable target);


        int GetDamage();    // для DamageStrategy
    }
}
