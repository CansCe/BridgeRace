using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public Material[] material;
    public int color;

    public int getColor()
    {
        return color;
    }
    public void SetColor(int c)
    {
        color = c;
        GetComponent<Renderer>().material = material[c];
    }

    public int GetOwnIndex()
    {
        return transform.GetSiblingIndex();
    }
}
