using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Picker.Models
{
    class IsWeakAgainst
    {
        public int _id;
        public string imgPath;
        public string _type;
        public string borderColor;
        public int weak_id { get { return this._id; } set { this._id = value; this.imgPath = "ms-appx:///Resources/Images/" + value.ToString() + ".png"; } }
        public string name { get; set; }

        public string type { get { return this._type; }
            set
            {
                _type = value; 
                if ( _type == "In Lane")
                {
                    borderColor = "Green";
                }
                else
                {
                    borderColor = "Red";
                }
            }
        }

    }
}
