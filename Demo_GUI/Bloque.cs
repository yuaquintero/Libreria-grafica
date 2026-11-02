using Engine2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_GUI
{
    internal class Bloque : GraphicsItem
    {

        public Bloque(float x, float y, float ancho, float alto)
            : base(x, y, ancho, alto)
        {
            this.ColorItem = Color.OrangeRed;
        }
    }
}