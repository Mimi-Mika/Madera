//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Madera.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Module
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Module()
        {
            this.Module_Favori = new HashSet<Module_Favori>();
            this.Couleur_Module = new HashSet<Couleur_Module>();
            this.Module_Maison = new HashSet<Module_Maison>();
        }
    
        public long idModule { get; set; }
        public string nom { get; set; }
        public Nullable<long> hauteur { get; set; }
        public Nullable<long> largeur { get; set; }
        public Nullable<double> prix { get; set; }
        public Nullable<long> idGamme { get; set; }
        public Nullable<long> idType { get; set; }
        public string imgUrl { get; set; }
    
        public virtual Gamme Gamme { get; set; }
        public virtual TypeModule TypeModule { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Module_Favori> Module_Favori { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Couleur_Module> Couleur_Module { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Module_Maison> Module_Maison { get; set; }
    }
}
