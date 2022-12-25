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
        /// Начальный экран игры.
        /// </summary>
        StartScreen,
        /// <summary>
        /// Игра.
        /// </summary>
        Game,
        /// <summary>
        /// Игра стоит на паузе из-за того, что открыты карты.
        /// </summary>
        PauseOnOpening,
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
