using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraController : MonoBehaviour
{

    public Room currRoom;
    public float camSpeed;

    [SerializeField] Camera camera;
    [SerializeField] RenderPipelineAsset renderer2D;

    public static CameraController instance { get; private set; }

    void Awake()
    {
        if (instance == null)
        {
            Debug.Log("camera created");
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Update()
    {
        MoveCam();
    }

    private void MoveCam()
    {
        if (currRoom == null)
            return;

        Vector2 pos = new Vector3(currRoom.X * Room.width, currRoom.Y * Room.height, transform.position.z);
        Debug.Log(pos);
        transform.position = Vector3.MoveTowards(transform.position, pos, camSpeed * Time.deltaTime);
    }

    
}
