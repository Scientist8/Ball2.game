using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockController : MonoBehaviour
{
    private int _count;

    public Text _countText;

    private AudioSource _bounceSound;

    private GameController _gameController;

    void Awake()
    {
        _bounceSound = GameObject.Find("BounceSound").GetComponent<AudioSource>();
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }


    void Update()
    {
        if (transform.position.y <= -10)
        {
            _gameController._score += 6;
            Destroy(gameObject);
        }
    }

    public void SetStartingCount(int count)
    {
        this._count = count;
        _countText.text = _count.ToString();
    }

    private void OnCollisionEnter2D(Collision2D target)
    {
        if(target.collider.name == "Ball" && _count > 0)
        {
            _count--;
            Camera.main.GetComponent<CameraController>().SmallShake();
            _countText.text = _count.ToString();
            _bounceSound.Play();
            if (_count == 0)
            {
                Destroy(gameObject);
                _gameController._score += 4;
                Camera.main.GetComponent<CameraController>().MediumShake();
                GameObject.Find("ExtraBallProgress").GetComponent<Progress>().IncreaseCurrentWidth();
            }
        }
    }
}
