
using UnityEngine;
using Zenject;

public class ParticlePoolerInstaller : MonoInstaller
{
    [SerializeField] private ParticlePooler _particlePooler;

    public override void InstallBindings()
    {
        Container.Bind<ParticlePooler>().FromInstance(_particlePooler).AsSingle();
    }
}
