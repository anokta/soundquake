﻿// ----------------------------------------------------------------------
//   soundquake - Ludum Dare 32 Compo Entry
//
//     Copyright 2015 Alper Gungormusler. All rights reserved.
//
// -----------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public static int level = 0;
    public static Transform levelTransform;
    float targetLevelY = 0.0f;

    public GameObject groundPrefab;
    public GameObject victimPrefab;

    NoiseManager noiseManager;
    VictimController victimController;

    float yThreshold = -5.0f;

    void Awake()
    {
        noiseManager = GetComponent<NoiseManager>();
    }

    void OnEnable()
    {
        GameEventManager.GameMenu += GameMenu;
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;
        GameEventManager.GameQuit += GameQuit;
    }

    void OnDisable()
    {
        GameEventManager.GameMenu -= GameMenu;
        GameEventManager.GameStart -= GameStart;
        GameEventManager.GameOver -= GameOver;
        GameEventManager.GameQuit -= GameQuit;
    }

    void Start()
    {
        GameEventManager.TriggerGameMenu();
    }

    void Update()
    {
        // Quit the game if ESC pressed.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameEventManager.TriggerGameQuit();
        }
        // Start the game if Space pressed.
        if (GameEventManager.CurrentState == GameEventManager.GameState.InMenu)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameEventManager.TriggerGameStart();
            }
        }
        else
        {
            if (levelTransform.position.y != targetLevelY)
            {
                levelTransform.position = Vector3.Lerp(levelTransform.position, new Vector3(levelTransform.position.x, targetLevelY, 0.0f), 2.0f * Time.deltaTime);
            }

            if (GameEventManager.CurrentState == GameEventManager.GameState.Running)
            {
                if (victimController.transform.position.y < yThreshold)
                {
                    GameEventManager.TriggerGameOver();
                }
            }
            else if (GameEventManager.CurrentState == GameEventManager.GameState.Over)
            {
                if (Mathf.Abs(levelTransform.position.y - targetLevelY) < 5.0f)
                {
                    GameObject.Destroy(levelTransform.gameObject);
                    GameEventManager.TriggerGameStart();
                }
            }
        }
    }

    void GameMenu()
    {
        noiseManager.enabled = false;
        Microphone.End(null);
        noiseManager.recording = false;

        // Create the victim.
        victimController = GameObject.Instantiate(victimPrefab).GetComponent<VictimController>();
        victimController.gameObject.SetActive(false);
    }

    void GameStart()
    {
        level++;
        InitLevel(level);
    }

    void GameOver()
    {
        noiseManager.enabled = false;
        Microphone.End(null);
        noiseManager.recording = false;

        targetLevelY = 20.0f;
    }

    void GameQuit()
    {
        Microphone.End(null);
    }

    void InitLevel(int level)
    {
        levelTransform = new GameObject("Level").transform;
        targetLevelY = 0.0f;

        // Get the screen borders.
        float left = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f)).x;
        float right = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, 0.0f)).x;
        float down = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f)).y;
        float up = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, Screen.height, 0.0f)).y;
        float middle = left;
        while (middle - left < 1.0f || right - middle < 1.0f || Mathf.Abs(middle - victimController.transform.position.x) < level)
            middle = Random.Range(left, right);

        // Create the ground floor.
        Transform groundLeft = GameObject.Instantiate(groundPrefab).transform;
        groundLeft.localPosition = new Vector3(left + (middle - left) / 2.0f, groundPrefab.transform.position.y, 0.0f);
        groundLeft.localScale = new Vector3(middle - left, groundPrefab.transform.localScale.y, 1.0f);
        groundLeft.parent = levelTransform;
        Transform groundRight = GameObject.Instantiate(groundPrefab).transform;
        groundRight.localPosition = new Vector3(middle + 1.0f + (right - middle - 1.0f) / 2.0f, groundPrefab.transform.position.y, 0.0f);
        groundRight.localScale = new Vector3(right - middle - 1.0f, groundPrefab.transform.localScale.y, 1.0f);
        groundRight.parent = levelTransform;
        // Create borders, so the victim cannot escape! :)
        Transform borderLeft = GameObject.Instantiate(groundPrefab).transform;
        borderLeft.localPosition = new Vector3(left - 0.5f * borderLeft.localScale.x, 0.0f, 0.0f);
        borderLeft.parent = levelTransform;
        Transform borderRight = GameObject.Instantiate(groundPrefab).transform;
        borderRight.localPosition = new Vector3(right + 0.5f * borderRight.localScale.x, 0.0f, 0.0f);
        borderRight.parent = levelTransform;

        // Start the level empty.
        levelTransform.position = new Vector3(0.0f, down - 0.5f * levelTransform.localScale.y, 0.0f);
        yThreshold = 0.5f * down;

        // Enable noise maker generation.
        noiseManager.enabled = true;

        // Reset the Victim.
        victimController.gameObject.SetActive(true);
        victimController.rigid.velocity = Vector3.zero;
        victimController.rigid.angularVelocity = Vector3.zero;
        victimController.transform.position = new Vector3(victimController.transform.position.x, up + 0.5f * victimController.transform.localScale.y, 0.0f);
    }
}