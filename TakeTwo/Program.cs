using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Learning;
using SFML.Window;

namespace TakeTwo
{
    internal class Program : Game
    {
        /// <summary>
        /// Главное меню игры.
        /// </summary>
        private static string _mainMenu;

        /// <summary>
        /// Экран завершения игры.
        /// </summary>
        private static string _gameOver;

        /// <summary>
        /// Экран завершения игры с рекордом.
        /// </summary>
        private static string _gameOverRecord;
        
        /// <summary>
        /// Колоды для игры на экранах разного размера. Номер колоды совпадает с количеством карт по вертикали.
        /// </summary>
        private static Dictionary<int, Deck> _decks;

        /// <summary>
        /// Задники игровых экранов.
        /// </summary>
        private static List<string> _backgrounds;

        /// <summary>
        /// Задник текущего игрового уровня.
        /// </summary>
        private static string _currentBackground = string.Empty;

        /// <summary>
        /// Спрайт с правилами игры.
        /// </summary>
        private static string _howToPlay;

        /// <summary>
        /// Состояние игры.
        /// </summary>
        private static GameState _state;

        /// <summary>
        /// Режим игры.
        /// </summary>
        private static GameMode _mode;

        /// <summary>
        /// Время, на которое открываются карты в зависимости от сложности игры, с.
        /// </summary>
        private static double[] _showTimes = { 1, .5, .25 };

        /// <summary>
        /// Время, данное на прохождение уровня в зависимости от сложности игры, с.
        /// </summary>
        private static double[] _levelTimes = { 720, 360, 180 };

        /// <summary>
        /// Спрайт "Время" на игровом экране.
        /// </summary>
        private static string _gameTextTime;

        /// <summary>
        /// Спрайт "Уровень" на игровом экране.
        /// </summary>
        private static string _gameTextLevel;

        /// <summary>
        /// Спрайт "Очки" на игровом экране.
        /// </summary>
        private static string _gameTextScore;

        /// <summary>
        /// Спрайт "Уровень пройден!" на игровом экране.
        /// </summary>
        private static string _gameTextLevelComplete;

        /// <summary>
        /// Спрайт "Ваш результат" в игровом меню.
        /// </summary>
        private static string _menuTextScore;

        /// <summary>
        /// Спрайт "Рекорд" в игровом меню.
        /// </summary>
        private static string _menuTextRecord;

        /// <summary>
        /// Игровая музыка.
        /// </summary>
        private static List<string> _music;

        /// <summary>
        /// Текущий проигрываемый музыкальный трек.
        /// </summary>
        private static string _currentTrack = string.Empty;

        /// <summary>
        /// Проигрывать ли музыку.
        /// </summary>
        private static bool _playMusic = true;

        /// <summary>
        /// Размеры игровых уровней.
        /// </summary>
        private static Vector2Int[] _levels =
        {
            new Vector2Int(2,2),
            new Vector2Int(3,2),
            new Vector2Int(4,2),
            new Vector2Int(4,3),
            new Vector2Int(5,3),
            new Vector2Int(6,3),
            new Vector2Int(5,4),
            new Vector2Int(6,4),
            new Vector2Int(7,4),
            new Vector2Int(8,4),
            new Vector2Int(7,5),
            new Vector2Int(8,5),
            new Vector2Int(9,5),
            new Vector2Int(10,5),
            new Vector2Int(9,6),
            new Vector2Int(10,6),
            new Vector2Int(11,6),
            new Vector2Int(12,6)
        };

        /// <summary>
        /// Текущий игровой уровень.
        /// </summary>
        private static int _level;

        /// <summary>
        /// Карты текущего игрового уровня.
        /// </summary>
        private static List<Card> _cards;

        /// <summary>
        /// Размер карты текущего игрового уровня.
        /// </summary>
        private static Vector2Int _cardSize;

        /// <summary>
        /// Рубашка текущего игрового уровня.
        /// </summary>
        private static string _cardShirt;

        /// <summary>
        /// Очки игрока.
        /// </summary>
        private static int _score;

        /// <summary>
        /// Время игрока.
        /// </summary>
        private static double _time;

        /// <summary>
        /// Время приостановки игры по различным причинам.
        /// </summary>
        private static double _pauseTime;

        /// <summary>
        /// Рекорд.
        /// </summary>
        private static int _record;

        /// <summary>
        /// Игровой бог.
        /// </summary>
        private static Random _random;

        /// <summary>
        /// Открытые игроком карты.
        /// </summary>
        private static List<Card> _openedCards;

        /// <summary>
        /// Игра.
        /// </summary>
        private static bool _game;

        /// <summary>
        /// Точка входа в программу.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //молитва игровому богу
            _random = new Random((int)(DateTime.Now.Ticks & 0xFFFFFFFF));

            //инициализировать окна игры
            InitWindow(1024, 768, "Найди пару: Сальвадор Дали");
            SetFillColor(Color.Black);

            //загрузить контент игры
            LoadContent();

            _state = GameState.MainMenu;

            _game = true;

            PlayMusic();

            while (_game)
            {
                //1. Расчёт.
                DispatchEvents();

                //выход из игры в главном меню игры
                if (GetKeyDown(Keyboard.Key.Escape) && _state == GameState.MainMenu) _game = false;

                //переключение проигрывания музыки
                if (GetKeyDown(Keyboard.Key.M))
                {
                    _playMusic = !_playMusic;
                    PlayMusic();
                }

                //игровая логика
                switch (_state)
                {
                    case GameState.MainMenu:
                        if (GetMouseButtonDown(Mouse.Button.Left))
                        {
                            if (MouseX > 718 && MouseX < 835 && MouseY > 458 && MouseY < 495)
                            {
                                //запуск лёгкого режима игры
                                _mode = GameMode.Easy;
                                _level = 0;
                                _score = 0;
                                LevelInit();
                            }
                            else if (MouseX > 662 && MouseX < 894 && MouseY > 516 && MouseY < 557)
                            {
                                //запуск нормального режима игры
                                _mode = GameMode.Normal;
                                _level = 0;
                                _score = 0;
                                LevelInit();
                            }
                            else if (MouseX > 660 && MouseX < 856 && MouseY > 634 && MouseY < 614)
                            {
                                //запуск сложного режима игры
                                _mode = GameMode.Hard;
                                _level = 0;
                                _score = 0;
                                LevelInit();
                            }
                            else if (MouseX > 702 && MouseX < 898 && MouseY > 612 && MouseY < 672)
                            {
                                //открыть справку по игре
                                _state = GameState.Help;
                                //выбрать задник для текста справки
                                while (true)
                                {
                                    string newBackground = _backgrounds[_random.Next(_backgrounds.Count)];
                                    if (_currentBackground == newBackground) continue;
                                    _currentBackground = newBackground;
                                    break;
                                }
                            }
                            else if (MouseX > 715 && MouseX < 846 && MouseY > 693 && MouseY < 733)
                            {
                                //выход из игры
                                _game = false;
                            }
                        }
                        break;
                    case GameState.Game:
                        //уменьшить время игрока
                        _time -= DeltaTime;
                        //время вышло, либо игрок решил прервать игру
                        if (_time <= 0 || GetKeyDown(Keyboard.Key.Escape))
                        {
                            //игра окончена
                            if (_score > _record)
                            {
                                _record = _score;
                                _state = GameState.GameOverRecord;
                            }
                            else
                            {
                                _state = GameState.GameOver;
                            }
                        }
                        else
                        {
                            //игра продолжается
                            //проверить не нажата ли мышь на карте
                            if (GetMouseButtonDown(Mouse.Button.Left))
                            {
                                for (int i = 0; i < _cards.Count; i++)
                                {
                                    if (MouseX > _cards[i].Position.X && MouseX < _cards[i].Position.X + _cardSize.X &&
                                        MouseY > _cards[i].Position.Y && MouseY < _cards[i].Position.Y + _cardSize.Y &&
                                        !_cards[i].IsOpen)
                                    {
                                        //мышь нажата на карте и карта не открыта - открыть карту
                                        _cards[i].IsOpen = true;
                                        _openedCards.Add(_cards[i]);
                                    }
                                }
                                //если открыто две карты - перейти в режим показа карт
                                if (_openedCards.Count >= 2)
                                {
                                    _pauseTime = _showTimes[(int)_mode];
                                    _state = GameState.ShowCards;
                                }
                            }
                        }
                        break;
                    case GameState.LevelStart:
                        //проверить, не хочет ли игрок выйти из игры
                        if (GetKeyDown(Keyboard.Key.Escape))
                        {
                            //игра окончена
                            if (_score > _record)
                            {
                                _record = _score;
                                _state = GameState.GameOverRecord;
                            }
                            else
                            {
                                _state = GameState.GameOver;
                            }
                        }
                        //ожидание конца паузы перед показом всех карт уровня
                        _pauseTime -= DeltaTime;
                        if (_pauseTime <= 0)
                        {
                            //вскрыть все карты и перейти в режим показа карт
                            foreach (Card card in _cards) card.IsOpen = true;
                            _pauseTime = _showTimes[(int)_mode];
                            _state = GameState.ShowCards;
                        }
                        break;
                    case GameState.ShowCards:
                        //проверить, не хочет ли игрок выйти из игры
                        if (GetKeyDown(Keyboard.Key.Escape))
                        {
                            //игра окончена
                            if (_score > _record)
                            {
                                _record = _score;
                                _state = GameState.GameOverRecord;
                            }
                            else
                            {
                                _state = GameState.GameOver;
                            }
                        }
                        //ожидание конца показа карт
                        _pauseTime -= DeltaTime;
                        if (_pauseTime <= 0)
                        {
                            //если открытые карты совпадают - удалить их из списка карт и засчитать очки игроку
                            if (_openedCards.Count == 2 && _openedCards[0].Sprite == _openedCards[1].Sprite)
                            {
                                _cards.Remove(_openedCards[0]);
                                _cards.Remove(_openedCards[1]);

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
                            }

                            //закрыть все карты
                            foreach (Card card in _cards) card.IsOpen = false;

                            //очистить список открытых карт
                            _openedCards.Clear();

                            if (_cards.Count == 0)
                            {
                                //карт не осталось - уровень пройден
                                _pauseTime = 3;
                                _state = GameState.LevelComplete;
                            }
                            else
                            {
                                //карты ещё есть - перейти в режим игры для дальнейшего прохождения уровня
                                _state = GameState.Game;
                            }
                        }
                        break;
                    case GameState.LevelComplete:
                        //проверить, не хочет ли игрок выйти из игры
                        if (GetKeyDown(Keyboard.Key.Escape))
                        {
                            //игра окончена
                            if (_score > _record)
                            {
                                _record = _score;
                                _state = GameState.GameOverRecord;
                            }
                            else
                            {
                                _state = GameState.GameOver;
                            }
                        }
                        //ожидание конца показа сообщения о том, что уровень пройден
                        _pauseTime -= DeltaTime;
                        if (_pauseTime <= 0)
                        {
                            _level++;
                            LevelInit();
                        }
                        break;
                    case GameState.GameOver:
                    case GameState.GameOverRecord:
                    case GameState.Help:
                        if (GetKeyDown(Keyboard.Key.Escape) || GetKeyDown(Keyboard.Key.Return) || GetKeyDown(Keyboard.Key.Space) || GetMouseButtonDown(Mouse.Button.Left))
                        {
                            if(_state != GameState.Help) PlayMusic();
                            _state = GameState.MainMenu;
                        }
                        break;
                }

                //2. Очистка буфера и окна.
                ClearWindow(128, 128, 128);

                //3. Отрисовка экрана игры
                Draw();

                DisplayWindow();


                //5. Ожидание
                Delay(1);
            }
        }

        /// <summary>
        /// Загрузка контента при старте игры.
        /// </summary>
        private static void LoadContent()
        {
            //загрузить неигровые экраны
            _mainMenu = LoadTexture("content\\mianmenu.png");
            _gameOver = LoadTexture("content\\gameover1.png");
            _gameOverRecord = LoadTexture("content\\gameover2.png");

            //загрузить спрайты надписей
            _gameTextTime = LoadTexture("content\\time.png");
            _gameTextLevel = LoadTexture("content\\level.png");
            _gameTextScore = LoadTexture("content\\score.png");
            _gameTextLevelComplete = LoadTexture("content\\levelcomplete.png");
            _menuTextScore = LoadTexture("content\\menuscore.png");
            _menuTextRecord = LoadTexture("content\\menurecord.png");
            _howToPlay = LoadTexture("content\\howtoplay.png");

            //загрузить колоды игровых карт
            //номер колоды совпадает с количеством карт по вертикали
            _decks = new Dictionary<int, Deck>();
            for (int i = 2; i < 7; i++)
            {
                //создать колоду и загрузить рубашку
                Deck deck = new Deck { CardShirt = LoadTexture($"content\\{i}\\cardshirt.png") };
                //задать размер карты в колоде
                switch (i)
                {
                    case 2:
                        deck.CardSize = new Vector2Int(229, 345);
                        break;
                    case 3:
                        deck.CardSize = new Vector2Int(150, 226);
                        break;
                    case 4:
                        deck.CardSize = new Vector2Int(111, 167);
                        break;
                    case 5:
                        deck.CardSize = new Vector2Int(88, 132);
                        break;
                    case 6:
                        deck.CardSize = new Vector2Int(72, 108);
                        break;
                }
                //загрузить карты - в каждой колоде 100 карт - от 001.png до 100.png
                for (int j = 1; j < 101; j++) deck.Cards.Add(LoadTexture($"content\\{i}\\{j:000}.png"));
                _decks.Add(i, deck);
            }

            //загрузить задники игровых экранов - 10 задников от back01.jpg до back10.jpg
            _backgrounds = new List<string>();
            for (int i = 1; i < 11; i++) _backgrounds.Add(LoadTexture($"content\\back{i:00}.jpg"));
            
            //загрузка шрифта
            SetFont(@"content\Gabriola.ttf");

            //загрузка музыки
            _music = new List<string>
            {
                LoadMusic(@"content\1.wav"),
                LoadMusic(@"content\2.wav"),
                LoadMusic(@"content\3.wav"),
                LoadMusic(@"content\4.wav"),
                LoadMusic(@"content\5.wav")
            };
        }

        /// <summary>
        /// Инициализация уровня.
        /// </summary>
        private static void LevelInit()
        {
            //в начале уровня все карты открыты
            _state = GameState.ShowCards;
            _time = _levelTimes[(int)_mode];
            _pauseTime = _showTimes[(int)_mode];
            _openedCards = new List<Card>();
            //вычислить номер уровня - последний уровень повторяется пока не надоест игроку, либо пока игрок не проиграет
            int level = _level < _levels.Length ? _level : _levels.Length - 1;
            //создать список карт поля
            _cards = new List<Card>();
            //номер используемой колоды совпадает с числом карт по вертикали
            int deck = _levels[level].Y;
            //сохранить размер и рубашку карты текущего уровня
            _cardSize = _decks[deck].CardSize;
            _cardShirt = _decks[deck].CardShirt;
            //рассчитать смещение по вертикали и по горизонтали при рисовании карт на столе
            //Поля сверху-сбоку-снизу 58-12-10
            //Расстояния между картами - 10 пикселей
            Vector2Int offset = new Vector2Int(
                (1000 - (_cardSize.X * _levels[level].X + 10 * (_levels[level].X - 1))) / 2 + 12,
                (700 - (_cardSize.Y * _levels[level].Y + 10 * (_levels[level].Y - 1))) / 2 + 58
            );
            //вычислить позицию неиспользуемой ячейки в центре - только для уровней с нечётным количеством карт
            Vector2Int middle = _levels[level].X % 2 > 0 && _levels[level].Y % 2 > 0
                ? new Vector2Int(_levels[level].X / 2, _levels[level].Y / 2)
                : new Vector2Int(-1, -1);
            
            //разместить каррты на поле
            //список номеров карт в колоде
            List<int> deckNumbers = new List<int>();
            //список позиций на поле
            List<Vector2Int> positions = new List<Vector2Int>();
            //заполнить список номеров карт в колоде чтобы брать из неё по одной новой карте за раз
            for (int i = 0; i < 100; i++) deckNumbers.Add(i);
            //заполнить списки позиций карт и карт для размещения на поле
            for (int j = 0, addCounter = 0, cardIndex = _random.Next(deckNumbers.Count); j < _levels[level].Y; j++)
            {
                for (int i = 0; i < _levels[level].X; i++)
                {
                    //пропуск средней ячейки
                    if(middle.X != -1 && middle.X == i && middle.Y == j) continue;

                    //добавить позицию карты
                    positions.Add(new Vector2Int(i, j));

                    //добавить карту для размещения на поле - в список добавляется по две одинаковых карты
                    if (addCounter == 2)
                    {
                        //эта карта уже была дважды добавлена - удалить её номер из списка номеров карт в колоде и выбрать новую
                        deckNumbers.RemoveAt(cardIndex);
                        cardIndex = _random.Next(deckNumbers.Count);
                        addCounter = 0;
                    }
                    _cards.Add(new Card { Sprite = _decks[deck].Cards[deckNumbers[cardIndex]], IsOpen = true });
                    addCounter++;
                }
            }
            //скопировать случайным образом координаты карт в карты на игровом поле
            Vector2Int posSize = _cardSize + new Vector2Int(10, 10);
            foreach (Card card in _cards)
            {
                //настроить положение карты на поле
                int posIndex = _random.Next(positions.Count);
                card.Position = positions[posIndex] * posSize + offset; //new Vector2Int(positions[posIndex].X * (_cardSize.X + 10) + offset.X, positions[posIndex].Y * (_cardSize.Y + 10) + offset.Y);
                //удалить использованную координату из списка
                positions.RemoveAt(posIndex);
            }

            //выбрать задник для нового уровня
            while (true)
            {
                string newBackground = _backgrounds[_random.Next(_backgrounds.Count)];
                if (_currentBackground == newBackground) continue;
                _currentBackground = newBackground;
                break;
            }

            //на каждом уровне новая музыкальная композиция
            PlayMusic();
        }

        /// <summary>
        /// Запуск проигрывания музыки.
        /// </summary>
        private static void PlayMusic()
        {
            //остановить проигрывание текущего трека
            if(_currentTrack != string.Empty) StopMusic(_currentTrack);

            //если проигрывание музыки запрещено - завершить работу
            if(!_playMusic)
            {
                _currentTrack = string.Empty;
                return;
            }
            
            //запустить случайный трек на проигрывание
            while (true)
            {
                string newTrack = _music[_random.Next(_music.Count)];
                if (_currentTrack == newTrack) continue;
                _currentTrack = newTrack;
                break;
            }
            PlayMusic(_currentTrack);
        }

        /// <summary>
        /// Отрисовка игрового экрана.
        /// </summary>
        private static void Draw()
        {
            switch (_state)
            {
                case GameState.MainMenu:
                    //главное меню
                    DrawSprite(_mainMenu, 0, 0);

                    //результат последней игры
                    //по цифрам см. "Наработки.txt, раздел ОТРИСОВКА ЦИФР НА СТАРТОВОМ ЭКРАНЕ"
                    {
                        int offset;
                        if (_score > 999)
                        {
                            offset = 0;
                        }
                        else if (_score > 99)
                        {
                            offset = 13 / 2;
                        }
                        else if (_score > 9)
                        {
                            offset = 13;
                        }
                        else
                        {
                            offset = 13 * 3 / 2;
                        }
                        DrawSprite(_menuTextScore, 663 + offset, 339);
                        DrawText(843 + offset, 326, _score.ToString(), 33);
                    }

                    //рекорд
                    //по цифрам см. "Наработки.txt, раздел ОТРИСОВКА ЦИФР НА СТАРТОВОМ ЭКРАНЕ"
                    {
                        int offset;
                        if (_record > 999)
                        {
                            offset = 0;
                        }
                        else if (_record > 99)
                        {
                            offset = 13 / 2;
                        }
                        else if (_record > 9)
                        {
                            offset = 13;
                        }
                        else
                        {
                            offset = 13 * 3 / 2;
                        }
                        DrawSprite(_menuTextRecord, 710 + offset, 383);
                        DrawText(793 + offset, 370, _record.ToString(), 33);
                    }
                    break;
                case GameState.Help:
                    DrawSprite(_currentBackground, 0, 0);
                    DrawSprite(_howToPlay, 0,  0);
                    break;
                case GameState.Game:
                case GameState.LevelStart:
                case GameState.LevelComplete:
                case GameState.ShowCards:
                    //игровой экран
                    //нарисовать задник
                    DrawSprite(_currentBackground, 0, 0);
                    //нарисовать карты
                    foreach (Card card in _cards) DrawSprite(card.IsOpen ? card.Sprite : _cardShirt, card.Position.X, card.Position.Y);
                    
                    //нарисовать остаток времени
                    //по цифрам см. "Наработки.txt, раздел ОТРИСОВКА НАДПИСЕЙ НА ИГРОВОМ ЭКРАНЕ"
                    DrawSprite(_gameTextTime, 12, 0);
                    TimeSpan currentTime = TimeSpan.FromSeconds(_time);
                    DrawText(12 + 97 + 9, -9, _time >= 3600 ? currentTime.ToString("hh\\:mm\\:ss") : currentTime.ToString("mm\\:ss"), 47);

                    //нарисровать текущий уровень
                    //по цифрам см. "Наработки.txt, раздел ОТРИСОВКА НАДПИСЕЙ НА ИГРОВОМ ЭКРАНЕ"
                    int offsetX;
                    if (_level > 99)
                    {
                        offsetX = (1024 - (127 + 65)) / 2;
                    }
                    else if(_level > 9)
                    {
                        offsetX = (1024 - (127 + 46)) / 2;
                    }
                    else
                    {
                        offsetX = (1024 - (127 + 27)) / 2;
                    }
                    DrawSprite(_gameTextLevel, offsetX, 0);
                    //внутриигровой подсчёт уровней начинается с нуля, поэтому при выводе на экран увеличиваем на 1
                    DrawText(offsetX + 127 + 9, -9, (_level+1).ToString(), 47);

                    //нарисовать текущие очки
                    //по цифрам см. "Наработки.txt, раздел ОТРИСОВКА НАДПИСЕЙ НА ИГРОВОМ ЭКРАНЕ"
                    if (_score > 999)
                    {
                        offsetX = 1024 - 82 - 84 - 12;
                    }
                    else if (_score > 99)
                    {
                        offsetX = 1024 - 82 - 65 - 12;
                    }
                    else if(_score > 9)
                    {
                        offsetX = 1024 - 82 - 46 - 12;
                    }
                    else
                    {
                        offsetX = 1024 - 82 - 27 - 12;
                    }
                    DrawSprite(_gameTextScore, offsetX, 0);
                    DrawText(offsetX + 82 + 9, -9, _score.ToString(), 47);

                    if(_state == GameState.LevelComplete) DrawSprite(_gameTextLevelComplete, 238, 368);

                    break;
                case GameState.GameOver:
                    //конец игры
                    DrawSprite(_gameOver, 0, 0);

                    //результат последней игры
                    //по цифрам см. "Наработки.txt, раздел ОТРИСОВКА ЦИФР НА ЭКРАНЕ КОНЦА ИГРЫ"
                    {
                        int offset;
                        if (_score > 999)
                        {
                            offset = 0;
                        }
                        else if (_score > 99)
                        {
                            offset = 13 / 2;
                        }
                        else if (_score > 9)
                        {
                            offset = 13;
                        }
                        else
                        {
                            offset = 13 * 3 / 2;
                        }
                        DrawSprite(_menuTextScore, 659 + offset, 579);
                        DrawText(839 + offset, 566, _score.ToString(), 33);
                    }
                    break;
                case GameState.GameOverRecord:
                    DrawSprite(_gameOverRecord, 0, 0);

                    //результат последней игры
                    //по цифрам см. "Наработки.txt, раздел ОТРИСОВКА ЦИФР НА ЭКРАНЕ КОНЦА ИГРЫ С РЕКОРДОМ"
                    {
                        int offset;
                        if (_score > 999)
                        {
                            offset = 0;
                        }
                        else if (_score > 99)
                        {
                            offset = 13 / 2;
                        }
                        else if (_score > 9)
                        {
                            offset = 13;
                        }
                        else
                        {
                            offset = 13 * 3 / 2;
                        }
                        DrawSprite(_menuTextScore, 661 + offset, 520);
                        DrawText(841 + offset, 507, _score.ToString(), 33);
                    }
                    break;
            }
        }
    }
}
