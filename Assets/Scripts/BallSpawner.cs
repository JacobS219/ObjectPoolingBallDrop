using UnityEngine;
using UnityEngine.Pool;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private Transform _dropPoint;
    [SerializeField] private Ball _ballPrefab;
    [SerializeField] private Gradient _gradient;
    [SerializeField, Range(0.1f, 2f)] private float _gradientSpeed = 0.25f;
    [SerializeField] private ParticlePool _particlePool;

    private ObjectPool<Ball> _pool;
    private float _colorTimer;
    public int InactiveCount;
    public int ActiveCount;

    void Update()
    {
        var ball = _pool.Get();
        ball.transform.position = ChooseRandomPosition();
        InactiveCount = _pool.CountInactive;
        ActiveCount = _pool.CountActive;
    }

    void Awake() => _pool = new ObjectPool<Ball>(CreateBall, OnTakeBallFromPool, OnReturnBalltoPool);

    void OnTakeBallFromPool(Ball ball)
    {
        ball.gameObject.SetActive(true);

        _colorTimer += Time.deltaTime * _gradientSpeed;
        if (_colorTimer > 1f)
        {
            _colorTimer = 0f;
        }

        var color = _gradient.Evaluate(_colorTimer);
        ball.GetComponent<Renderer>().material.color = color;
        ball.ResetTimerAndScale();
    }

    void OnReturnBalltoPool(Ball ball)
    {
        if (_particlePool != null && _particlePool.isActiveAndEnabled)
        {
            _particlePool.Pool.Get().transform.position = ball.transform.position;
        }

        ball.gameObject.SetActive(false);
    }

    Ball CreateBall()
    {
        var ball = Instantiate(_ballPrefab);
        ball.SetPool(_pool);
        return ball;
    }

    Vector3 ChooseRandomPosition()
    {
        int size = 5;
        var offset = new Vector3(Random.Range(-size, size), 0f, Random.Range(-size, size));
        return _dropPoint.position + offset;
    }

    void OnValidate(){ }
}
