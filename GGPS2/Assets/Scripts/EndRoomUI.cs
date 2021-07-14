using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndRoomUI : MonoBehaviour
{
    public TextMeshProUGUI stats;
    public int wasteCount;
    public int indulgenceCount;
    public int recycleCount;
    public int devCount;

    public TextMeshProUGUI title;
    public int levelNumber;
    public int roomNumber;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        stats.text =
            "Dumped litter:\t\t\t\t" + wasteCount + '\t' + " times" + '\n' +
            "Indulged gluttonously:\t\t" + indulgenceCount + '\t' + " times" + '\n' +
            "Thoughtfully recycled:\t" + recycleCount + '\t' + " times" + '\n' + '\n' +
            "Dev's best:\t\t\t\t\t" + devCount + '\t' + " bottles" + '\n';

        title.text = "Room " + levelNumber + "-" + roomNumber + " Complete!";
    }
}
