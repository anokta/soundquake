// ----------------------------------------------------------------------
//   soundquake - Ludum Dare 32 Compo Entry
//
//     Copyright 2015 Alper Gungormusler. All rights reserved.
//
// -----------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GuiManager : MonoBehaviour
{
    public Text title, click, level;

    float currentAlphaMenu, targetAlphaMenu;
    float currentAlphaLevel, targetAlphaLevel;

    void OnEnable()
    {
        GameEventManager.GameMenu += GameMenu;
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;
    }

    void OnDisable()
    {
        GameEventManager.GameMenu -= GameMenu;
        GameEventManager.GameStart -= GameStart;
        GameEventManager.GameOver -= GameOver;
    }

    void Awake()
    {
        currentAlphaMenu = targetAlphaMenu = 0.0f;
        currentAlphaLevel = targetAlphaLevel = 0.0f;
    }

    void Update()
    {
        if (currentAlphaMenu != targetAlphaMenu)
        {
            currentAlphaMenu = Mathf.Lerp(currentAlphaMenu, targetAlphaMenu, 4.0f * Time.deltaTime);
        }
        if (currentAlphaLevel != targetAlphaLevel)
        {
            currentAlphaLevel = Mathf.Lerp(currentAlphaLevel, targetAlphaLevel, 2.0f * Time.deltaTime);
        }

        title.rectTransform.localPosition = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0.0f);
        click.rectTransform.localPosition = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0.0f);
        level.rectTransform.localPosition = new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.4f, 0.4f), 0.0f);

        Color c;

        c = level.color;
        c.a = currentAlphaLevel;
        level.color = c;

        c = title.color;
        c.a = currentAlphaMenu;
        title.color = c;

        if (GameEventManager.CurrentState == GameEventManager.GameState.InMenu)
        {
            click.color = c;

            if (Input.GetMouseButtonDown(0))
            {
                click.fontStyle = FontStyle.Bold;
            }
            if (Input.GetMouseButtonUp(0))
            {
                click.enabled = false;
                GameEventManager.TriggerGameStart();
            }
        }
    }

    void GameMenu()
    {
        targetAlphaMenu = 1.0f;
    }

    void GameStart()
    {
        targetAlphaMenu = 0.0f;
        targetAlphaLevel = 0.0f;

        level.text = "floor " + (1 - GameManager.level);
    }

    void GameOver()
    {
        currentAlphaLevel = 0.1f;
        targetAlphaLevel = 1.0f;
    }
}
