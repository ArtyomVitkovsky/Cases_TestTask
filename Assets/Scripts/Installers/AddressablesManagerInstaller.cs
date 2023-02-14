using UI;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class AddressablesManagerInstaller : MonoInstaller
    {
        [SerializeField] private AddressablesManager addressablesManager;
        
        public override void InstallBindings()
        {
            var addressablesManagerInstance = Container.InstantiatePrefabForComponent<AddressablesManager>(addressablesManager);

            Container.Bind<AddressablesManager>().FromInstance(addressablesManagerInstance).AsSingle();
        }
    }
}