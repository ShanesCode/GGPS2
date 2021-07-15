using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public static GameManager gameManager;

    public static int spawnRoom;

    public List<Achievement> achievements = new List<Achievement>()
    {
        new Achievement(0, "King Chugger", "Drink 50 times.", "Not one bathroom break needed, the sign of a true king.", null, true),
        new Achievement(1, "Standup Citizen", "Recycle 10 times.", "The council still has every right to harass you, but you'll still take the moral high ground.")
    };

    public void SetSpawnRoom(int roomNumber)
    {
        spawnRoom = roomNumber;
    }
}
