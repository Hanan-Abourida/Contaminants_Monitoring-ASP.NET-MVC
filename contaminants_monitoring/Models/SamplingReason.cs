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
    
    public partial class SamplingReason
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SamplingReason()
        {
            this.LabSamples = new HashSet<LabSample>();
        }
    
        public int pkSamplingReasonId { get; set; }
        public string SamplingReasonCode { get; set; }
        public string SamplingReasonText { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LabSample> LabSamples { get; set; }
    }
}
