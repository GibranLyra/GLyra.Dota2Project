//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GLyra.Dota2
{
    using System;
    using System.Collections.Generic;
    
    public partial class Skill
    {
        public Skill()
        {
            this.Hero = new HashSet<Hero>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> ManaCostLv1 { get; set; }
        public Nullable<int> ManaCostLv2 { get; set; }
        public Nullable<int> ManaCostLv3 { get; set; }
        public Nullable<int> ManaCostLv4 { get; set; }
        public Nullable<int> CoolDownLv1 { get; set; }
        public Nullable<int> CoolDownLv2 { get; set; }
        public Nullable<int> CoolDownLv3 { get; set; }
        public Nullable<int> CoolDownLv4 { get; set; }
        public int AbilityTypeId { get; set; }
        public Nullable<int> TargetAffectedTypeId { get; set; }
        public Nullable<int> DamageTypeId { get; set; }
        public int ImageId { get; set; }
        public string VideoUrl { get; set; }
    
        public virtual AbilityType AbilityType { get; set; }
        public virtual DamageType DamageType { get; set; }
        public virtual ICollection<Hero> Hero { get; set; }
        public virtual TargetAffectedType TargetAffectedType { get; set; }
    }
}
