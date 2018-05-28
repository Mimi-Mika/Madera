using Madera.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Madera.View.Pages.PlanVues
{
    class Zone
    {
        public int zoneMorteTailleX { get; set; }
        public int zoneMorteTailleY { get; set; }
        public int zoneMorteCoordX { get; set; }
        public int zoneMorteCoordY { get; set; }

        public void ZoneMorteCoordonee(ZoneMorte zoneMorteSelection)
        {
            if ((int)zoneMorteSelection.coordonneeX != 0)
            {
                zoneMorteCoordX = ((int)zoneMorteSelection.coordonneeX * 2 + 1);
                zoneMorteTailleX = ((int)zoneMorteSelection.longueur * 2 + 1);
            }
            else
            {
                zoneMorteTailleX = ((int)zoneMorteSelection.longueur * 2 + 1) + 1;
            }

            if ((int)zoneMorteSelection.coordonneeY != 0)
            {
                zoneMorteCoordY = ((int)zoneMorteSelection.coordonneeY * 2 + 1);
                zoneMorteTailleY = ((int)zoneMorteSelection.largeur * 2 + 1);
            }
            else
            {
                zoneMorteTailleY = ((int)zoneMorteSelection.largeur * 2 + 1) + 1;
            }
        }
    }
}
