using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Madera.View.Pages.PlanVues
{
    class ModMaisonPos
    {
        public int posXDebut;
        public int posYDebut;
        public int posXFin;
        public int posYFin;

        public void position(int x , int y)
        {
            if ((x % 2) == 0)
            {
                switch (x)
                {
                    case 0:
                        posXDebut = x;
                        break;
                    default:
                        posXDebut = (x) / 2;
                        break;
                }
                switch (y)
                {
                    case 1:
                        posYDebut = y - 1;
                        break;
                    default:
                        posYDebut = (y - 1) / 2;
                        break;
                }
                posXFin = posXDebut;
                posYFin = posYDebut + 1;
            }
            else
            {
                switch (x)
                {
                    case 1:
                        posXDebut = x - 1;
                        break;
                    default:
                        posXDebut = (x - 1) / 2;
                        break;
                }
                switch (y)
                {
                    case 0:
                        posYDebut = y;
                        break;
                    default:
                        posYDebut = (y) / 2;
                        break;
                }
                posXFin = posXDebut + 1;
                posYFin = posYDebut;
            }
        }
    }
}
