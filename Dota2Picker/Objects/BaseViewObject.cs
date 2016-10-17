using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Picker.Objects
{
    class BaseViewObject
    {
        private int _lastIndexForHeroesView;
        private static BaseViewObject baseView = new BaseViewObject();
        public static BaseViewObject bvoInstance
        {
            get
            {
                if (baseView == null)
                {
                    baseView = new BaseViewObject();
                }
                return baseView;
            }
        }

        public int lastHeroView {
            get { return this._lastIndexForHeroesView; }
            set { this._lastIndexForHeroesView = value; }
        }

    }
}
