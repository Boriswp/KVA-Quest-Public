using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworkFun : NetworkBehaviour
{
    public PlayerController playerMovement;

    void Start()
    {
        Debug.Log(isLocalPlayer);
        playerMovement.isLocalPlayer = isLocalPlayer;
        playerMovement.StartFunc();
    }
}
