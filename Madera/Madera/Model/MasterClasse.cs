using System;
using System.Collections;
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

        public List<Projet_EtatCommande> NewProjetEtatCommande { get; set; }
        public List<EtatCommande> LockEtatCommande { get; set; }

        public List<Couleur> LockCouleur { get; set; }
        public List<Couleur_Module> NewCouleurModule { get; set; }
        public List<Finition> LockFinition { get; set; }
        public List<Module_Maison> NewModuleMaison { get; set; }
        public List<TypeModule> LockTypeModule { get; set; }
        public List<Module> LockModule { get; set; }

        public List<Favori> NewFavori { get; set; }
        public List<Module_Favori> NewModuleFavori { get; set; }
        public List<Gamme> LockGamme { get; set; }


    }
}
