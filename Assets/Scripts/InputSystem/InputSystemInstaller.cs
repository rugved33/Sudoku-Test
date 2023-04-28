using Zenject;
using UnityEngine;

public class InputSystemInstaller : MonoInstaller
{
    [SerializeField] private InputSystem _inputSystem;

    public override void InstallBindings()
    {
        Container.Bind<InputSystem>().FromInstance(_inputSystem).AsSingle();
    }
}
