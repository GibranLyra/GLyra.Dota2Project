//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GLyra.Dota2.Repositories
{
    using System;
    using System.Collections.Generic;
    
    public partial class Skill
    {
        public Skill()
        {
            this.SkillAbilityTypes = new HashSet<SkillAbilityTypes>();
            this.SkillEffectName = new HashSet<SkillEffectName>();
            this.SkillImage = new HashSet<SkillImage>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> ManaCostLv1 { get; set; }
        public Nullable<int> ManaCostLv2 { get; set; }
        public Nullable<int> ManaCostLv3 { get; set; }
        public Nullable<int> ManaCostLv4 { get; set; }
        public Nullable<decimal> CoolDownLv1 { get; set; }
        public Nullable<decimal> CoolDownLv2 { get; set; }
        public Nullable<decimal> CoolDownLv3 { get; set; }
        public Nullable<decimal> CoolDownLv4 { get; set; }
        public Nullable<int> TargetAffectedTypeId { get; set; }
        public Nullable<int> DamageTypeId { get; set; }
        public string VideoUrl { get; set; }
        public int HeroId { get; set; }
    
        public virtual DamageType DamageType { get; set; }
        public virtual Hero Hero { get; set; }
        public virtual TargetAffectedType TargetAffectedType { get; set; }
        public virtual ICollection<SkillAbilityTypes> SkillAbilityTypes { get; set; }
        public virtual ICollection<SkillEffectName> SkillEffectName { get; set; }
        public virtual ICollection<SkillImage> SkillImage { get; set; }
    }
}
