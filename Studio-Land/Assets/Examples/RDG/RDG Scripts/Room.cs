using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //Room size
    public const float width = 15.7f;
    public const float height = 8.7f;

    //Room position in grid
    public int X;
    public int Y;

    //array of doors
    Door[] doors;

    void Start()
    {
        if(RoomLoader.instance == null)
        {
            Debug.Log("wrong scene, press play in the main scene");
            return; //no room controller instance created yet
        }

        RoomLoader.instance.PositionRoom(this);

        doors = GetComponentsInChildren<Door>();

    }

    public void SetDoors()
    {
        for(int i = 0; i < doors.Length; i++){
            Vector2Int offset;
            switch(doors[i].type){
                case Door.Type.top:
                    offset = new Vector2Int(0, 1);
                    break;
                case Door.Type.bot:
                    offset = new Vector2Int(0, -1);
                    break;
                case Door.Type.left:
                    offset = new Vector2Int(-1, 0);
                    break;
                default:
                    offset = new Vector2Int(1, 0);
                    break;
            }
            if(!RoomLoader.instance.IsCoordEmpty(new Vector2Int(X + offset.x, Y + offset.y)))
                Open(doors[i]);
        }
    }

    public void Open(Door d)
    {
        d.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        d.openTile();
    }

    public void OnTriggerStay2D(Collider2D collider)
    {
        Debug.Log("??");
        if(collider.tag == "player")
            CameraController.instance.currRoom = this;
    }

}
