using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pausemenuGO;
    public GameManager gameManager;

    void Start()
    {
        if(!gameManager)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.TogglePause();
            pausemenuGO.SetActive(gameManager.GetPauseState());
        }

    }
}
