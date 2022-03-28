using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCamera : MonoBehaviour
{
    public float timeToTarget;
    public Vector3 offset;

    private GameManager gm;
    private float xVel;
    private float zVel;

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        Vector3 worldPos = gm.GetRoomWorldPosition() + offset;
        float newX = Mathf.SmoothDamp(this.transform.position.x, worldPos.x, ref xVel, timeToTarget);
        float newZ = Mathf.SmoothDamp(this.transform.position.z, worldPos.z, ref zVel, timeToTarget);
        transform.position = new Vector3(newX, this.transform.position.y, newZ);
    }
}
