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
    
    public partial class SkillEffectName
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int SkillId { get; set; }
        public string ValueLv1 { get; set; }
        public string ValueLv2 { get; set; }
        public string ValueLv3 { get; set; }
        public string ValueLv4 { get; set; }
        public string ValueScepter { get; set; }
    
        public virtual Skill Skill { get; set; }
    }
}
