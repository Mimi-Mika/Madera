using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Madera.Model
{
    public class MasterClasse
    {
        public Client NewClient { get; set; }
        public Commercial NewCommercial { get; set; }
        public Couleur NewCouleur { get; set; }
        public Empreinte NewEmpreinte { get; set; }
        public Finition NewFinition { get; set; }
        public Projet NewProjet { get; set; }
        public Maison NewMaison { get; set; }
        public TypeDalle NewTypeDalle { get; set; } 
    }
}
