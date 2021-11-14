using UnityEngine;
using UnityEngine.Pool;

public class ParticlePool : MonoBehaviour
{
    public enum PoolType
    {
        Stack,
        Linkedlist
    }

    public PoolType poolType;

    public bool collectionChecks = true;
    public int maxPoolSize = 10;

    private IObjectPool<ParticleSystem> m_Pool;
    [SerializeField] private ParticleSystem _particlePrefab;

    public IObjectPool<ParticleSystem> Pool
    {
        get
        {
            if (m_Pool == null)
            {
                if (poolType == PoolType.Stack)
                {
                    m_Pool = new ObjectPool<ParticleSystem>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
                }
                else
                {
                    m_Pool = new ObjectPool<ParticleSystem>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, maxPoolSize);
                }
            }

            return m_Pool;
        }
    }

    ParticleSystem CreatePooledItem()
    {
        var go = Instantiate(_particlePrefab);
        var ps = go.GetComponent<ParticleSystem>();
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        var main = ps.main;
        main.duration = 1;
        main.startLifetime = 1;
        main.loop = false;

        var returnToPool = go.gameObject.AddComponent<ReturnToPool>();
        returnToPool.pool = Pool;

        return ps;
    }

    void OnReturnedToPool(ParticleSystem system)
    {
        system.gameObject.SetActive(false);
    }

    void OnTakeFromPool(ParticleSystem system)
    {
        system.gameObject.SetActive(true);
    }

    void OnDestroyPoolObject(ParticleSystem system)
    {
        Destroy(system.gameObject);
    }
}
