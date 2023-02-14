using UI;
using UI.Classes;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameUIInstaller : MonoInstaller
    {
        [SerializeField] private GameUI gameUI;
        public override void InstallBindings()
        {
            var gameUIInstance = Container.InstantiatePrefabForComponent<GameUI>(gameUI);

            Container.Bind<GameUI>().FromInstance(gameUIInstance).AsSingle();
        }
    }
}