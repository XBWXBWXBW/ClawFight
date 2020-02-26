using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public PlayerData playerData;
    public Player(PlayerData d) : base(d){
        playerData = d;
    }
}
