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
    
    public partial class MycotoxinML
    {
        public int pkMycotoxinMLId { get; set; }
        public int fkMycotoxinId { get; set; }
        public double ML { get; set; }
        public int fkUOMId { get; set; }
        public int fkCommodityId { get; set; }
    
        public virtual Commodity Commodity { get; set; }
        public virtual Mycotoxin Mycotoxin { get; set; }
    }
}
