using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureCard : MonoBehaviour
{
    public GameObject controller;
    public Sprite Anvil, Battery, Cham, Cola, Krombacher, RubicsCube,
        Coffe, Ducktape, GameCube, Gollum, KnightGold,
        KnightRed, Mario, Monitor, Pokeball, Police,
        Raspb, Remote, Screw, Skull, Smartphone, Snes,
        Sword, Walter, ReturnToStart;

    public string treasure;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        switch (this.name)
        {
            case "Anvil":
                treasure = "Anvil";
                this.GetComponent<SpriteRenderer>().sprite = Anvil;
                break;
            case "Battery":
                treasure = "Battery";
                this.GetComponent<SpriteRenderer>().sprite = Battery;
                break;
            case "Cham":
                treasure = "Cham";
                this.GetComponent<SpriteRenderer>().sprite = Cham;
                break;
            case "Cola":
                treasure = "Cola";
                this.GetComponent<SpriteRenderer>().sprite = Cola;
                break;
            case "Krombacher":
                treasure = "Krombacher";
                this.GetComponent<SpriteRenderer>().sprite = Krombacher;
                break;
            case "RubicsCube":
                treasure = "RubicsCube";
                this.GetComponent<SpriteRenderer>().sprite = RubicsCube;
                break;
            case "Coffe":
                treasure = "Coffe";
                this.GetComponent<SpriteRenderer>().sprite = Coffe;
                break;
            case "Ducktape":
                treasure = "Ducktape";
                this.GetComponent<SpriteRenderer>().sprite = Ducktape;
                break;
            case "GameCube":
                treasure = "GameCube";
                this.GetComponent<SpriteRenderer>().sprite = GameCube;
                break;
            case "Gollum":
                treasure = "Gollum";
                this.GetComponent<SpriteRenderer>().sprite = Gollum;
                break;
            case "KnightGold":
                treasure = "KnightGold";
                this.GetComponent<SpriteRenderer>().sprite = KnightGold;
                break;
            case "KnightRed":
                treasure = "KnightRed";
                this.GetComponent<SpriteRenderer>().sprite = KnightRed;
                break;
            case "Mario":
                treasure = "Mario";
                this.GetComponent<SpriteRenderer>().sprite = Mario;
                break;
            case "Monitor":
                treasure = "Monitor";
                this.GetComponent<SpriteRenderer>().sprite = Monitor;
                break;
            case "Pokeball":
                treasure = "Pokeball";
                this.GetComponent<SpriteRenderer>().sprite = Pokeball;
                break;
            case "Police":
                treasure = "Police";
                this.GetComponent<SpriteRenderer>().sprite = Police;
                break;
            case "Raspb":
                treasure = "Raspb";
                this.GetComponent<SpriteRenderer>().sprite = Raspb;
                break;
            case "Skull":
                treasure = "Skull";
                this.GetComponent<SpriteRenderer>().sprite = Skull;
                break;
            case "Remote":
                treasure = "Remote";
                this.GetComponent<SpriteRenderer>().sprite = Remote;
                break;
            case "Screw":
                treasure = "Screw";
                this.GetComponent<SpriteRenderer>().sprite = Screw;
                break;
            case "Smartphone":
                treasure = "Smartphone";
                this.GetComponent<SpriteRenderer>().sprite = Smartphone;
                break;
            case "Snes":
                treasure = "Snes";
                this.GetComponent<SpriteRenderer>().sprite = Snes;
                break;
            case "Sword":
                treasure = "Sword";
                this.GetComponent<SpriteRenderer>().sprite = Sword;
                break;
            case "Walter":
                treasure = "Walter";
                this.GetComponent<SpriteRenderer>().sprite = Walter;
                break;
                 case "ReturnToStart":
                treasure = "ReturnToStart";
                this.GetComponent<SpriteRenderer>().sprite = ReturnToStart;
                break;

            default:
                break;
        }
    }
    public void MoveCard(int player)
    {
        if (0 > player || 3 < player)
        {
            hideCard();
            return;
        }
          
        switch (player)
        {
            //topLeft
            case 0:
                this.transform.position = new Vector3(999, 999, -0.1f);
                break;
                //topRight
            case 1:
                this.transform.position = new Vector3(999, 999, -0.1f);
                break;
                //botLeft
            case 2:
                this.transform.position = new Vector3(999, 999, -0.1f);
                break;
            //botRight
            case 3:
                this.transform.position = new Vector3(999, 999, -0.1f);
                break;

            default:
                break;
        }
        
    }

    public void hideCard()
    {
        this.transform.position = new Vector3(999, 999, -0.1f);
    }

    public void showCard()
    {
        this.transform.position = new Vector3(17.68f, 0.19f, -0.1f);
    }
        
}
