//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Madera.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ZoneMorte
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ZoneMorte()
        {
            this.Empreinte = new HashSet<Empreinte>();
        }
    
        public long idZoneMorte { get; set; }
        public Nullable<long> longueur { get; set; }
        public Nullable<long> largeur { get; set; }
        public Nullable<long> coordonneeX { get; set; }
        public Nullable<long> coordonneeY { get; set; }
        public Nullable<long> idEmpreinte { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Empreinte> Empreinte { get; set; }
        public virtual Empreinte Empreinte1 { get; set; }
    }
}
