using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTurn : MonoBehaviour
{
    public Sprite turnSprite;
    public GameObject controller;
    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
    }

    public void placeArrow()
    {
        this.transform.position = new Vector3(-22, 10, -0.1f);
    }
}
