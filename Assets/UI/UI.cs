using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Mirror;

public class UI : NetworkBehaviour
{
    public GameObject Controller;

    public Label Player1_Name;
    public Label Player2_Name;
    public Label Player3_Name;
    public Label Player4_Name;

    public Label Player1_Score;
    public Label Player2_Score;
    public Label Player3_Score;
    public Label Player4_Score;

    public GameObject Player1Winscreen;
    public GameObject Player2Winscreen;
    public GameObject Player3Winscreen;
    public GameObject Player4Winscreen;


    public void Start()
    {
        startUI();
        disableUI();
    }

    public void disableUI()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        root.SetEnabled(false);
        Player1_Score = root.Q<Label>("Score_Player1");
        Player1_Score.SetEnabled(false);
        Player2_Score = root.Q<Label>("Score_Player2");
        Player2_Score.SetEnabled(false);
        Player3_Score = root.Q<Label>("Score_Player3");
        Player3_Score.SetEnabled(false);
        Player4_Score = root.Q<Label>("Score_Player4");
        Player4_Score.SetEnabled(false);
    }

    public void startUI()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        Player1_Score = root.Q<Label>("Score_Player1");
        Player1_Score.text = "Verbleibende Karten " + 6;
        Player2_Score = root.Q<Label>("Score_Player2");
        Player2_Score.text = "Verbleibende Karten " + 6;
        Player3_Score = root.Q<Label>("Score_Player3");
        Player3_Score.text = "Verbleibende Karten " + 6;
        Player4_Score = root.Q<Label>("Score_Player4");
        Player4_Score.text = "Verbleibende Karten " + 6;

    }

    public void updateUI()
    {
        Game Script = Controller.GetComponent<Game>();
        var root = GetComponent<UIDocument>().rootVisualElement;
        Player1_Score = root.Q<Label>("Score_Player1");
        Player1_Score.text = "Verbleibende Karten " + (6 - Script.Players[0].GetComponent<Player>().cardsFinished.Count);
        Player2_Score = root.Q<Label>("Score_Player2");
        Player2_Score.text = "Verbleibende Karten " + (6 - Script.Players[1].GetComponent<Player>().cardsFinished.Count);
        Player3_Score = root.Q<Label>("Score_Player3");
        Player3_Score.text = "Verbleibende Karten " + (6 - Script.Players[2].GetComponent<Player>().cardsFinished.Count);
        Player4_Score = root.Q<Label>("Score_Player4");
        Player4_Score.text = "Verbleibende Karten " + (6 - Script.Players[3].GetComponent<Player>().cardsFinished.Count);
    }

    public void setTurn(int turn)
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        Player1_Name = root.Q<Label>("Player1_Label");
        Player2_Name = root.Q<Label>("Player2_Label");
        Player3_Name = root.Q<Label>("Player3_Label");
        Player4_Name = root.Q<Label>("Player4_Label");


        switch (turn)
        {
            case 0:
                root.SetEnabled(false);
                Player1_Name.SetEnabled(true);
                Player1_Score.SetEnabled(true);
                break;
            case 1:
                root.SetEnabled(false);
                Player2_Name.SetEnabled(true);
                Player2_Score.SetEnabled(true);
                break;
            case 2:
                root.SetEnabled(false);
                Player3_Name.SetEnabled(true);
                Player3_Score.SetEnabled(true);
                break;
            case 3:
                root.SetEnabled(false);
                Player4_Name.SetEnabled(true);
                Player4_Score.SetEnabled(true);
                break;
        }
    }

    [Command(requiresAuthority =false)]
    public void CallPlayerWins(int Player)
    {
        playerWins(Player);
    }

    [ClientRpc]
    public void playerWins(int Player)
    {
        switch(Player)
        {
            case 0:
                Player1Winscreen.transform.position = new Vector3(Player1Winscreen.transform.position.x, Player1Winscreen.transform.position.y, Player1Winscreen.transform.position.z - 6); 
                break;
            case 1:
                Player2Winscreen.transform.position = new Vector3(Player2Winscreen.transform.position.x, Player2Winscreen.transform.position.y, Player2Winscreen.transform.position.z - 6);
                break;
            case 2:
                Player3Winscreen.transform.position = new Vector3(Player3Winscreen.transform.position.x, Player3Winscreen.transform.position.y, Player3Winscreen.transform.position.z - 6);
                break;
            case 3:
                Player4Winscreen.transform.position = new Vector3(Player4Winscreen.transform.position.x, Player4Winscreen.transform.position.y, Player4Winscreen.transform.position.z - 6);
                break;
        }
    }

}
