using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int levelNumber;

    [HideInInspector] public List<List<Room>> floors = new List<List<Room>>();
    private const int MIN_NUMBER_OF_STARTING_PATHS = 3;
    private const int MAX_NUMBER_OF_STARTING_PATHS = 5;
    private int noOfStartingPaths;

    private int noOfFloors;
    private const int MIN_NUMBER_OF_FLOORS = 6;
    private const int NUMBER_OF_FLOORS_INCREMENT = 3;
    // Start is called before the first frame update
    void Start()
    {
        noOfStartingPaths = Random.Range(MIN_NUMBER_OF_STARTING_PATHS, MAX_NUMBER_OF_STARTING_PATHS + 1);
        noOfFloors = MIN_NUMBER_OF_FLOORS + levelNumber * NUMBER_OF_FLOORS_INCREMENT;
        GeneratePaths();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GenerateStartingRooms()
    {
        List<Room> rooms = new List<Room>();
        
        for (int i = 0; i < noOfStartingPaths; i++)
        {
            Room room = new Room();
            room.floor = 0;
            room.pathIndex = i;
            rooms.Add(room);
        }

        floors.Add(rooms);
    }

    void GenerateNextFloor(int floor)
    {
        List<Room> rooms = new List<Room>();

        for (int i = 0; i < floors[floor - 1].Count; i++)
        {
            Room precedingRoom = floors[floor - 1][i];
            // Choose whether to connect to left, forward, right, or any combination of those
            // Prevent left/right paths being taken if already on left/rightmost path
            int decision;
            if (precedingRoom.pathIndex == 0)
            {
                decision = Random.Range(0, 3);
                switch (decision)
                {
                    case 0:
                        // Connect straight
                        break;
                    case 1:
                        // Connect right
                        break;
                    case 2:
                        // Connect straight and right
                        break;
                }
            }
            else if (precedingRoom.pathIndex == noOfStartingPaths - 1)
            {
                decision = Random.Range(0, 3);
                switch (decision)
                {
                    case 0:
                        // Connect left
                        break;
                    case 1:
                        // Connect straight
                        break;
                    case 2:
                        // Connect left and straight
                        break;
                }
            } 
            else
            {
                decision = Random.Range(0, 7);
                switch (decision)
                {
                    case 0:
                        // Connect left
                        break;
                    case 1:
                        // Connect straight
                        break;
                    case 2:
                        // Connect right
                        break;
                    case 3:
                        // Connect left and straight
                        break;
                    case 4:
                        // Connect left and right
                        break;
                    case 5:
                        // Connect straight and right
                        break;
                    case 6:
                        // Connect left, straight and right
                        break;
                }
            }

            // Create a new room for each room in previous floor
            Room room = new Room();
            room.floor = floor;

            // Update the following rooms for the ones that preceded them
            floors[floor - 1][i].followingRooms.Add(room);

            // Add the new rooms to the new floor
            rooms.Add(room);
        }

        // Add the new floor to the list of floors
        floors.Add(rooms);
    }

    void GenerateFinalFloor()
    {
        List<Room> rooms = new List<Room>();
        Room finalRoom = new Room();
        finalRoom.floor = noOfFloors - 1;

        for (int i = 0; i < floors[noOfFloors - 2].Count; i++)
        {
            floors[noOfFloors - 2][i].followingRooms.Add(finalRoom);
        }

        rooms.Add(finalRoom);
        floors.Add(rooms);
    }

    void GeneratePaths()
    {
        for(int i = 0; i < noOfFloors; i++)
        {
            if (i == 0) 
            { 
                GenerateStartingRooms(); 
            }
            else if (i != 0 && i < noOfFloors - 1)
            { 
                GenerateNextFloor(i); 
            }
            else
            {
                GenerateFinalFloor();
            }
        }
    }
}
