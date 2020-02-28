using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETeam {
   None,TeamA,TeamB,
}
public class PlayerData : EntityData
{
    public ETeam eTeam = ETeam.None;
    public bool isMainPlayer = false;
}
