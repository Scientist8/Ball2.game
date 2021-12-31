using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private ShotCountText _shotCountText;

    public Text _ballsCountText;

    public GameObject[] _blocks;

    public List<GameObject> _levels;

    private GameObject _level1;
    private GameObject _level2;

    private Vector2 _level1Pos;
    private Vector2 _level2Pos;

    public int _shotCount;
    public int _ballsCount;
    public int _score;

    private GameObject _ballsContainer;
    public GameObject _gameOver;

    private bool _firstShot = true;

    void Awake()
    {
        _shotCountText = GameObject.Find("ShotCountText").GetComponent<ShotCountText>();
        _ballsCountText = GameObject.Find("BallCountText").GetComponent<Text>();
        _ballsContainer = GameObject.Find("BallsContainer");
    }

    void Start()
    {

        _ballsCount = PlayerPrefs.GetInt("BallsCount", 5);
        _ballsCountText.text = _ballsCount.ToString();

        Physics2D.gravity = new Vector2(0, -17);

        SpawnLevel();
        GameObject.Find("Cannon").GetComponent<Animator>().SetBool("MoveIn", true);
    }

    void Update()
    {
        if (_ballsContainer.transform.childCount == 0 && _shotCount == 4)
        {
            _gameOver.SetActive(true);
            GameObject.Find("Cannon").GetComponent<Animator>().SetBool("MoveIn", false);
        }

        if (_shotCount > 2)
            _firstShot = false;
        else
            _firstShot = true;

        CheckBlocks();
    }

    void SpawnNewLevel(int numberLevel1, int numberLevel2, int min, int max)
    {
        if(_shotCount > 1)
        {
            Camera.main.GetComponent<CameraController>().RotateCameraToFront();
        }

        _shotCount = 1;

        _level1Pos = new Vector2(3.5f, 1);
        _level2Pos = new Vector2(3.5f, -3.4f);

        _level1 = _levels[numberLevel1];
        _level2 = _levels[numberLevel2];

        Instantiate(_level1, _level1Pos, Quaternion.identity);
        Instantiate(_level2, _level2Pos, Quaternion.identity);

        SetBlockCount(min, max);

        RemoveBalls();

    }

    void SpawnLevel()
    {
        if (PlayerPrefs.GetInt("Level", 0) == 0)
            SpawnNewLevel(0, 17, 3, 5);

        if (PlayerPrefs.GetInt("Level") == 1)
            SpawnNewLevel(1, 18, 3, 5);

        if (PlayerPrefs.GetInt("Level") == 2)
            SpawnNewLevel(2, 19, 3, 6);

        if (PlayerPrefs.GetInt("Level") == 3)
            SpawnNewLevel(5, 20, 4, 7);

        if (PlayerPrefs.GetInt("Level") == 4)
            SpawnNewLevel(12, 28, 5, 8);

        if (PlayerPrefs.GetInt("Level") == 5)
            SpawnNewLevel(14, 29, 7, 10);

        if (PlayerPrefs.GetInt("Level") == 6)
            SpawnNewLevel(15, 30, 6, 12);

        if (PlayerPrefs.GetInt("Level") == 7)
            SpawnNewLevel(16, 31, 9, 15);

    }

    void SetBlockCount(int min, int max)
    {
        _blocks = GameObject.FindGameObjectsWithTag("Block");

        for (int i = 0; i < _blocks.Length; i++)
        {
            int count = Random.Range(min, max);
            _blocks[i].GetComponent<BlockController>().SetStartingCount(count);
        }
    }

    public void CheckBlocks()
    {
        _blocks = GameObject.FindGameObjectsWithTag("Block");

        if (_blocks.Length < 1)
        {
            RemoveBalls();
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
            SpawnLevel();

            if (_ballsCount >= PlayerPrefs.GetInt("BallsCount", 5))
                PlayerPrefs.SetInt("BallsCount", _ballsCount);

            if (_firstShot)
                _score += 50;
            else
                _score += 30;
        }
    }

    void RemoveBalls()
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");

        for (int i = 0; i < balls.Length; i++)
        {
            Destroy(balls[i]);
        }
    }

    public void CheckShotCount()
    {
        if (_shotCount == 1)
        {
            _shotCountText.SetTopText("SHOTS");
            _shotCountText.SetBottomText("1/3");
            _shotCountText.Flash();
        }
        if (_shotCount == 2)
        {
            _shotCountText.SetTopText("SHOTS");
            _shotCountText.SetBottomText("2/3");
            _shotCountText.Flash();
        }
        if (_shotCount == 3)
        {
            _shotCountText.SetTopText("FINAL");
            _shotCountText.SetBottomText("SHOT");
            _shotCountText.Flash();
        }
    }
}
