using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Learning;
using SFML.Window;

namespace Tags
{
    internal class Program : Game
    {
        /// <summary>
        /// Игровое поле.
        /// </summary>
        private static int[] _tags = new int[16];
        
        /// <summary>
        /// Местонахождение пустой клетки на поле.
        /// </summary>
        private static int _zeroPos;

        /// <summary>
        /// Массив цветов выделенных надписей - всегда красный за исключением того, когда обычный текст красный.
        /// </summary>
        private static Color[] _highlightColors = { Color.Blue, Color.Red, Color.Red, Color.Red, Color.Red, Color.Red, Color.Red, Color.Red };

        /// <summary>
        /// Массив цветов обычных надписей и пятнашек.
        /// </summary>
        private static Color[] _normalColors = { Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.Cyan, Color.Magenta, Color.White };

        /// <summary>
        /// Массив спрайтов главного меню.
        /// </summary>
        private static string[] _mainMenuSprites;

        /// <summary>
        /// Массив спрайтов пятнашек.
        /// </summary>
        private static string[] _tagsSprites;

        /// <summary>
        /// Игровая музыка.
        /// </summary>
        private static string _music;

        /// <summary>
        /// Проигрывать музыку.
        /// </summary>
        private static bool _play;

        /// <summary>
        /// Массив кадров пятнашек внутри спрайтов пятнашек.
        /// </summary>
        private static Vector2Int[] _tagsFrames =
        {
            new Vector2Int(450, 450),
            new Vector2Int(0, 0),
            new Vector2Int(150, 0),
            new Vector2Int(300, 0),
            new Vector2Int(450, 0),
            new Vector2Int(0, 150),
            new Vector2Int(150, 150),
            new Vector2Int(300, 150),
            new Vector2Int(450, 150),
            new Vector2Int(0, 300),
            new Vector2Int(150, 300),
            new Vector2Int(300, 300),
            new Vector2Int(450, 300),
            new Vector2Int(0, 450),
            new Vector2Int(150, 450),
            new Vector2Int(300, 450)
        };

        /// <summary>
        /// Индекс текущего цвета, отбражаемого на экране.
        /// </summary>
        private static int _colorindex;
        
        /// <summary>
        /// Состояние игры.
        /// </summary>
        private static GameState _state;

        /// <summary>
        /// Режим игры.
        /// </summary>
        private static GameMode _mode;

        /// <summary>
        /// Очки игрока.
        /// </summary>
        private static int _score;

        /// <summary>
        /// Время игрока.
        /// </summary>
        private static double _time;

        /// <summary>
        /// Рекорд.
        /// </summary>
        private static int _record;
        
        /// <summary>
        /// Игровой бог.
        /// </summary>
        private static Random _random;

        /// <summary>
        /// Игра.
        /// </summary>
        private static bool _game;

        static void Main(string[] args)
        {
            //молитва игровому богу
            _random = new Random((int)(DateTime.Now.Ticks & 0xFFFFFFFF));

            //инициализировать окна игры
            InitWindow(600, 660, "Пятнашки");

            LoadContent();

            //выбрать цвет
            _colorindex = _random.Next(_normalColors.Length);

            _state = GameState.MainMenu;

            _game = true;

            _play = true;

            PlayMusic(_music);

            while (_game)
            {
                //1. Расчёт.
                DispatchEvents();

                //включение и отключение музыки
                if (GetKeyDown(Keyboard.Key.M))
                {
                    if(_play)
                        StopMusic(_music);
                    else
                        PlayMusic(_music);
                    _play = !_play;
                }

                switch (_state)
                {
                    case GameState.MainMenu:
                        if (GetKeyDown(Keyboard.Key.Num1))
                        {
                            _mode = GameMode.Easy;
                            _time = 15 * 60;
                            _score = 0;
                            _state = GameState.Game;
                            NextLevel();
                        }
                        else if (GetKeyDown(Keyboard.Key.Num2))
                        {
                            _mode = GameMode.Normal;
                            _time = 10 * 60;
                            _score = 0;
                            _state = GameState.Game;
                            NextLevel();
                        }
                        else if (GetKeyDown(Keyboard.Key.Num3))
                        {
                            _mode = GameMode.Hard;
                            _time = 5 * 60;
                            _score = 0;
                            _state = GameState.Game;
                            NextLevel();
                        }
                        else if (GetKeyDown(Keyboard.Key.Escape))
                        {
                            _game = false;
                        }
                        break;
                    case GameState.Game:
                        _time -= DeltaTime;
                        if (_time <= 0 || GetKeyDown(Keyboard.Key.Escape))
                        {
                            //игра окончена - вернуться в главное меню
                            if (_record < _score) _record = _score;
                            _state = GameState.MainMenu;
                            NextColor();
                        }

                        if (GetKeyDown(Keyboard.Key.Left))
                        {
                            //сдвигание фишки влево, на свободную позицию
                            if (_zeroPos % 4 < 3)
                            {
                                _tags[_zeroPos] = _tags[_zeroPos + 1];
                                _tags[_zeroPos + 1] = 0;
                                _zeroPos++;
                                TestWin();
                            }
                        }
                        else if (GetKeyDown(Keyboard.Key.Down))
                        {
                            //сдвигание фишки вниз, на свободную позицию
                            if (_zeroPos / 4 > 0)
                            {
                                _tags[_zeroPos] = _tags[_zeroPos - 4];
                                _tags[_zeroPos - 4] = 0;
                                _zeroPos -= 4;
                                TestWin();
                            }
                        }
                        else if (GetKeyDown(Keyboard.Key.Right))
                        {
                            //сдвигание фишки вправо, на свободную позицию
                            if (_zeroPos % 4 > 0)
                            {
                                _tags[_zeroPos] = _tags[_zeroPos - 1];
                                _tags[_zeroPos - 1] = 0;
                                _zeroPos--;
                                TestWin();
                            }
                        }
                        else if (GetKeyDown(Keyboard.Key.Up))
                        {
                            //сдвигание фишки вверх, на свободную позицию
                            if (_zeroPos / 4 < 3)
                            {
                                _tags[_zeroPos] = _tags[_zeroPos + 4];
                                _tags[_zeroPos + 4] = 0;
                                _zeroPos += 4;
                                TestWin();
                            }
                        }

                        break;
                }

                //3. Отрисовка экрана игры
                ClearWindow(0, 0, 0);

                Draw();

                DisplayWindow();


                //5. Ожидание
                Delay(1);
            }
        }

        /// <summary>
        /// Загрузка контента.
        /// </summary>
        private static void LoadContent()
        {
            // Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.Cyan, Color.Magenta, Color.White 

            _mainMenuSprites = new string[7];
            _mainMenuSprites[0] = LoadTexture("content\\start-red.png");
            _mainMenuSprites[1] = LoadTexture("content\\start-green.png");
            _mainMenuSprites[2] = LoadTexture("content\\start-blue.png");
            _mainMenuSprites[3] = LoadTexture("content\\start-yellow.png");
            _mainMenuSprites[4] = LoadTexture("content\\start-cyan.png");
            _mainMenuSprites[5] = LoadTexture("content\\start-magenta.png");
            _mainMenuSprites[6] = LoadTexture("content\\start-white.png");

            _tagsSprites = new string[7];
            _tagsSprites[0] = LoadTexture("content\\15-red.png");
            _tagsSprites[1] = LoadTexture("content\\15-green.png");
            _tagsSprites[2] = LoadTexture("content\\15-blue.png");
            _tagsSprites[3] = LoadTexture("content\\15-yellow.png");
            _tagsSprites[4] = LoadTexture("content\\15-cyan.png");
            _tagsSprites[5] = LoadTexture("content\\15-magenta.png");
            _tagsSprites[6] = LoadTexture("content\\15-white.png");

            SetFont("content\\courbd.ttf");

            _music = LoadMusic("content\\music.wav");
        }

        /// <summary>
        /// Запуск следующего уровня игры.
        /// </summary>
        private static void NextLevel()
        {
            //перемешать пятнашки на поле
            List<int> tags = new List<int>();
            for (int i = 0; i < 16; i++) tags.Add(i);
            for (int i = 0; i < _tags.Length; i++)
            {
                int tagIndex = _random.Next(tags.Count);
                _tags[i] = tags[tagIndex];
                tags.RemoveAt(tagIndex);
            }
            for (int i = 0; i < _tags.Length; i++)
            {
                if (_tags[i] == 0)
                {
                    _zeroPos = i;
                    break;
                }
            }
            NextColor();
        }

        /// <summary>
        /// Выбрать следующий цвет игрового поля.
        /// </summary>
        private static void NextColor()
        {
            while (true)
            {
                int nextColor = _random.Next(_normalColors.Length);
                if (nextColor != _colorindex)
                {
                    _colorindex = nextColor;
                    break;
                }
            }
        }

        /// <summary>
        /// Проверка выигрыша в игре.
        /// </summary>
        private static void TestWin()
        {
            //должен быть игровой режим
            //пустое место должно быть в последней клетке поля
            if(_state != GameState.Game || _zeroPos != 15) return;

            //проверить последовательность пятнашек - всё должно быть рассортировано
            for (int i = 0, j = 1; i < 15; i++, j++)
            {
                if(_tags[i] != j) return;
            }

            //пятнашки выиграны
            //добавить очки за победу
            switch (_mode)
            {
                case GameMode.Easy:
                    _score++;
                    break;
                case GameMode.Normal:
                    _score += 2;
                    break;
                case GameMode.Hard:
                    _score += 5;
                    break;
            }
            //перейти к следующему уровню
            NextLevel();
        }

        /// <summary>
        /// Отрисовка экрана.
        /// </summary>
        private static void Draw()
        {
            switch (_state)
            {
                case GameState.MainMenu:
                    //нарисовать главное меню
                    DrawSprite(_mainMenuSprites[_colorindex], 0, 60);
                    SetFillColor(_normalColors[_colorindex]);
                    //нарисовать рекорд
                    DrawText(10, -3, "Рекорд", 48);
                    SetFillColor(_highlightColors[_colorindex]);
                    if (_record < 10)
                    {
                        DrawText(416 + 29 * 5, -3, _record.ToString(), 48);
                    }
                    else if (_record < 100)
                    {
                        DrawText(416 + 29 * 4, -3, _record.ToString(), 48);
                    }
                    else if (_record < 1000)
                    {
                        DrawText(416 + 29 * 3, -3, _record.ToString(), 48);
                    }
                    else if (_record < 10000)
                    {
                        DrawText(416 + 29 * 2, -3, _record.ToString(), 48);
                    }
                    else if (_record < 100000)
                    {
                        DrawText(416 + 29, -3, _record.ToString(), 48);
                    }
                    else
                    {
                        DrawText(416, -3, _record.ToString(), 48);
                    }
                    break;
                case GameState.Game:
                    //нарисовать время
                    SetFillColor(_highlightColors[_colorindex]);
                    TimeSpan time = TimeSpan.FromSeconds(_time);
                    DrawText(10, -3, time.ToString("mm\\:ss"), 48);
                    //нарисовать очки
                    if (_score < 10)
                    {
                        DrawText(416 + 29 * 5, -3, _score.ToString(), 48);
                    }
                    else if (_score < 100)
                    {
                        DrawText(416 + 29 * 4, -3, _score.ToString(), 48);
                    }
                    else if (_score < 1000)
                    {
                        DrawText(416 + 29 * 3, -3, _score.ToString(), 48);
                    }
                    else if (_score < 10000)
                    {
                        DrawText(416 + 29 * 2, -3, _score.ToString(), 48);
                    }
                    else if (_score < 100000)
                    {
                        DrawText(416 + 29, -3, _score.ToString(), 48);
                    }
                    else
                    {
                        DrawText(416, -3, _score.ToString(), 48);
                    }

                    //нарисовать игровое поле
                    int x = 0;
                    int y = 0;
                    foreach (int tag in _tags)
                    {
                        DrawSprite(_tagsSprites[_colorindex], x, y + 60, _tagsFrames[tag].X, _tagsFrames[tag].Y, 150, 150);
                        x += 150;
                        if (x > 599)
                        {
                            x = 0;
                            y += 150;
                        }
                    }
                    break;
            }
        }
    }
}
