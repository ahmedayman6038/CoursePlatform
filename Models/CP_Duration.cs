//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CoursePlatform.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CP_Duration
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CP_Duration()
        {
            this.CP_RegisteredCourse = new HashSet<CP_RegisteredCourse>();
        }
    
        public int ID { get; set; }
        public System.DateTime FromDate { get; set; }
        public System.DateTime ToDate { get; set; }
        public int CourseID { get; set; }
        public Nullable<int> MinCapacity { get; set; }
        public Nullable<int> MaxCapacity { get; set; }
        public Nullable<int> NumberRolled { get; set; }
        public bool Active { get; set; }
        public int CreatorID { get; set; }
        public Nullable<int> TerminatorID { get; set; }
        public Nullable<int> SerialFrom { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<int> ModifiedID { get; set; }
        public Nullable<System.DateTime> ModificationDate { get; set; }
    
        public virtual CP_Course CP_Course { get; set; }
        public virtual CP_User CP_User { get; set; }
        public virtual CP_User CP_User1 { get; set; }
        public virtual CP_User CP_User2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CP_RegisteredCourse> CP_RegisteredCourse { get; set; }
    }
}
