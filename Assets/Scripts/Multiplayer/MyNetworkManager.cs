using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyNetworkManager : NetworkManager
{


    public delegate void OnPLayerConnect();
    public static OnPLayerConnect onPLayerConnect;

    public delegate void OnPLayerDisconnect();
    public static OnPLayerDisconnect onPLayerDisconnect;

    [Scene]
    public string GamePlayScene;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        onPLayerConnect?.Invoke();
        base.OnServerAddPlayer(conn);
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        onPLayerDisconnect?.Invoke();
        base.OnServerDisconnect(conn);
    }

    public void LoadPlayScene() {

        onPLayerConnect = null;
        onPLayerDisconnect = null;
        ServerChangeScene(GamePlayScene);

    }
}
