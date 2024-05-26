using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using Mirror;

public class Card : NetworkBehaviour
{
    public GameObject controller;

    //normale Karten
    public Sprite corner, straight, cornerStraight;
    //start Karten
    public Sprite startBlue, startGreen, startRed, startYellow;
    //Schatzkarten Corner
    public Sprite cornerAnvil, cornerBattery, cornerCham, cornerCola, cornerKrombacher, cornerRubicsCube;
    //Schatzkarten CornerStraight
    public Sprite cornerStraightCoffe, cornerStraightDucktape, cornerStraightGameCube, cornerStraightGollum, cornerStraightKnightGold,
                cornerStraightKnightRed, cornerStraightMario, cornerStraightMonitor, cornerStraightPokeball, cornerStraightPolice,
                cornerStraightRaspb, cornerStraightRemote, cornerStraightScrew, cornerStraightSkull, cornerStraightSmartphone, cornerStraightSnes,
                cornerStraightSword, cornerStraightWalter;

    public  bool openTop, openBot, openRight, openLeft;
    public bool green = false;

    public string treasure;

    public int xCoord = 0;
    public int yCoord = 0;

    public bool isSolid;

    // 0 = nicht gedreht, 1 = 90° nach rechts, 2 = 180° 3 = 90° nach links
    public int direction = 0;


public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        switch (this.name)
        {
            case "corner":
                this.GetComponent<SpriteRenderer>().sprite = corner;
                openTop = false;
                openRight = true;
                openBot = true;
                openLeft = false;
                break;
            case "straight":
                this.GetComponent<SpriteRenderer>().sprite = straight;
                openTop = false;
                openRight = true;
                openBot = false;
                openLeft = true;
                break;
            case "cornerStraight":
                this.GetComponent<SpriteRenderer>().sprite = cornerStraight;
                openTop = false;
                openRight = true;
                openBot = true;
                openLeft = true;
                break;
            case "startBlue":
                this.GetComponent<SpriteRenderer>().sprite = startBlue;
                treasure = "BLUE";
                openTop = false;
                openRight = true;
                openBot = true;
                openLeft = false;
                break;
            case "startGreen":
                this.GetComponent<SpriteRenderer>().sprite = startGreen;
                treasure = "GREEN";
                openTop = false;
                openRight = true;
                openBot = true;
                openLeft = false;
                break;
            case "startRed":
                this.GetComponent<SpriteRenderer>().sprite = startRed;
                treasure = "RED";
                openTop = false;
                openRight = true;
                openBot = true;
                openLeft = false;
                break;
            case "startYellow":
                this.GetComponent<SpriteRenderer>().sprite = startYellow;
                treasure = "YELLOW";
                openTop = false;
                openRight = true;
                openBot = true;
                openLeft = false;
                break;
            case "cornerStraightCoffe":
                this.GetComponent<SpriteRenderer>().sprite = cornerStraightCoffe;
                treasure = "COFFE";
                openTop = false;
                openRight = true;
                openBot = true;
                openLeft = true;
                break;
            case "cornerStraightDucktape":
                this.GetComponent<SpriteRenderer>().sprite = cornerStraightDucktape;
                treasure = "DUCKTAPE";
                openTop = false;
                openRight = true;
                openBot = true;
                openLeft = true;
                break;
            case "cornerStraightGameCube":
                this.GetComponent<SpriteRenderer>().sprite = cornerStraightGameCube;
                treasure = "GAMECUBE";
                openTop = false;
                openRight = true;
                openBot = true;
                openLeft = true;
                break;
            case "cornerStraightGollum":
                this.GetComponent<SpriteRenderer>().sprite = cornerStraightGollum;
                treasure = "GOLLUM";
                openTop = false;
                openRight = true;
                openBot = true;
                openLeft = true;
                break;
            case "cornerStraightKnightGold":
                this.GetComponent<SpriteRenderer>().sprite = cornerStraightKnightGold;
                treasure = "KNIGHTGOLD";
                openTop = false;
                openRight = true;
                openBot = true;
                openLeft = true;
                break;
            case "cornerStraightKnightRed":
                this.GetComponent<SpriteRenderer>().sprite = cornerStraightKnightRed;
                treasure = "KNIGHTRED";
                openTop = false;
                openRight = true;
                openBot = true;
                openLeft = true;
                break;
            case "cornerStraightMario":
                this.GetComponent<SpriteRenderer>().sprite = cornerStraightMario;
                treasure = "MARIO";
                openTop = false;
                openRight = true;
                openBot = true;
                openLeft = true;
                break;
            case "cornerStraightMonitor":
                this.GetComponent<SpriteRenderer>().sprite = cornerStraightMonitor;
                treasure = "MONITOR";
                openTop = false;
                openRight = true;
                openBot = true;
                openLeft = true;
                break;
            case "cornerStraightPokeball":
                this.GetComponent<SpriteRenderer>().sprite = cornerStraightPokeball;
                treasure = "POKEBALL";
                openTop = false;
                openRight = true;
                openBot = true;
                openLeft = true;
                break;
            case "cornerStraightPolice":
                this.GetComponent<SpriteRenderer>().sprite = cornerStraightPolice;
                treasure = "POLICE";
                openTop = false;
                openRight = true;
                openBot = true;
                openLeft = true;
                break;
            case "cornerStraightRaspb":
                this.GetComponent<SpriteRenderer>().sprite = cornerStraightRaspb;
                treasure = "RASPB";
                openTop = false;
                openRight = true;
                openBot = true;
                openLeft = true;
                break;
            case "cornerStraightRemote":
                this.GetComponent<SpriteRenderer>().sprite = cornerStraightRemote;
                treasure = "REMOTE";
                openTop = false;
                openRight = true;
                openBot = true;
                openLeft = true;
                break;
            case "cornerStraightScrew":
                this.GetComponent<SpriteRenderer>().sprite = cornerStraightScrew;
                treasure = "SCREW";
                openTop = false;
                openRight = true;
                openBot = true;
                openLeft = true;
                break;
            case "cornerStraightSkull":
                this.GetComponent<SpriteRenderer>().sprite = cornerStraightSkull;
                treasure = "SKULL";
                openTop = false;
                openRight = true;
                openBot = true;
                openLeft = true;
                break;
            case "cornerStraightSmartphone":
                this.GetComponent<SpriteRenderer>().sprite = cornerStraightSmartphone;
                treasure = "SMARTPHONE";
                openTop = false;
                openRight = true;
                openBot = true;
                openLeft = true;
                break;
            case "cornerStraightSnes":
                this.GetComponent<SpriteRenderer>().sprite = cornerStraightSnes;
                treasure = "SNES";
                openTop = false;
                openRight = true;
                openBot = true;
                openLeft = true;
                break;
            case "cornerStraightSword":
                this.GetComponent<SpriteRenderer>().sprite = cornerStraightSword;
                treasure = "SWORD";
                openTop = false;
                openRight = true;
                openBot = true;
                openLeft = true;
                break;
            case "cornerStraightWalter":
                this.GetComponent<SpriteRenderer>().sprite = cornerStraightWalter;
                treasure = "WALTER";
                openTop = false;
                openRight = true;
                openBot = true;
                openLeft = true;
                break;

            case "cornerAnvil":
                this.GetComponent<SpriteRenderer>().sprite = cornerAnvil;
                treasure = "ANVIL";
                openTop = false;
                openRight = true;
                openBot = true;
                openLeft = false;
                break;
            case "cornerBattery":
                this.GetComponent<SpriteRenderer>().sprite = cornerBattery;
                treasure = "BATTERY";
                openTop = false;
                openRight = true;
                openBot = true;
                openLeft = false;
                break;
            case "cornerCham":
                this.GetComponent<SpriteRenderer>().sprite = cornerCham;
                treasure = "CHAM";
                openTop = false;
                openRight = true;
                openBot = true;
                openLeft = false;
                break;
            case "cornerCola":
                this.GetComponent<SpriteRenderer>().sprite = cornerCola;
                treasure = "COLA";
                openTop = false;
                openRight = true;
                openBot = true;
                openLeft = false;
                break;
                 case "cornerKrombacher":
                this.GetComponent<SpriteRenderer>().sprite = cornerKrombacher;
                treasure = "KROMBACHER";
                openTop = false;
                openRight = true;
                openBot = true;
                openLeft = false;
                break;
                 case "cornerRubicsCube":
                this.GetComponent<SpriteRenderer>().sprite = cornerRubicsCube;
                treasure = "RUBICSCUBE";
                openTop = false;
                openRight = true;
                openBot = true;
                openLeft = false;
                break;
            default:
                break;
        }
        direction = 0;
    }

    public void MoveCard(int x, int y, int rotation)
    {
        if (isSolid)
        {
            return;
        }
        xCoord = x;
        yCoord = y;
        
        
        float calculatedX = x*3.37f;
        calculatedX -= 12;
        float calculatedY = y*-3.37f;
        calculatedY += 10.0f;

        this.transform.position = new Vector3(calculatedX, calculatedY, -0.1f);
        if(direction != rotation)
        {
            int difference = rotation-direction;
            direction = rotation;
            difference %= 4;
            this.transform.Rotate(0, 0, difference *-90);

            for (int i = 0; i < difference; i++)
            {
                boolTurn();
            }
        }
    }

    public void boolTurn()
    {
        bool tempTop = getOpenTop();
        bool tempRight = getOpenRight();
        bool tempBot = getOpenBot();
        bool tempLeft = getOpenLeft();

        setOpenTop(tempLeft);
        setOpenRight(tempTop);
        setOpenBot(tempRight);
        setOpenLeft(tempBot);
    }

    //Getter

    public bool getOpenTop()
    {
        return openTop;
    }

    public bool getOpenRight()
    {
        return openRight;
    }

    public bool getOpenBot()
    {
        return openBot;
    }

    public bool getOpenLeft()
    {
        return openLeft;
    }

    public bool getGreen()
    {
        return green;
    }

    public int getColumn()
    {
        return xCoord;
    }

    public int getRow()
    {
        return yCoord;
    }

    //Setter

    public void setOpenTop(bool newBool)
    {
        openTop = newBool;
    }

    public void setOpenRight(bool newBool)
    {
        openRight = newBool;
    }

    public void setOpenBot(bool newBool)
    {
        openBot = newBool;
    }

    public void setOpenLeft(bool newBool)
    {
        openLeft = newBool;
    }

    public void setGreen(bool newBool)
    {
        green = newBool;
    }
}
