using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorBehavior : MonoBehaviour
{
    public string nextRoom;


    bool doorOpen = false;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position == transform.position) 
        {
            SceneManager.LoadScene(nextRoom, LoadSceneMode.Single);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.isGameOver) 
        {
            //SceneManager.LoadScene(nextRoom, LoadSceneMode.Additive);
        }
    }
}
