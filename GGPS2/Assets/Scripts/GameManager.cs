using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int spawnRoom;

    public List<Achievement> achievements = new List<Achievement>();
    
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    private void Start()
    {
        spawnRoom = 0;
    }

    public void SetSpawnRoom(int roomNumber)
    {
        spawnRoom = roomNumber;
    }
}
