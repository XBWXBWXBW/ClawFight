using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClawFight
{
    class GameManager
    {
        public GameManager() {
            PlayerManager.instance.Init();
        }
    }
}
