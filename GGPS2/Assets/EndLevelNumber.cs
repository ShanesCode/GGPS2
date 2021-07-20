using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndLevelNumber : MonoBehaviour
{
    GameObject gameManager;
    public TextMeshPro text;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager");
        text = GetComponent<TextMeshPro>();
        text.text = (gameManager.GetComponent<GameManager>().GetWasteCount() - gameManager.GetComponent<GameManager>().GetRecycleCount()).ToString();
    }
}
