using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject _pauseMenu;

    public Text _scoreText;
    public Text _bestScoreText;

    private GameController _gameController;

    private void Awake()
    {
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    void Update()
    {
        _scoreText.text = _gameController._score.ToString();

        if (_gameController._score > PlayerPrefs.GetInt("BestScore", 0))
            PlayerPrefs.SetInt("BestScore", _gameController._score);

        _bestScoreText.text = "Best: " + PlayerPrefs.GetInt("BestScore");
    }

    public void TryAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Pause()
    {
        Time.timeScale = 0;
        _pauseMenu.gameObject.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        _pauseMenu.gameObject.SetActive(false);
    }
}
