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
    
    public partial class CP_Registration
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CP_Registration()
        {
            this.CP_RegisteredCourse = new HashSet<CP_RegisteredCourse>();
        }
    
        public int ID { get; set; }
        public string StudentCode { get; set; }
        public string StudentEmail { get; set; }
        public string NationalID { get; set; }
        public string Password { get; set; }
        public bool ActivationStatus { get; set; }
        public string LevelCode { get; set; }
        public string PhoneNumber { get; set; }
        public string StudentName { get; set; }
        public string StudentFaculty { get; set; }
        public string StudentAddress { get; set; }
        public string TelephoneNumber { get; set; }
        public string StudentSection { get; set; }
        public string LevelName { get; set; }
        public string UniversityName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CP_RegisteredCourse> CP_RegisteredCourse { get; set; }
    }
}
