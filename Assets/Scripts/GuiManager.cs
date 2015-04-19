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
    public Text title, click;

    float currentAlphaMenu, targetAlphaMenu;

    void OnEnable()
    {
        GameEventManager.GameMenu += GameMenu;
        GameEventManager.GameStart += GameStart;
    }

    void OnDisable()
    {
        GameEventManager.GameMenu -= GameMenu;
        GameEventManager.GameStart -= GameStart;
    }

    void Awake()
    {
        currentAlphaMenu = targetAlphaMenu = 0.0f;
    }

    void Update()
    {
        if (currentAlphaMenu != targetAlphaMenu)
        {
            currentAlphaMenu = Mathf.Lerp(currentAlphaMenu, targetAlphaMenu, 4.0f * Time.deltaTime);
        }

        Color c = title.color;
        c.a = currentAlphaMenu;
        title.color = c;

        if (GameEventManager.CurrentState == GameEventManager.GameState.InMenu)
        {
            click.color = c;

            if (Input.GetMouseButtonDown(0))
            {
                click.fontStyle = FontStyle.Bold;
            }
            if (Input.GetMouseButtonUp(0)) {
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
    }
}
