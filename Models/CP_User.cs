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
    
    public partial class CP_User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CP_User()
        {
            this.CP_Course = new HashSet<CP_Course>();
            this.CP_Course1 = new HashSet<CP_Course>();
            this.CP_Duration = new HashSet<CP_Duration>();
            this.CP_Duration1 = new HashSet<CP_Duration>();
            this.CP_Duration2 = new HashSet<CP_Duration>();
            this.CP_User1 = new HashSet<CP_User>();
            this.CP_User11 = new HashSet<CP_User>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Nullable<int> CoursePrivilege { get; set; }
        public Nullable<int> CreatorID { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<int> ModifiedID { get; set; }
        public Nullable<System.DateTime> ModificationDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CP_Course> CP_Course { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CP_Course> CP_Course1 { get; set; }
        public virtual CP_Course CP_Course2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CP_Duration> CP_Duration { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CP_Duration> CP_Duration1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CP_Duration> CP_Duration2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CP_User> CP_User1 { get; set; }
        public virtual CP_User CP_User2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CP_User> CP_User11 { get; set; }
        public virtual CP_User CP_User3 { get; set; }
    }
}
