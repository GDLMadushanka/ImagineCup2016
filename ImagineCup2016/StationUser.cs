//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ImagineCup2016
{
    using System;
    using System.Collections.Generic;
    
    public partial class StationUser
    {
        public int Id { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> StationId { get; set; }
    
        public virtual station station { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}
