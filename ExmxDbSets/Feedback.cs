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
    
    public partial class Feedback
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Feedback()
        {
            this.FeedbackReplies = new HashSet<FeedbackReply>();
        }
    
        public int FeedbackId { get; set; }
        public int FeedbackBy { get; set; }
        public int FeedbaclTo { get; set; }
        public string Feedback1 { get; set; }
        public double Rating { get; set; }
        public System.DateTime FeedbackDate { get; set; }
    
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FeedbackReply> FeedbackReplies { get; set; }
    }
}
