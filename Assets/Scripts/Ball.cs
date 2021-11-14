using UnityEngine;
using UnityEngine.Pool;

public class Ball : MonoBehaviour
{
    private float _timer;
    private Transform _transform;
    private IObjectPool<Ball> _pool;

    ////[SerializeField] private BallSettings _ballSettings;
    ////private float _timeInTrigger;
    ////private bool _inWaterTrigger;

    public void SetPool(IObjectPool<Ball> pool) => _pool = pool;

    ////void OnTriggerEnter(Collider other) => _inWaterTrigger = true;

    ////void OnDisable() => _inWaterTrigger = false;

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > 10)
        {
            if (_pool != null)
            {
                _pool.Release(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        ////if (_inWaterTrigger == false)
        ////{
        ////    return;
        ////}

        ////_timeInTrigger += Time.deltaTime;
        ////transform.localScale = Vector3.one * (1f + _timeInTrigger / _ballSettings.LifeAfterLanding);
        ////if (_timeInTrigger >= _ballSettings.LifeAfterLanding)
        ////{
        ////    if (_pool != null)
        ////    {
        ////        _pool.Release(this);
        ////    }
        ////    else
        ////    {
        ////        Destroy(gameObject);
        ////    }
        ////}
    }

    void Awake() => _transform = transform;

    public void ResetTimerAndScale()
    {
        _timer = 0f;
        _transform.localScale = Vector3.one;  //using transform in the callback causes a 40b allocation

        ////_timeInTrigger = 0f;
    }
}
