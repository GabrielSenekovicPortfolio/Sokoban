using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ScoreManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<GameOverManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ProgressionManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<AudioManager>().FromComponentInHierarchy().AsSingle();
    }
}