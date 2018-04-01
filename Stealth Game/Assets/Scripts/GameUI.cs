using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour {

    public GameObject winUI;
    public GameObject loseUI;
    bool isGameOver;

	void Start ()
    {
        Guard.PlayerSpotted += showGameLoseUI;
        FindObjectOfType<PlayerController> ().OnReachingEnd += showGameWinUI;
	}
	
	void Update ()
    {
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
