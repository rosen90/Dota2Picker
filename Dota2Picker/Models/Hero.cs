

namespace Dota2Picker.Models
{
    class Hero
    {
        public int _id;
        public string imgPath;
        public int id { get { return this._id; } set { this._id = value; this.imgPath = "ms-appx:///Resources/Images/" + value.ToString() +".png"; } }
        public string name { get; set; }
        public int attack_id { get; set; }
        public int attribute_id { get; set; }
        public int role_id { get; set; }
        public string type_name { get; set; }
        public string attribute_name { get; set; }
        

    }
}
