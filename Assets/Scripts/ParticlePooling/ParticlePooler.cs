using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
public class ParticlePooler : MonoBehaviour
{
    [SerializeField] private ParticleDataBase _particleDataBase;
    private Queue<PoolTimer> _poolTimers = new Queue<PoolTimer>();
    private List<PoolTimer> _timers = new List<PoolTimer>();

    private void Start()
    {
        InitializePool();
    }
    private void Update()
    {
        if(_poolTimers.Count > 0)
        {
            for (int i = _timers.Count - 1; i >= 0 ; i--)
            {
                _timers[i].Update();
            }
        }
    }

    private void InitializePool()
    {
        for (int i = 0; i < _particleDataBase.Particles.Count; i++)
        {
            _particleDataBase.Particles[i].InitPool(transform);
        }
    }
    public  Transform GetParticle(VFXType vfxType)
    {
        int index = 0;

        for (int i =  _particleDataBase.Particles.Count - 1; i >= 0 ; i--)
        {
            if(vfxType == _particleDataBase.Particles[i].VFXType)
            {
                index = i;
            }
        }

        if (index > _particleDataBase.Particles.Count)
        {
            return null;
        }

        Transform particle = _particleDataBase.Particles[index].Pool.GetElement<Transform>();

        PoolTimer poolTimer = new PoolTimer(_particleDataBase.Particles[index].Pool,
                                            particle,
                                            _particleDataBase.Particles[index].
                                            Duration);
        _poolTimers.Enqueue(poolTimer);
        _timers.Add(poolTimer);
        poolTimer.OnTimerComplete += RemoveTimer;
        return particle;
    }

    private void RemoveTimer()
    {
        _poolTimers.Dequeue();
        _timers.RemoveAt(_timers.Count-1);
    }

    public void SpawnVFX(VFXType vfxType, Vector3 targetPos)
    {
        Transform particle = GetParticle(vfxType);

        if (particle != null)
        {
            particle.position = targetPos;
            particle.gameObject.SetActive(true);
        }
    }
}

public class PoolTimer
{
    private float _delay = 1;
    private float _timer;
    private Pool _pool;
    private Transform _pooledObject;
    public UnityAction OnTimerComplete;
    public PoolTimer(Pool pool, Transform poolObject, float duration)
    {
        _pool = pool;
        _pooledObject = poolObject;
        _delay = duration;
    }
    public void Update()
    {
        if(_pooledObject)
        {
            _timer += Time.deltaTime;

            if(_timer > _delay)
            {
                if(_pool == null)
                {
                    _pooledObject.gameObject.SetActive(false);
                }
                else
                {
                    _pool.ReturnElement(_pooledObject);
                }
                _timer = 0;
                OnTimerComplete?.Invoke();
            }
        }
    }
    public void ReturnToPool()
    {
        _timer = 0;
        _pool.ReturnElement(_pooledObject);
        _pooledObject.gameObject.SetActive(false);
        OnTimerComplete?.Invoke();
    }
}

[System.Serializable]
public class ParticleSystemDataBase
{
    public List<PoolObject> Particles = new List<PoolObject>();
}
