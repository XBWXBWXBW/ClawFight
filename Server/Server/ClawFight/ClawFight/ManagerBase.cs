using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClawFight
{
    class ManagerBase<T> where T : ManagerBase<T> ,new()
    {
        private static T _instance;
        public static T instance {
            get {
                if (_instance == null) {
                    _instance = new T();
                }
                return _instance;
            }
        }
    }
}
