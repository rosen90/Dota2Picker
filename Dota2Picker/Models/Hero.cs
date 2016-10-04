using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Picker.Models
{
    class Hero
    {
        public int id { get; set; }
        public string name { get; set; }
        public int attack_id { get; set; }
        public int attribute_id { get; set; }
        public int role_id { get; set; }
        public string type_name { get; set; }
        public string attribute_name { get; set; }

    }
}
