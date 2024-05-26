using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Mirror;
using Assets.Scripts;

public class Game : NetworkBehaviour
{
    public GameObject UI;

    public GameObject Card;
    public GameObject Arrow;
    public GameObject ArrowTurn;
    public GameObject TreasureCard;
    public GameObject Player;

    public NetworkManager Networking;


    public List<GameObject> DestroyOnClose = new List<GameObject>();

    [SyncVar]
    public readonly SyncList<string> randomCards = new SyncList<string>();

    [SyncVar]
    public readonly SyncList<int> randomCardsRotation = new SyncList<int>();

    [SyncVar]
    public readonly SyncList<string> randomTreasureCards = new SyncList<string>();

    //board mit [column, row]
    public GameObject[,] board = new GameObject[7, 7];


    public GameObject currentCard;

    System.Random rng = new System.Random();

    private int lastRow = -1;
    private int lastCol = -1;

    Color green = new Color(0, 255, 0, 255);
    Color clear = new Color(255, 255, 255, 255);

    [SyncVar]
    public int currentPlayer = 0;

    public int localPlayer = -1;

    private bool turnPushed = false;

    [SyncVar]
    public bool startGame = false;

    bool startGameLocal = true;

    public List<GameObject> Players = new List<GameObject>() {};


    


    void Start()
    {

        setSolidCards();
        CreateArrows();
        createRandomCards();
        for (int i = 0; i < 4; i++)
        {
            addPlayer();
        }
        createTreasureCardsRandom();

        startTurn();
    }

    void Update()
    {
        if (NetworkClient.connection.identity == null)
        {
            return;
        }
        else
        {
            showCurrentPlayerCard();
        }

    

        if (!(NetworkClient.connection.identity.name.Contains((currentPlayer + 1).ToString())))
        {
            return;
        }
        Networking.GetComponent<NetworkManagerHUD>().enabled = false;
        //if (startGame && startGameLocal)
        //{
        //    startGameLocal = false;
        //    startTurn();
        //}
        //if (startGameLocal)
        //{
        //    return;
        //}

        if (Input.GetMouseButtonDown(0))
        {
            
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit)
            {
                if (hit.collider)
                {
                    var gObjName = hit.collider.gameObject.name;
                    gObjName = gObjName.Replace("(Clone)", "");
                    Debug.Log(NetworkClient.connection.identity.name);
                    askForPlayerTurn(mousePos, gObjName);
                }
            }
        }
    }

    [Command (requiresAuthority = false)]
    public void askForPlayerTurn(Vector2 mousePos, string gObjName)
    {
         doHit(mousePos, gObjName);
     
    }

    public void showCurrentPlayerCard()
    {
        for(int i = 0; i < Players.Count;i++)
        {
            if(NetworkClient.connection.identity.name.Contains((i+1).ToString()))
            {
                Players[i].GetComponent<Player>().currentCard.showCard();
            }
            else
            {
                Players[i].GetComponent<Player>().currentCard.hideCard();
            }
        }
    }

    [ClientRpc]
    private void doHit(Vector2 mousePos, string gObjName)
    {

        var hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (gObjName == "Arrow" && !turnPushed)
        {
            turnPushed = true;
            var gameObject = hit.collider.gameObject;
            var arrow = gameObject.GetComponent<Arrow>();

            if (!placeCard(arrow.column, arrow.row, currentCard.GetComponent<Card>().direction))
            {
                Debug.Log("karten Fehler");
                turnPushed = false;
                return;
            }
            Player current = Players[currentPlayer].GetComponent<Player>();
            possibleMovements(current.x, current.y);
            return;
        }
        if (gObjName == "ArrowTurn" && !turnPushed)
        {
            var card = currentCard.GetComponent<Card>();
            card.MoveCard(-2, 0, card.direction + 1);

            //Booleans mitdrehen:
            //boolTurn(card);
        }
        if (gObjName.StartsWith("corner") || gObjName.StartsWith("straight") || gObjName.StartsWith("start"))
        {
            var gameObject = hit.transform.gameObject;
            var card = gameObject.GetComponent<Card>();

            if (card.getGreen())
            {
                var curPlayer = Players[currentPlayer].GetComponent<Player>();
                curPlayer.movePlayer(card.xCoord, card.yCoord, false);
                for (int i = 0; i < 7; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        board[i, j].GetComponent<SpriteRenderer>().color = clear;
                        board[i, j].GetComponent<Card>().setGreen(false);
                    }
                }
                var curCard = board[curPlayer.x, curPlayer.y].GetComponent<Card>();
                if (curCard.treasure != null && curCard.treasure.ToUpper() == curPlayer.currentCard.treasure.ToUpper())
                {
                    curPlayer.pullCard();
                }
                UI.GetComponent<UI>().updateUI();
                setNextPlayerTurn();
            }
        }
    }



   // [Command(requiresAuthority = false)] //Test
    private void setNextPlayerTurn()
    {
        turnPushed = false;
        currentPlayer += 1;
        currentPlayer %= Players.Count;
        Debug.Log(currentPlayer);

        for (int i = 0; i < 4; i++)
        {
            Debug.Log(Players[i].name);
        }
    }

    public void possibleMovements(int column, int row)
    {
        try
        {
            Card card = board[column, row].GetComponent<Card>();

            if (!card.green)
            {
                card.GetComponent<SpriteRenderer>().color = green;
                card.green = true;

                if (card.openTop && row > 0 && board[column, row - 1].GetComponent<Card>().openBot)
                {
                    possibleMovements(column, row - 1);
                }

                if (card.openRight && column < 6 && board[column + 1, row].GetComponent<Card>().openLeft)
                {
                    possibleMovements(column + 1, row);
                }

                if (card.openBot && row < 6 && board[column, row + 1].GetComponent<Card>().openTop)
                {
                    possibleMovements(column, row + 1);
                }

                if (card.openLeft && column > 0 && board[column - 1, row].GetComponent<Card>().openRight)
                {
                    possibleMovements(column - 1, row);
                }
            }
        }
        catch (Exception ex)
        {

            serverLog(ex.Message);
        }
        
    }

    [Command]
    private void serverLog(string logString)
    {
        Debug.Log(logString);
    }

    public void startTurn()
    {
        foreach (var item in Players)
        {
            item.GetComponent<Player>().pullCard();

        }
    }



    #region placeObjects

    private bool calcIsSolid(int x, int y)
    {
        if (x % 2 == 0 && y % 2 == 0 && x > 0)
            return true;

        return false;
    }
    public void setSolidCards()
    {
        //Ecken erstellen
        board[0, 0] = CreateCard("startBlue", 0, 0, 0);
        board[6, 0] = CreateCard("startGreen", 6, 0, 1);
        board[6, 6] = CreateCard("startRed", 6, 6, 2);
        board[0, 6] = CreateCard("startYellow", 0, 6, 3);

        //solide Karten erstellen
        board[0, 2] = CreateCard("cornerStraightCoffe", 0, 2, 3);
        board[0, 4] = CreateCard("cornerStraightGameCube", 0, 4, 3);
        board[2, 0] = CreateCard("cornerStraightDucktape", 2, 0, 0);
        board[2, 2] = CreateCard("cornerStraightGollum", 2, 2, 3);
        board[2, 4] = CreateCard("cornerStraightKnightGold", 2, 4, 2);
        board[2, 6] = CreateCard("cornerStraightKnightRed", 2, 6, 2);
        board[4, 0] = CreateCard("cornerStraightMario", 4, 0, 0);
        board[4, 2] = CreateCard("cornerStraightMonitor", 4, 2, 0);
        board[4, 4] = CreateCard("cornerStraightPokeball", 4, 4, 1);
        board[4, 6] = CreateCard("cornerStraightPolice", 4, 6, 2);
        board[6, 2] = CreateCard("cornerStraightRaspb", 6, 2, 1);
        board[6, 4] = CreateCard("cornerStraightRemote", 6, 4, 1);
    }
    public void createRandomCards()
    {
        int counter = 0;
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (board[i, j] == null)
                {
                    
                    var currentCard = randomCards[counter];
                    board[i, j] = CreateCard(currentCard, i, j, randomCardsRotation[counter]);
                    counter++;
                }
            }
        }
        currentCard = CreateCard(randomCards[counter], -2, 0, 0);
    }

    public void CreateArrows()
    {
        //Pfeile erstellen
        //links
        for (int i = 1; i < 6; i += 2)
        {
            DestroyOnClose.Add(CreateArrow(-1, i, 0));
        }
        //rechts
        for (int i = 1; i < 6; i += 2)
        {
            DestroyOnClose.Add(CreateArrow(7, i, 2));
        }
        //oben
        for (int i = 1; i < 6; i += 2)
        {
            DestroyOnClose.Add(CreateArrow(i, -1, 1));
        }
        //unten
        for (int i = 1; i < 6; i += 2)
        {
            DestroyOnClose.Add(CreateArrow(i, 7, 3));
        }
       DestroyOnClose.Add(CreateArrowTurn());
    }

    public void SetCardList()
    {
        randomCards.AddRange(new List<string>() {
            "straight",
            "straight",
            "straight",
           "straight",
           "straight",
            "straight",
            "straight",
            "straight",
           "straight",
            "straight",
            "straight",
          "straight",
            "straight",
            "corner",
            "corner",
            "corner",
            "corner",
            "corner",
            "corner",
            "corner",
            "corner",
            "corner",
            "cornerRubicsCube",
           "cornerKrombacher",
           "cornerCola",
           "cornerCham",
            "cornerBattery",
            "cornerAnvil",
            "cornerStraightWalter",
            "cornerStraightSword",
         "cornerStraightSnes",
           "cornerStraightSmartphone",
           "cornerStraightSkull",
          "cornerStraightScrew" }
        );
        for (int i = 0; i < 34; i++)
        {
            randomCardsRotation.Add(rng.Next() % 4);
        }
        randomCards.Shuffle();

        randomTreasureCards.AddRange(
         new List<string>()
        {
            "Anvil", "Battery", "Cham", "Cola", "Krombacher", "RubicsCube",
        "Coffe", "Ducktape", "GameCube", "Gollum", "KnightGold",
        "KnightRed", "Mario", "Monitor", "Pokeball","Police",
        "Raspb", "Remote", "Screw", "Skull", "Smartphone","Snes",
        "Sword", "Walter"
        });

        randomTreasureCards.Shuffle();
    }


    public void createTreasureCardsRandom()
    {
        for (int i = 0; i < randomTreasureCards.Count; i++)
        {
            CreateTreasureCard(randomTreasureCards[i], i % Players.Count);
        }
        for (int i = 0; i < Players.Count; i++)
        {
            CreateTreasureCard("ReturnToStart", i);
        }
    }
    public GameObject addPlayer()
    {
        if (Players.Count >= 4)
            return null;

        Players.Add(CreatePlayer(Players.Count));
        return Players.Last();
    }


    #endregion


    #region placeCard
    private bool placeCard(int row, int column, int rotation)
    {
        if (!isValidPush(row, column))
            return false;

        Debug.Log("valid Push");

        switch (row)
        {
            case 0:
                pushColumnTop(column, rotation);
                break;
            case 6:
                pushColumnBot(column, rotation);
                break;
            default:
                switch (column)
                {
                    case 0:
                        pushRowLeft(row, rotation);
                        break;
                    case 6:
                        pushRowRight(row, rotation);
                        break;
                    default:
                        break;
                }
                break;
        }
        lastCol = column;
        lastRow = row;
        return true;
    }

    private bool isValidPush(int row, int column)
    {
        //invalidHandle
        if (0 > row || 6 < row || 0 > column || 6 < column)
        {
            return false;
        }
        //solid Field
        if (row % 2 == 0 && column % 2 == 0)
        {
            return false;
        }
        // row oder coulmn muss 0 oder 6 sein 
        if (row != 0 && column != 0 && row != 6 && column != 6)
        {
            return false;
        }
        //letzten zug prüfen -> darf niht rückgängig gemacht werden
        switch (row)
        {
            case 0:
                if (lastRow == 6 && lastCol == column)
                    return false;
                break;
            case 6:
                if (lastRow == 0 && lastCol == column)
                    return false;
                break;
            default:
                switch (column)
                {
                    case 0:
                        if (lastCol == 6 && lastRow == row)
                            return false;
                        break;
                    case 6:
                        if (lastCol == 0 && lastRow == row)
                            return false;
                        break;
                    default:
                        break;
                }
                break;
        }
        return true;
    }
    private void pushColumnTop(int column, int rotation)
    {
        var tempCard = board[column, 6];
        for (int i = 5; i >= 0; i--)
        {
            board[column, i + 1] = board[column, i];
            Card card = board[column, i + 1].GetComponent<Card>();
            card.MoveCard(column, i + 1, card.direction % 4);
        }

        board[column, 0] = currentCard;
        board[column, 0].GetComponent<Card>().MoveCard(column, 0, rotation);

        currentCard = tempCard;
        //boolReset(currentCard.GetComponent<Card>());
        currentCard.GetComponent<Card>().MoveCard(-2, 0, currentCard.GetComponent<Card>().direction);

        foreach (var item in Players)
        {
            var curPlayer = item.GetComponent<Player>();
            if (curPlayer.x == column)
            {
                curPlayer.movePlayer(curPlayer.x, (curPlayer.y + 1) % 7, true);
            }
        }
    }
    private void pushColumnBot(int column, int rotation)
    {
        var tempCard = board[column, 0];
        for (int i = 1; i <= 6; i++)
        {
            board[column, i - 1] = board[column, i];
            Card card = board[column, i - 1].GetComponent<Card>();
            card.MoveCard(column, i - 1, card.direction % 4);
        }

        board[column, 6] = currentCard;
        board[column, 6].GetComponent<Card>().MoveCard(column, 6, rotation);

        currentCard = tempCard;
        //boolReset(currentCard.GetComponent<Card>());
        currentCard.GetComponent<Card>().MoveCard(-2, 0, currentCard.GetComponent<Card>().direction);
        foreach (var item in Players)
        {
            var curPlayer = item.GetComponent<Player>();
            if (curPlayer.x == column)
            {
                curPlayer.movePlayer(curPlayer.x, (curPlayer.y - 1) % 7, true);
            }
        }
    }
    private void pushRowLeft(int row, int rotation)
    {
        var tempCard = board[6, row];
        for (int i = 5; i >= 0; i--)
        {
            board[i + 1, row] = board[i, row];
            Card card = board[i + 1, row].GetComponent<Card>();
            card.MoveCard(i + 1, row, card.direction % 4);
        }

        board[0, row] = currentCard;
        board[0, row].GetComponent<Card>().MoveCard(0, row, rotation);

        currentCard = tempCard;
        //boolReset(currentCard.GetComponent<Card>());
        currentCard.GetComponent<Card>().MoveCard(-2, 0, currentCard.GetComponent<Card>().direction);
        foreach (var item in Players)
        {
            var curPlayer = item.GetComponent<Player>();
            if (curPlayer.y == row)
            {
                curPlayer.movePlayer((curPlayer.x + 1) % 7, curPlayer.y, true);
            }
        }
    }
    private void pushRowRight(int row, int rotation)
    {
        var tempCard = board[0, row];
        for (int i = 1; i <= 6; i++)
        {
            board[i - 1, row] = board[i, row];
            Card card = board[i - 1, row].GetComponent<Card>();
            card.MoveCard(i - 1, row, card.direction % 4);

        }

        board[6, row] = currentCard;
        board[6, row].GetComponent<Card>().MoveCard(6, row, rotation);

        currentCard = tempCard;
        //boolReset(currentCard.GetComponent<Card>());
        currentCard.GetComponent<Card>().MoveCard(-2, 0, currentCard.GetComponent<Card>().direction);
        foreach (var item in Players)
        {
            var curPlayer = item.GetComponent<Player>();
            if (curPlayer.y == row)
            {
                curPlayer.movePlayer((curPlayer.x - 1) % 7, curPlayer.y, true);
            }
        }
    }
    //public void boolTurn(Card turnCard)
    //{
    //    bool tempTop = turnCard.getOpenTop();
    //    bool tempRight = turnCard.getOpenRight();
    //    bool tempBot = turnCard.getOpenBot();
    //    bool tempLeft = turnCard.getOpenLeft();

    //    turnCard.setOpenTop(tempLeft);
    //    turnCard.setOpenRight(tempTop);
    //    turnCard.setOpenBot(tempRight);
    //    turnCard.setOpenLeft(tempBot);
    //}



    //public void boolReset(Card pleaseReset)
    //{
    //    int i = pleaseReset.direction;
    //    while (i % 4 != 0)
    //    {
    //        boolTurn(pleaseReset);
    //        i++;
    //    }
    //}

    #endregion


    #region CreateGameObjects

    public GameObject CreatePlayer(int id )
    {
        string color = "";
        switch (id)
        {
            case 0:
                color = "BLUE";
                break;
            case 1:
                color = "GREEN";
                break;
            case 2:
                color = "RED";
                break;
            case 3:
                color = "YELLOW";
                break;
            default:
                break;
        }
        GameObject gameObject = Instantiate(Player, new Vector3(0, 0, 0.1f), Quaternion.identity);
        Player player = gameObject.GetComponent<Player>();
        player.id = id;
        player.Activate(color);
        return gameObject;
    }
    public GameObject CreateCard(string name, int x, int y, int rotation)
    {
        GameObject gameObject = Instantiate(Card, new Vector3(0, 0, 0.1f), Quaternion.identity);
        Card card = gameObject.GetComponent<Card>();
        card.name = name;
        card.Activate();
        card.MoveCard(x, y, rotation % 4);
        card.isSolid = calcIsSolid(x, y);
        return gameObject;
    }

    public GameObject CreateTreasureCard(string name, int player)
    {
        GameObject gameObject = Instantiate(TreasureCard, new Vector3(0, 0, 0.1f), Quaternion.identity);
        TreasureCard card = gameObject.GetComponent<TreasureCard>();
        card.name = name;
        card.Activate();
        if (card.name == "ReturnToStart")
        {
            card.treasure = Players[player].GetComponent<Player>().color.ToUpper();
        }
        card.MoveCard(-1);

        Debug.Log("------>" + Players[player].GetComponent<Player>().name);

        Players[player].GetComponent<Player>().addCard(card);
        return gameObject;
    }


    public GameObject CreateArrow(int x, int y, int rotation)
    {
        GameObject gameObject = Instantiate(Arrow, new Vector3(0, 0, 0.1f), Quaternion.identity);
        Arrow arrow = gameObject.GetComponent<Arrow>();
        arrow.Activate();
        arrow.MoveArrow(x, y, rotation % 4);
        return gameObject;
    }


    public GameObject CreateArrowTurn()
    {
        GameObject gameObject = Instantiate(ArrowTurn, new Vector3(0, 0, 0.1f), Quaternion.identity);
        ArrowTurn arrow = gameObject.GetComponent<ArrowTurn>();
        arrow.Activate();
        arrow.placeArrow();
        return gameObject;
    }
    #endregion
}