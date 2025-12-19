using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3
{
    string Name { get; }
    public interface IBattleable
    {
        // Свойства, которые обязан иметь боевой объект
        int HP { get; }            // Текущее здоровье (только для чтения извне)
        bool IsDead { get; }       // Жив или мертв

        // Методы воздействия НА объект
        void TakeDamage(int damage);       // Получить урон
        void GetHeal(int added_hp);        // Лечение

        // Методы воздействия ОТ объекта
        void IncreaseDamage(int added_damage); // Увеличить свою атаку (для Booster)

        // Атаковать кого-то (реализует IBattleable)
        void Hit(IBattleable target);
    }
}
