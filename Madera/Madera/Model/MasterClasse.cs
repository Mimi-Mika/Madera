using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Madera.Model
{
    public class MasterClasse
    {
        //TODO: Modifier nom si nouvelle table a entrer: New sinon Lock
        public Commercial LockCommercial { get; set; }
        public Client LockClient { get; set; }
        public Projet NewProjet { get; set; }

        public Maison NewMaison { get; set; }
        public Empreinte LockEmpreinte { get; set; }
        public ZoneMorte LockZoneMorte { get; set; }

        public Maison_TypeDalle NewMaisonTypeDalle { get; set; }
        public TypeDalle LockTypeDalle { get; set; }

        public Projet_EtatCommande NewProjetEtatCommande { get; set; }
        public EtatCommande LockEtatCommande { get; set; }

        public Couleur NewCouleur { get; set; }
        public Finition NewFinition { get; set; }
        public List<Module_Maison> NewModuleMaison { get; set; }
        public List<Module> NewModule { get; set; }
    }
}
