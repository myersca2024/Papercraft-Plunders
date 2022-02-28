using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static bool inCombat = false;
    public static bool isGameOver = false;

    GameObject[] doors;
    GameObject[] enemies;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LateStart(.5f));
        inCombat = true;
    }

    IEnumerator LateStart(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        doors = GameObject.FindGameObjectsWithTag("Door");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject door in doors)
        {
            door.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnemies();

        //Debug.Log(enemies.Length);
    }

    void CheckEnemies()
    {

        GameObject[] curEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (curEnemies.Length == 0)
        {
            OpenDoors();
        }
    }

    public void OpenDoors() 
    {
        inCombat = false;
        foreach (GameObject door in doors) 
        {
            door.SetActive(true);
        }
    }


    public void Restart() 
    {
        inCombat = false;
        isGameOver = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
