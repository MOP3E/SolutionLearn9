using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeTwo
{
    internal class Card
    {
        /// <summary>
        /// Спрайт, которым рисуется карта.
        /// </summary>
        public string Sprite;

        /// <summary>
        /// Позиция карты.
        /// </summary>
        public Vector2Int Position;

        /// <summary>
        /// Карта вскрыта и её может видеть игрок.
        /// </summary>
        public bool IsOpen;
    }
}
