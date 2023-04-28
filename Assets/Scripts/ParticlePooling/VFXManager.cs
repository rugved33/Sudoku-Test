using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class VFXManager : MonoBehaviour
{
    private ParticlePooler _particlePooler;

    [Inject]
    private void Construct(ParticlePooler particlePooler)
    {
        _particlePooler = particlePooler;
    }
    public void SpawnVFX(VFXType vfxType, Vector3 targetPos)
    {
        Transform particle = _particlePooler.GetParticle(vfxType);

        if (particle != null)
        {
            particle.position = targetPos;
            particle.gameObject.SetActive(true);
        }
    }
}
