using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeTwo
{
    internal enum GameState
    {
        /// <summary>
        /// Начальный экран игры с главным меню.
        /// </summary>
        MainMenu,
        /// <summary>
        /// Экран справки.
        /// </summary>
        Help,
        /// <summary>
        /// Игра.
        /// </summary>
        Game,
        /// <summary>
        /// Начало уровня.
        /// </summary>
        LevelStart,
        /// <summary>
        /// Уровень пройден.
        /// </summary>
        LevelComplete,
        /// <summary>
        /// Игра показывает карты игроку.
        /// </summary>
        ShowCards,
        /// <summary>
        /// Конец игры, стандартный.
        /// </summary>
        GameOver,
        /// <summary>
        /// Конец игры, игрок установил новый рекорд.
        /// </summary>
        GameOverRecord
    }
}
