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
    
    public partial class CollectionDesign
    {
        public int pkCollectionDesignId { get; set; }
        public Nullable<int> fkLabSampleId { get; set; }
        public string fkDesignerId { get; set; }
        public Nullable<System.DateTime> GeneratedDate { get; set; }
        public string fkCollectorId { get; set; }
    }
}