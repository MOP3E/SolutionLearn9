using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeTwo
{
    /// <summary>
    /// Состояние игровой карты.
    /// </summary>
    internal enum CardState
    {
        /// <summary>
        /// Карта закрыта.
        /// </summary>
        Closed,
        /// <summary>
        /// Карта открыта.
        /// </summary>
        Opened,
        /// <summary>
        /// Карта удалена с игрового поля.
        /// </summary>
        Fired
    }
}
