using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour {

    public GameObject winUI;
    public GameObject loseUI;
    bool isGameOver;

	// Use this for initialization
	void Start () {
        Guard.PlayerSpotted += showGameLoseUI;
	}
	
	// Update is called once per frame
	void Update () {
		if (isGameOver && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }
	}

    void showGameWinUI()
    {
        ShowGameUI(winUI);
    }

    void showGameLoseUI()
    {
        ShowGameUI(loseUI);
    }

    void ShowGameUI(GameObject ui)
    {
        ui.SetActive(true);
        isGameOver = true;
        Guard.PlayerSpotted -= showGameLoseUI;
    }
}
