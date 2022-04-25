using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCamera : MonoBehaviour
{
    public float timeToTarget;
    public Vector3 offset;
    public Vector3 zoomOutOffset;
    public float zoomSpeed;
    public CanvasGroup ui;

    private GameManager gm;
    private float xVel;
    private float yVel;
    private float startingY;
    private float zVel;

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        startingY = this.transform.position.y;
    }

    void Update()
    {
        if (Time.timeScale == 0) {
            return;
        }

        Vector3 worldPos = gm.GetRoomWorldPosition() + offset;
        float newX;
        float newZ;
        float newY;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            newX = Mathf.SmoothDamp(this.transform.position.x, zoomOutOffset.x, ref xVel, timeToTarget / zoomSpeed);
            newZ = Mathf.SmoothDamp(this.transform.position.z, zoomOutOffset.z + worldPos.z / 2, ref zVel, timeToTarget / zoomSpeed);
            newY = Mathf.SmoothDamp(this.transform.position.y, zoomOutOffset.y, ref yVel, timeToTarget / zoomSpeed);
            ui.alpha = 0.3f;
        }
        else
        {
            newX = Mathf.SmoothDamp(this.transform.position.x, worldPos.x, ref xVel, timeToTarget / zoomSpeed);
            newZ = Mathf.SmoothDamp(this.transform.position.z, worldPos.z, ref zVel, timeToTarget / zoomSpeed);
            newY = Mathf.SmoothDamp(this.transform.position.y, startingY, ref yVel, timeToTarget / zoomSpeed);
            ui.alpha = 1f;
        }
        transform.position = new Vector3(newX, newY, newZ);
    }
}
