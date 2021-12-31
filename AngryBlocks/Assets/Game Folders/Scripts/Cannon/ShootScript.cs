using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootScript : MonoBehaviour
{
    private GameController _gameController;
    [SerializeField] private float _power = 2;

    private Vector2 _startPos;

    private bool _shoot, _aiming;

    private GameObject _Dots;
    private List<GameObject> _projectilePath;

    private Rigidbody2D _ballBody;

    public GameObject _ballPrefab;
    public GameObject _ballsContainer;

    void Awake()
    {
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
        _Dots = GameObject.Find("Dots");
    }

    void Start()
    {
        _projectilePath = _Dots.transform.Cast<Transform>().ToList().ConvertAll(t => t.gameObject);
        HideDots();
    }

    void Update()
    {
        _ballBody = _ballPrefab.GetComponent<Rigidbody2D>();

        if (_gameController._shotCount <= 3 && !IsMouseOverUI())
        {
            Aim();
            Rotate();
        }
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    void Aim()
    {
        if (_shoot)
            return;
        if (Input.GetMouseButton(0))
        {
            if (!_aiming)
            {
                _aiming = true;
                _startPos = Input.mousePosition;
                _gameController.CheckShotCount();
            }
            else
            {
                PathCalculation();
            }
        }
        else if (_aiming && !_shoot)
        {
            _aiming = false;
            HideDots();
            StartCoroutine(Shoot());
            if(_gameController._shotCount == 1)
            {
                Camera.main.GetComponent<CameraController>().RotateCameraToSide();
            }
        }
    }

    Vector2 ShootForce(Vector3 force)
    {
        return (new Vector2(_startPos.x, _startPos.y) - new Vector2(force.x, force.y)) * _power;
    }

    Vector2 DotPath(Vector2 startP, Vector2 startVel, float t)
    {
        return startP + startVel * t + 0.5f * Physics2D.gravity * t * t;
    }

    void PathCalculation()
    {
        Vector2 vel = ShootForce(Input.mousePosition) * Time.fixedDeltaTime / _ballBody.mass;

        for (int i = 0; i < _projectilePath.Count; i++)
        {
            _projectilePath[i].GetComponent<Renderer>().enabled = true;
            float t = i / 15f;
            Vector3 point = DotPath(transform.position, vel, t);
            point.z = 1;
            _projectilePath[i].transform.position = point;
        }
    }

    void ShowDots()
    {
        for (int i = 0; i < _projectilePath.Count; i++)
        {
            _projectilePath[i].GetComponent<Renderer>().enabled = true;
        }
    }

    void HideDots()
    {
        for (int i = 0; i < _projectilePath.Count; i++)
        {
            _projectilePath[i].GetComponent<Renderer>().enabled = false;
        }
    }

    void Rotate()
    {
        Vector2 dir = GameObject.Find("dot (1)").transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    IEnumerator Shoot()
    {
        for (int i = 0; i < _gameController._ballsCount; i++)
        {
            yield return new WaitForSeconds(0.07f);
            GameObject ball = Instantiate(_ballPrefab, transform.position, Quaternion.identity);
            ball.name = "Ball";
            ball.transform.SetParent(_ballsContainer.transform);
            _ballBody = ball.GetComponent<Rigidbody2D>();
            _ballBody.AddForce(ShootForce(Input.mousePosition));

            int balls = _gameController._ballsCount - i;
            _gameController._ballsCountText.text = (_gameController._ballsCount - i - 1).ToString();
        }
        yield return new WaitForSeconds(0.5f);
        _gameController._shotCount++;
        _gameController._ballsCountText.text = _gameController._ballsCount.ToString();
    }
}
