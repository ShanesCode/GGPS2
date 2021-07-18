using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndBottle : MonoBehaviour
{
    public GameObject label;
    public GameObject labelText;
    public List<string> firstWords;
    public List<string> secondWords;

    void Start()
    {
        RandomiseBottleColours();
        RandomiseBottleWords();
    }

    void RandomiseBottleColours()
    {
        float bottleRed = UnityEngine.Random.Range(0.0f, 1.0f);
        float bottleGreen = UnityEngine.Random.Range(0.0f, 1.0f);
        float bottleBlue = UnityEngine.Random.Range(0.0f, 1.0f);

        gameObject.GetComponent<SpriteRenderer>().color = new Color(bottleRed, bottleGreen, bottleBlue);

        float labelRed = 1.0f - bottleRed;
        float labelGreen = 1.0f - bottleGreen;
        float labelBlue = 1.0f - bottleBlue;

        label.GetComponent<SpriteRenderer>().color = new Color(labelRed, labelGreen, labelBlue);
    }

    void RandomiseBottleWords()
    {
        int firstWord = UnityEngine.Random.Range(0, firstWords.Count);
        int secondWord = UnityEngine.Random.Range(0, secondWords.Count);

        labelText.GetComponent<TextMeshPro>().text = firstWords[firstWord] + '\n' + secondWords[secondWord];
    }
}
