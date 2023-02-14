using System;
using Setup;
using Setup.Items;
using UI;
using UI.Classes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Camera worldCamera;
    [SerializeField] private GameSetup gameSetup;
    [SerializeField] private ItemsSetup itemsSetup;
    [Inject] private GameUI _gameUI;
    [Inject] private AddressablesManager _addressablesManager;

    private UnityAction _onAssetsLoaded;
    
    private void Awake()
    {
        _gameUI.ShowLoadingScreen();
        _onAssetsLoaded += InitializeGameplay;
        _addressablesManager.LoadAsset(gameSetup.ItemsSetupReference, AssignItemsSetup);
    }

    private void AssignItemsSetup(AsyncOperationHandle<ItemsSetup> loadedAsset)
    {
        itemsSetup = loadedAsset.Result;
        _onAssetsLoaded?.Invoke();
    }

    private void InitializeGameplay()
    {
        _gameUI.Initialize(worldCamera, itemsSetup);
    }
}