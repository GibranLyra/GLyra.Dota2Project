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
    
    public partial class AbilityType
    {
        public AbilityType()
        {
            this.SkillAbilityTypes = new HashSet<SkillAbilityTypes>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<SkillAbilityTypes> SkillAbilityTypes { get; set; }
    }
}
