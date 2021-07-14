using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Achievement
{
    public int id;
    public string title;
    public string requirement;
    public string flavourText;
    public Sprite sprite;
    public bool achieved = false;
}
