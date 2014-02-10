using GLyra.Dota2.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLyra.Dota2.Converters
{
    public class AttributesCreator
    {
        Attributes attribute;

        public AttributesCreator(string agility, string intelligence, string strength, string damage, string moveSpeed, string armor)
        {
            attribute.Agility = agility;
            attribute.Intelligence = intelligence;
            attribute.Strength = strength;
            attribute.Damage = damage;
            attribute.MoveSpeed = moveSpeed;
            attribute.Armor = armor;
        }
    }
}
