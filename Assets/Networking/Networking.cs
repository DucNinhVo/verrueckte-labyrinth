using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Networking : NetworkManager
{

    public GameObject[] PlayerPrefabs = new GameObject[4];

    public GameObject GameController;

    public override void OnClientConnect() //wird vor AddPlayer ausgeführt
    {
        Debug.Log("On Client Connect \n");
        Game Script = GameController.GetComponent<Game>();
        Debug.Log(numPlayers);

    }

    


    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Game Script = GameController.GetComponent<Game>();
        Debug.Log("OnServerAddPlayer ausgeführt");
        GameObject Player = null;

        switch (numPlayers)
        {
            case 0:
                Player = Instantiate(PlayerPrefabs[0]);
                Debug.Log("Case 0");
                NetworkServer.Spawn(Player);

                break;
            case 1:
                Player = Instantiate(PlayerPrefabs[1]);
                Debug.Log("Case 1");
                NetworkServer.Spawn(Player);

                break;
            case 2:
                Player = Instantiate(PlayerPrefabs[2]);
                Debug.Log("Case 2");
                NetworkServer.Spawn(Player);

                break;
            case 3:
                Player = Instantiate(PlayerPrefabs[3]);
                Debug.Log("Case 3");
                NetworkServer.Spawn(Player);

                break;
            default:
                Debug.Log(numPlayers);
                break;
        }

        if (Player != null)
        {
            NetworkServer.AddPlayerForConnection(conn, Player);
            Debug.Log("Player Connected");
        }
        else
        {
            Debug.Log("Player Object unverändert");
        }

    }

    public override void OnStartServer()
    {
        Game Script = GameController.GetComponent<Game>();
        Script.SetCardList();
    }

    public override void OnStopServer()
    {
        //Game Script aus Controller erhalten
        Game Script = GameController.GetComponent<Game>();

        //Alle Tiles löschen und Array auf null setzen
        for (int i = 0; i <= 6; i++)
        {
            for (int j = 0; j <= 6; j++)
            {
                Destroy(Script.board[i, j]);
                Script.board[i, j] = null;
            }
        }
        for (int i = 3; i >=0 ; i--)
        {
            if(Script.Players.Count>i)
            Destroy(Script.Players[i]);
        }
        if(Script.currentCard != null)
        Destroy(Script.currentCard);



    }




}
