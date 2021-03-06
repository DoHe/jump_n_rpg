﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static bool gameRunning = true;
    public Text endMessage;

    private GameObject player;
    private GameObject curtain = null;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartLevel();
    }

    private GameObject SpawnCurtain(GameObject player)
    {
        Vector3 curtainPosition = player.transform.position;
        curtainPosition.z = -50;
        curtainPosition.x = curtainPosition.x + 3.6f;
        return (GameObject)Instantiate(Resources.Load("Curtain"), curtainPosition, Quaternion.identity);
    }

    private void Update()
    {
        if (curtain != null)
        {
            Vector3 curtainPosition = curtain.transform.position;
            curtainPosition.x = player.transform.position.x + 3.6f;
            curtainPosition.y = player.transform.position.y;
            curtain.transform.position = curtainPosition;
        }
    }

    void StartLevel()
    {
        curtain = SpawnCurtain(player);
        StartCoroutine("LevelStarted");
    }

    public IEnumerator LevelStarted()
    {
        yield return new WaitForSeconds(2.3f);
        Destroy(curtain);
        curtain = null;
        yield return null;
    }

    public void EndLevel(GameObject player, bool won)
    {
        gameRunning = false;
        curtain = SpawnCurtain(player);
        curtain.GetComponent<Animator>().SetTrigger("Close");
        if (!won)
        {
            endMessage.text = "You lost :(";
        }
        endMessage.enabled = true;
        StartCoroutine("LoadNextLevel");
    }

    public IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        gameRunning = true;
        yield return null;
    }
}
