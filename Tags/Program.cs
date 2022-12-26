using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Learning;
using SFML.Window;

namespace Tags
{
    internal class Program : Game
    {
        static void Main(string[] args)
        {
            //инициализировать окна игры
            InitWindow(1024, 768, "Пятнашки");
            SetFillColor(Color.Magenta);

            string sprite = LoadTexture("testMainMenu.png");
            string scoreSprite = LoadTexture("menuscore.png");
            string recordSprite = LoadTexture("menurecord.png");
            SetFont("Gabriola.ttf");

            int x = 844;
            int y = 566;
            uint size = 33;

            while (true)
            {
                //1. Расчёт.
                DispatchEvents();

                //2. Очистка буфера и окна.
                ClearWindow(192, 192, 192);

                if (GetKeyDown(Keyboard.Key.Left)) x--;
                if (GetKeyDown(Keyboard.Key.Right)) x++;
                if (GetKeyDown(Keyboard.Key.Up)) y--;
                if (GetKeyDown(Keyboard.Key.Down)) y++;
                if (GetKeyDown(Keyboard.Key.Add)) size++;
                if (GetKeyDown(Keyboard.Key.Subtract)) size--;

                //3. Отрисовка экрана игры
                DrawSprite(sprite, 0, 0);

                DrawText(10, 0, $"x = {x}", 24);
                DrawText(10, 20, $"y = {y}",24);
                DrawText(10, 40, $"size = {size}", 24);

                DrawText(x, y, "9999", size);
                //DrawSprite(scoreSprite, x, y);

                DisplayWindow();


                //5. Ожидание
                Delay(1);
            }
        }
    }
}
