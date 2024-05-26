using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int row = 0;
    public int column = 0;
    public GameObject controller;


    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
    }

    public void MoveArrow(int x, int y, int rotation)
    {
        switch (x)
        {
            case -1:
                row = 0;
                column = y;
                break;
            case 7:
                row = 6;
                column = y;
                break;

            default:
                switch (y)
                {
                    case -1:
                        row = x;
                        column = 0;
                        break;
                    case 7:
                        row = x;
                        column = 6;
                        break;
                    default:
                        break;
                }
                break;
        }

        float calculatedX = x * 3.34f;
        calculatedX -= 12f;
        float calculatedY = y * -3.34f;
        calculatedY += 10f;

        this.transform.position = new Vector3(calculatedX, calculatedY, -0.1f);
        this.transform.Rotate(0, 0, rotation * -90);
    }
}
