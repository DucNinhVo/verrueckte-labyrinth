using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    // Start is called before the first frame update

    public int id;
    public int x;
    public int y;
    public string color;
    public GameObject Controller;
    List<TreasureCard> cardsOpen = new List<TreasureCard>();
    public List<TreasureCard> cardsFinished = new List<TreasureCard>();
    public TreasureCard currentCard;

    public GameObject UI;


    public void Start()
    {
        UI = GameObject.FindGameObjectWithTag("UI");
        Controller = GameObject.FindGameObjectWithTag("GameController");
        Game Script = Controller.GetComponent<Game>();
        //Script.C2S_AddConnectionID(NetworkClient.connection.connectionId, NetworkClient.connection.identity.name);
        Debug.Log("Erreicht - " + NetworkClient.connection.identity);
        //Script.C2S_AddPlayer(NetworkClient.connection.identity.gameObject);
    }

    public void Activate(string playerColor)
    {
        Controller = GameObject.FindGameObjectWithTag("GameController");
        this.color = playerColor;
        

        switch (playerColor.ToUpper())
        {
            case "BLUE":
                movePlayer(0, 0 ,true); 
                GetComponent<SpriteRenderer>().color = Color.blue;
                break;
                case "GREEN":
                movePlayer(6, 0 ,true);
                GetComponent<SpriteRenderer>().color = Color.green;
                break;
                case "RED":
                movePlayer(6, 6 ,true);
                GetComponent<SpriteRenderer>().color = Color.red;
                break;
                case "YELLOW":
                movePlayer(0, 6 ,true);
                GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            default:
                break;
        }
    }

    public void pullCard()
    {
        if (currentCard != null)
        {
            currentCard.MoveCard(-1);
            cardsFinished.Add(currentCard);
        }   

        if (cardsOpen.Count > 0)
        {
            currentCard = cardsOpen.First();
            currentCard.MoveCard(id);
            cardsOpen.RemoveRange(0, 1);
        }
        else
        {
            currentCard = null;
            //hier zum Sieger setzen
            UI Overlay = UI.GetComponent<UI>();
            
            for(int i = 0;i <4;i++)
            {
                if(this.GetComponent<Player>().name.Contains((i+1).ToString()))
                {
                    Overlay.CallPlayerWins(i);
                }
            }

        }
    }

   //[Command(requiresAuthority =false)] //Test
    public void movePlayer(int toX, int toY,bool suppress) //Test
    {
        if (!suppress)
        {
            if (!Controller.GetComponent<Game>().board[toX, toY].GetComponent<Card>().getGreen())
                return;
        }
        while (toX < 0)
        {
            toX += 7;
        }
        while(toY < 0)
        {
            toY += 7;
        }
        toX %= 7;
        toY %= 7;

        float calculatedX = toX * 3.37f;
        calculatedX -= 12;
        float calculatedY = toY * -3.37f;
        calculatedY += 10.0f;
        x = toX;
        y = toY;
        float space = 0.6f;
        switch (color.ToUpper())
        {
            case "BLUE":
                calculatedX -= space;
                calculatedY += space;
                break;
            case "GREEN":
                calculatedX += space;
                calculatedY += space;
                break;
            case "RED":
                calculatedX += space;
                calculatedY -= space;
                break;
            case "YELLOW":
                calculatedX -= space;
                calculatedY -= space;
                break;
            default:
                break;
        }

        this.transform.position = new Vector3(calculatedX, calculatedY, -0.2f);

    }

    //[Command(requiresAuthority =false)] //Test
    public void addCard(TreasureCard card)
    {
        cardsOpen.Add(card);
    }

}

