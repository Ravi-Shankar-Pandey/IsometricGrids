using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCubeInfo : MonoBehaviour
{
    public int x;
    public int y;

    public void SetPosition(int xPos, int yPos)
    {
        x = xPos;
        y = yPos;
    }
}
