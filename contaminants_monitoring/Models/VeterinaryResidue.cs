//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Contaminants_Monitoring.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class VeterinaryResidue
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VeterinaryResidue()
        {
            this.VetResMRLs = new HashSet<VetResMRL>();
        }
    
        public int pkVeterinaryResidueId { get; set; }
        public string VetResidueName { get; set; }
        public string AntibioticsClass { get; set; }
        public string VetResidueCode { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VetResMRL> VetResMRLs { get; set; }
    }
}
