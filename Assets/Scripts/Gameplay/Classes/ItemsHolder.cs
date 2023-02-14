using System;
using System.Collections.Generic;
using Setup.Items;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Classes
{
    [Serializable]
    public class ItemsHolder : MonoBehaviour
    {
        [SerializeField] private Item itemPrefab;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private RectTransform container;
        [SerializeField] private HorizontalLayoutGroup containerHlg;
        private ItemsSetup _itemsSetup;
        
        private Dictionary<int, Item> _itemsMap;

        public UnityAction OnInitialized;
        
        public void Initialize(ItemsSetup itemsSetup, RectTransform itemsWiper)
        {
            this._itemsSetup = itemsSetup;
            
            _itemsMap = new Dictionary<int, Item>();

            for (int i = 0; i < 10; i++)
            {
                var instantiatedItem = Instantiate(itemPrefab, container);
                instantiatedItem.gameObject.name += $"_{i}";
                _itemsMap.Add(i, instantiatedItem);
            }

            CalculateContainerSize();
            
            itemsWiper.anchoredPosition = new Vector2(
                -(container.rect.height - 
                    containerHlg.padding.top - containerHlg.padding.bottom
                    + containerHlg.spacing),
                0);

            foreach (var key in _itemsMap.Keys)
            {
                SetupItem(key, itemsSetup);
            }

            
            OnInitialized?.Invoke();
        }
        
        private void SetupItem(int index, ItemsSetup itemsSetup)
        {
            var previousIndex = index == 0 ? _itemsMap.Count - 1 : index - 1;
            var itemSetup = itemsSetup.GetRandomItem();
            _itemsMap[index].Setup(_itemsMap[previousIndex], itemSetup);
        }

        private void WipeItem(Item itemToWipe)
        {
            var posX = itemToWipe.PreviousItem.RectTransform.anchoredPosition.x + itemToWipe.RectTransform.rect.width + containerHlg.spacing;
            itemToWipe.RectTransform.anchoredPosition = new Vector2(posX, itemToWipe.RectTransform.anchoredPosition.y);
            
            var itemSetup = _itemsSetup.GetRandomItem();
            itemToWipe.SetupView(itemSetup);
        }

        private void CalculateContainerSize()
        {
            Canvas.ForceUpdateCanvases();
            
            var itemHeight = rectTransform.rect.height - containerHlg.padding.bottom - containerHlg.padding.top;
            var width = itemHeight * _itemsMap.Count + 
                        containerHlg.spacing * _itemsMap.Count - 1 + 
                        containerHlg.padding.left + containerHlg.padding.right;

            container.sizeDelta = new Vector2(width, container.sizeDelta.y);
            RecalculateContainerLayout();
        }

        private void RecalculateContainerLayout()
        {
            containerHlg.CalculateLayoutInputHorizontal();
            containerHlg.CalculateLayoutInputVertical();
            containerHlg.SetLayoutHorizontal();
            containerHlg.SetLayoutVertical();
        }

        public void RollItems(float speed)
        {
            foreach (var itemKey in _itemsMap.Keys)
            {
                _itemsMap[itemKey].Move(speed);
            }
        }
        public void RollItems(Vector2 offset)
        {
            foreach (var itemKey in _itemsMap.Keys)
            {
                var item = _itemsMap[itemKey];
                var targetPosition = item.RectTransform.anchoredPosition + offset;
                _itemsMap[itemKey].Move(targetPosition);
            }
        }

        public void WipeItems(Vector3 wipePosition)
        {
            foreach (var key in _itemsMap.Keys)
            {
                var item = _itemsMap[key];
                var pos = item.transform.InverseTransformPoint(wipePosition);
                if (pos.x > 0)
                {
                    WipeItem(item);
                }
            }
        }

        public Vector3 GetClosestToCenterItemPositionDistance(out Vector3 direction, out Item closestItem)
        {
            Item closestItemResult = _itemsMap[0];
            Vector3 closestPosition = Vector3.positiveInfinity;
            direction = Vector3.right;
            
            closestPosition.y = closestPosition.z = 0;
            foreach (var key in _itemsMap.Keys)
            {
                var item = _itemsMap[key];
                var itemRT = item.RectTransform;
                var xDistance = (container.sizeDelta.x / 2f) - (itemRT.anchoredPosition.x);
                
                var heading = new Vector3(xDistance, 0, 0);
                var distance = heading.magnitude;

                if (Mathf.Abs(distance) < closestPosition.x)
                {
                    closestPosition.x = Mathf.Abs(distance);
                    direction = heading / distance;
                    closestItemResult = item;
                }
            }

            closestItem = closestItemResult;

            direction.y = direction.z = 0;
            return closestPosition;
        }
    }
}