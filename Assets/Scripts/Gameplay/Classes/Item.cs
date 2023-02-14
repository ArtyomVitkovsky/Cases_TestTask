using System;
using Gameplay.Interfaces;
using Setup.Items;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Gameplay.Classes
{
    public class Item : MonoBehaviour, IHorizontalMovable
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private TextMeshProUGUI costTMPro;
        [SerializeField] private Image icon;
        [SerializeField] private Image border;
        private Item _previousItem;
        [SerializeField] private ItemSetup _currentItemsSetup;
        private bool _isRollToClosest;
        
        public RectTransform RectTransform => rectTransform;

        public Item PreviousItem => _previousItem;

        public ItemSetup CurrentItemsSetup => _currentItemsSetup;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void Setup(Item previousItem, ItemSetup itemSetup)
        {
            var height = rectTransform.sizeDelta.y;
            rectTransform.sizeDelta = new Vector2(height, height);
            
            SetupView(itemSetup);

            _currentItemsSetup = itemSetup;
            _previousItem = previousItem;
        }

        public void SetupView(ItemSetup itemSetup)
        {
            icon.sprite = itemSetup.Icon;
            border.color = itemSetup.BorderColor;
            costTMPro.text = $"{itemSetup.Cost}";
            _currentItemsSetup = itemSetup;
        }

        public void Move(float speed)
        {
            _rigidbody2D.velocity = -Vector2.right * speed;
        }

        public void Move(Vector3 position)
        {
            rectTransform.anchoredPosition = position;
        }
        
    }
}