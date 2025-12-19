using lab3;
using System;

namespace laba3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Начало игры!");

            Player p1 = new Player("Игрок 1");
            Player p2 = new Player("Игрок 2");

            // Раздача стартовых рук
            for (int i = 0; i < 3; i++) { p1.DrawCard(); p2.DrawCard(); }

            bool gameOn = true;
            while (gameOn)
            {
                // Ход игрока 1
                p1.MakeMove(p2.Army);
                // Проверка условий победы (если бы атаковали героя)

                System.Threading.Thread.Sleep(1000); // Пауза для читаемости

                // Ход игрока 2
                p2.MakeMove(p1.Army);

                Console.WriteLine("\nНажмите Enter для следующего раунда...");
                Console.ReadLine();
            }
        }
    }
}
