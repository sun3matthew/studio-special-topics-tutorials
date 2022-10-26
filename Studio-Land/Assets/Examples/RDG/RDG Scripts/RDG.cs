using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RDG : MonoBehaviour
{
    [SerializeField] private int numCrawlers = 2; //not single to make branching maps
    [SerializeField] private int minSteps = 6;
    [SerializeField] private int maxSteps = 12;

    private List<Vector2Int> roomCoords = new List<Vector2Int>(); //list of room coordinates
    public Queue<Vector2Int> roomsQueue = new Queue<Vector2Int>(); //queue of rooms to load, FIFO

    public Vector2Int endRoomCoord;
    private bool endRoomCreated;

    //concatenates room coordinates, cleans list by removing duplicate rooms etc.
    public List<Vector2Int> GenCoordList()
    {
        roomCoords = new List<Vector2Int>();
        for(int i = 0; i < numCrawlers; i++){
            int numSteps = numSteps = Random.Range(minSteps, maxSteps + 1);
            Vector2Int currPos = Vector2Int.zero;
            for(int j = 0; j < numSteps; j++){
                int randDir = Random.Range(0, 2);
                if(randDir == 0)
                    currPos += new Vector2Int(0, Random.Range(0, 2) == 0 ? -1 : 1);
                else
                    currPos += new Vector2Int(Random.Range(0, 2) == 0 ? -1 : 1, 0);
                if(currPos != Vector2Int.zero && !hasPos(currPos))
                    roomCoords.Add(currPos);
            }
        }
        Debug.Log(roomCoords);
        return roomCoords;
    }
    private bool hasPos(Vector2Int pos){
        for(int i = 0; i < roomCoords.Count; i++)
            if(roomCoords[i] == pos)
                return true;
        return false;
    }

    //converts list into queue, finds the end room
    public void addRoomsToQueue()
    {
        roomsQueue.Clear();

        roomsQueue.Enqueue(Vector2Int.zero); //makes sure start room is loaded first
        foreach (Vector2Int pos in roomCoords)
        {
            roomsQueue.Enqueue(pos);
            if (pos == roomCoords[roomCoords.Count - 1] && pos != Vector2Int.zero && !endRoomCreated)
            {
                endRoomCoord = pos;
                endRoomCreated = true;
            }
        }
    }

    void Start()
    {
        roomCoords = GenCoordList();
        addRoomsToQueue();
    }
}

