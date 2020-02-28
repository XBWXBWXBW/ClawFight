using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : EntityData
{
    public ETeam eTeam = ETeam.None;
    public bool isMainPlayer = false;
    public bool isInRoom = false;
}
