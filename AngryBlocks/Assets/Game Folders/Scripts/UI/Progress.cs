using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress : MonoBehaviour
{
    public RectTransform _extraBallInner;

    private GameController _gameController;

    private float _currentWidth, _addWidth, _totalWidth;
    void Awake()
    {
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    void Start()
    {
        _extraBallInner.sizeDelta = new Vector2(31, 117);
        _currentWidth = 31;
        _totalWidth = 600;
    }

    void Update()
    {
        if (_currentWidth >= _totalWidth)
        {
            _gameController._ballsCount++;
            _gameController._ballsCountText.text = _gameController._ballsCount.ToString();
            _currentWidth = 31;
        }

        if (_currentWidth >= _addWidth)
        {
            _addWidth += 5;
            _extraBallInner.sizeDelta = new Vector2(_addWidth, 117);
        }
        else
            _addWidth = _currentWidth;
    }

    public void IncreaseCurrentWidth()
    {
        int addRandom = Random.Range(80, 120);
        _currentWidth = addRandom + 31 + _currentWidth % 576f;
    }
}
