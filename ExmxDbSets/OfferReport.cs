//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Roof_Care.ExmxDbSets
{
    using System;
    using System.Collections.Generic;
    
    public partial class OfferReport
    {
        public int Id { get; set; }
        public int ReportedById { get; set; }
        public int ReportedOfferId { get; set; }
        public string ReportText { get; set; }
    
        public virtual Offer Offer { get; set; }
        public virtual User User { get; set; }
    }
}
