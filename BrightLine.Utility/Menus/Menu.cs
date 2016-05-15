using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Utility.Menus
{
    /// <summary>
    /// Top most level container for the menu items.
    /// </summary>
    public class Menu
    {
        public Menu()
        {
            Items = new List<MenuItem>();    
        }


        /// <summary>
        /// The items in the this menu.
        /// </summary>
        public List<MenuItem> Items;
    }
}
