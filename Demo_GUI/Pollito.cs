using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine2D;

namespace Demo_GUI
{
    internal class Pollito : GraphicsItem 
    {
        //atributos

        float x;
        float y;
        float vel;
        public Pollito(float x, float y, float vel) :
           base(x, y, 50, 50, "pollo")
        {
            this.x = x;
            this.y = y;
            this.vel = vel;
        }





    }
}
