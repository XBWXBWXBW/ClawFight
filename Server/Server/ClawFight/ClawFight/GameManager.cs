using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;

namespace ClawFight
{
    class GameManager : ManagerBase<GameManager>
    {
        public int mapID = 1;
        public const int START_COUNT = 2;
        public override void Init()
        {
            base.Init();
            PlayerManager.instance.Init();
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        public bool CanEnterReady() {
            int _curCount = 0;
            foreach (var e in PlayerManager.instance.GetPlayerInRoom()) {
                if (e.Value.playerData.isReady) _curCount++;
            }
            if (_curCount > START_COUNT - 1) return false;
            return true;
        }
        public bool CanEnterPlay()
        {
            int _curCount = 0;
            foreach (var e in PlayerManager.instance.GetPlayerInRoom())
            {
                if (e.Value.playerData.isReady) _curCount++;
            }
            if (_curCount == START_COUNT) return true;
            return false;
        }
        public void PlayStart() {
            PlayerManager.instance.PlayStart();
        }
    }
}
