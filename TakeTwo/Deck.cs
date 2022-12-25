using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeTwo
{
    /// <summary>
    /// Карточная колода.
    /// </summary>
    internal class Deck
    {
        /// <summary>
        /// Рубашка.
        /// </summary>
        public string CardShirt;

        /// <summary>
        /// Карты в колоде.
        /// </summary>
        public List<string> Cards = new List<string>();
    }
}
