using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchLocation
{
    public int touchId;
    public Player _player;

    public touchLocation(int newTouchId, Player newPlayer)
    {
        touchId = newTouchId;
        _player = newPlayer;
    }
}
