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

    public Achievement(int id_, string title_, string requirement_ = "Requirement", string flavourText_ = "Flavour Text", Sprite sprite_ = null, bool achieved_ = false)
    {
        id = id_;
        title = title_;
        requirement = requirement_;
        flavourText = flavourText_;
        sprite = sprite_;
        achieved = achieved_;
    }
}
