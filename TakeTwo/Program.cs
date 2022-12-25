using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Learning;

namespace TakeTwo
{
    internal class Program : Game
    {
        /// <summary>
        /// Колоды для игры на экранах разного размера. Номер колоды совпадает с количеством карт по вертикали.
        /// </summary>
        private static Dictionary<int, Deck> _decks;

        static void Main(string[] args)
        {
            InitWindow(1024, 768, "Найди пару: Сальвадор Дали");

            SetFont(@"content\Gabriola.ttf");
            SetFillColor(Color.White);

            string sprite2 = LoadTexture(@"content\2\001.png");
            string sprite3 = LoadTexture(@"content\3\001.png");

            while (true)
            {
                //1. Расчёт.
                DispatchEvents();
                
                //2. Очистка буфера и окна.
                ClearWindow();

                SetFillColor(0, 255, 255);

                DrawText(10, 10, DeltaTime.ToString(CultureInfo.CurrentCulture), 48);

                DrawSprite(sprite2, 10, 30);
                DrawSprite(sprite3, 249, 30);

                DisplayWindow();

                //5. Ожидание
                Delay(1);
            }
        }

        private void LoadDecks()
        {
            //номер колоды совпадает с количеством карт по вертикали
            for (int i = 2; i < 7; i++)
            {
                Deck deck = new Deck();
                deck.CardShirt = LoadTexture($"content\\{i}\\cardshirt.png");
                for (int j = 1; j < 101; j++)
                {
                    
                }
            }
        }
    }
}
