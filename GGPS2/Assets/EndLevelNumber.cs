using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndLevelNumber : MonoBehaviour
{
    GameObject gameManager;
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager");
        text = GetComponent<TextMeshProUGUI>();
        text.text = gameManager.GetComponent<GameManager>().GetWasteCount().ToString();
    }
}
