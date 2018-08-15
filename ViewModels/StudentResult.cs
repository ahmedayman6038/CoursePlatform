using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoursePlatform.ViewModels
{
    public class StudentResult
    {
        public DateTime DurationFrom { get; set; }

        public DateTime DurationTo { get; set; }

        public string StudentName { get; set; }

        public string StudentLevel { get; set; }

        public string StudentFaculty { get; set; }

        public string ResultName { get; set; }
    }
}