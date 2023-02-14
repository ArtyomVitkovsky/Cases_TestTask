using System;
using DG.Tweening;
using Gameplay.Classes;
using Setup.Items;
using UI.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Classes
{
    public class WinPopup : MonoBehaviour, IPopup
    {
        [SerializeField] private RectTransform body;
        [SerializeField] private Image overlay;
        [SerializeField] private Item collectedItem;
        [SerializeField] private Button closeButton;

        private Color overlayColor = new Color(0, 0, 0, 0.8f);
        private Color overlayColorTransparent = new Color(0, 0, 0, 0);
        
        private void Awake()
        {
            closeButton.onClick.AddListener(Close);
        }

        public void SetItemSetup(ItemSetup itemSetup)
        {
            SetupItem(itemSetup);
        }
        
        public void Show()
        {
            body.gameObject.SetActive(true);
            overlay.gameObject.SetActive(true);
            overlay.DOColor(overlayColor, 0.1f);
            body.DOScale(1.1f, 0.1f).OnComplete(() =>
            {
                body.DOScale(1f, 0.1f);
            });
        }

        public void Close()
        {
            overlay.DOColor(overlayColorTransparent, 0.1f).OnComplete(()=>
            {
                overlay.gameObject.SetActive(false);
            });
            
            body.DOScale(1.1f, 0.1f).OnComplete(() =>
            {
                body.DOScale(0f, 0.1f).OnComplete(() =>
                {
                    body.gameObject.SetActive(false);
                });
            });
        }

        private void SetupItem(ItemSetup itemSetup)
        {
            collectedItem.SetupView(itemSetup);
        }
    }
}
