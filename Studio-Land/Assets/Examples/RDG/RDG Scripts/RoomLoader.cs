using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//for loading scenes
using UnityEngine.SceneManagement;

public class RoomLoader : MonoBehaviour
{
    //singleton manager
    public static RoomLoader instance { get; private set; }

    private void Awake()
    {
        //instatiating singleton instance
        if (instance == null)
        {
            Debug.Log("room loader instance created");
            instance = this; //sets it to this instance if script is running for the first time
            DontDestroyOnLoad(gameObject);
            rdg = GetComponent<RDG>();
        }
        else
        {
            Destroy(gameObject); //keeps only the first instance
        }
    }

    [SerializeField] private List<Room> loadedRooms = new List<Room>();

    private bool isLoadingRoom = false;
    private bool doorsSet = false;

    private Vector2Int currLoadRoomPos;
    private string currLoadRoomType;
    private RDG rdg;

    void Update()
    {
        if(!isLoadingRoom){
            if(rdg.roomsQueue.Count != 0){
                currLoadRoomPos = rdg.roomsQueue.Dequeue();
                isLoadingRoom = true;
                StartCoroutine(LoadRoom(currLoadRoomPos));
                Debug.Log("loading " + currLoadRoomType);
            }else if(!doorsSet){
                foreach(Room room in loadedRooms)
                    room.SetDoors();
                doorsSet = true;

            }
        }
    }

    //async operation to load rooms additively (scenes)
    IEnumerator LoadRoom(Vector2Int pos)
    {   
        currLoadRoomType = getRoomName(pos);
        AsyncOperation loadRoomOp = SceneManager.LoadSceneAsync(currLoadRoomType, LoadSceneMode.Additive);
        while(!loadRoomOp.isDone)
            yield return null;
    }
    private string getRoomName(Vector2Int pos){
        if(pos == Vector2Int.zero)
            return "Start";
        if(pos == rdg.endRoomCoord)
            return "End";
        return "start";
    }

    public bool IsCoordEmpty(Vector2Int coord) => loadedRooms.Find(room => room.X == coord.x && room.Y == coord.y) == null;

    //positions room correctly, adds room to list of loaded rooms
    //called within room class (attached to each room game object)
    //serves as a "constructor" for the rooms (since monobehavior scripts cannot have constructors)
    public void PositionRoom(Room room)
    {
        if(IsCoordEmpty(currLoadRoomPos)){
            room.transform.position = new Vector2(currLoadRoomPos.x * Room.width, currLoadRoomPos.y * Room.height);
            room.transform.SetParent(transform);

            room.X = currLoadRoomPos.x;
            room.Y = currLoadRoomPos.y;
            loadedRooms.Add(room);
        }else{
            GameObject.Destroy(room.gameObject);
        }

        SceneManager.MergeScenes(SceneManager.GetSceneByName(currLoadRoomType), SceneManager.GetSceneByName("RDG"));
        isLoadingRoom = false;
    }

    public List<Room> getRooms(){
        return loadedRooms;
    }

}
