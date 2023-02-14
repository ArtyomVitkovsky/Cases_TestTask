using System;
using Gameplay.Classes;
using Setup.Items;
using UI.Interfaces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Classes
{
    [Serializable]
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private CanvasScaler canvasScaler;
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private WinPopup winPopup;
        [SerializeField] private Roulette roulette;
        public UnityAction OnInitialized;
        
        
        public void Initialize(Camera renderCamera, ItemsSetup itemsSetup)
        {
            canvas.worldCamera = renderCamera;
            
            var match = (float) Screen.height / Screen.width;
            var referenceMatch = canvasScaler.referenceResolution.y / canvasScaler.referenceResolution.x;
            
            if (match >= referenceMatch)
            {
                match = 1f - match;
            }

            canvasScaler.matchWidthOrHeight = match;
            
            InitializeRoulette(itemsSetup);
        }
        
        private void InitializeRoulette(ItemsSetup itemsSetup)
        {
            roulette.OnInitialized += InitializationHolder;
            roulette.OnWin += ShowWinScreen;
            roulette.Initialize(itemsSetup);
        }
        
        public void ShowLoadingScreen()
        {
            loadingScreen.SetActive(true);
        }

        private void ShowWinScreen(ItemSetup itemSetup)
        {
            winPopup.SetItemSetup(itemSetup);
            ShowPopup(winPopup);
        }

        private void ShowPopup(IPopup popup)
        {
            popup.Show();
        }
        
        public void HideLoadingScreen()
        {
            loadingScreen.SetActive(false);
        }

        private void InitializationHolder()
        {
            OnInitialized?.Invoke();
            HideLoadingScreen();
        }

        private void OnDestroy()
        {
            roulette.OnInitialized -= InitializationHolder;
            roulette.OnWin -= ShowWinScreen;
        }
    }
}