//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PracticalTest.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmployeeMaster
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<int> DesignationId { get; set; }
        public Nullable<System.DateTime> DateOfJoining { get; set; }
        public Nullable<decimal> Salary { get; set; }
        public string PhotoPath { get; set; }
    
        public virtual DesignationMaster DesignationMaster { get; set; }
    }
}
