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
    
    public partial class Finition
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Finition()
        {
            this.Gamme = new HashSet<Gamme>();
        }
    
        public long idFinition { get; set; }
        public string nom { get; set; }
        public Nullable<double> surCout { get; set; }
        public string imgUrl { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Gamme> Gamme { get; set; }
    }
}
