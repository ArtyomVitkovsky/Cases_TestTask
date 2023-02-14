using System;
using Setup.Items;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Classes
{
    [Serializable]
    public class Roulette : MonoBehaviour
    {
        [SerializeField] private ItemsHolder itemsHolder;
        [SerializeField] private Button rollButton;
        [SerializeField] private float speed;
        [SerializeField] private float rollDuration;
        [SerializeField] private RectTransform _itemsWiper;

        private Item _collectedItem;
        
        private float _currentSpeed;
        private float _currentRollDuration;

        private float _currentScrollValueToClosestItem;
        private float _targetScrollValueToClosestItem;
        private Vector3 _directionToCollector;

        private bool _isRolling;
        private bool _isRollingToClosestItem;

        private float _rollStartTime;

        public UnityAction OnInitialized;
        public UnityAction<ItemSetup> OnWin;

        public void Initialize(ItemsSetup itemsSetup)
        {
            rollButton.onClick.AddListener(StartRoll);

            itemsHolder.OnInitialized += InitializationHolder;
            itemsHolder.Initialize(itemsSetup, _itemsWiper);
        }

        private void InitializationHolder()
        {
            OnInitialized?.Invoke();
        }

        private void FixedUpdate()
        {
            if (_isRolling)
            {
                Roll();

            }
            else if (_isRollingToClosestItem)
            {
                RollToClosestItem();
            }

            if (_isRolling || _isRollingToClosestItem) WipeItems();
        }

        private void StartRoll()
        {
            _rollStartTime = Time.realtimeSinceStartup;
            Debug.LogWarning($"Roll start time = {_rollStartTime}");
            _currentSpeed = speed;
            _currentRollDuration = 0;
            _isRolling = true;
        }

        private void Roll()
        {
            if (_currentSpeed > 0.001f)
            {
                _currentSpeed = Mathf.Lerp( speed, 0, _currentRollDuration / rollDuration);
                _currentRollDuration += Time.fixedDeltaTime;
            }
            else
            {
                Debug.LogWarning($"Roll duration = {Time.realtimeSinceStartup - _rollStartTime}");

                _currentSpeed = 0;
                _isRolling = false;
                SetRollOffsetToClosestItem();
            }
            
            itemsHolder.RollItems(_currentSpeed);
        }

        private void SetRollOffsetToClosestItem()
        {
            var offset = itemsHolder.GetClosestToCenterItemPositionDistance(out _directionToCollector, out _collectedItem);
            _targetScrollValueToClosestItem = offset.x;
            _currentScrollValueToClosestItem = 0;
            _currentRollDuration = 0;
            _isRollingToClosestItem = true;
        }
        
        private void RollToClosestItem()
        {
            var duration = (rollDuration * 0.1f);
            _currentScrollValueToClosestItem = Mathf.Lerp( _targetScrollValueToClosestItem, 0, _currentRollDuration / duration);
            _currentRollDuration += Time.fixedDeltaTime;

            itemsHolder.RollItems(_directionToCollector * _targetScrollValueToClosestItem / duration * Time.fixedDeltaTime);

            if (_currentScrollValueToClosestItem <= 0.001f)
            {
                _isRollingToClosestItem = false;
                OnWin?.Invoke(_collectedItem.CurrentItemsSetup);
            }

        }

        private void WipeItems()
        {
            itemsHolder.WipeItems(_itemsWiper.position);
        }
    }
}