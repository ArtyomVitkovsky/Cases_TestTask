using System;
using UnityEngine;

namespace Setup.Items
{
    [Serializable]
    public class ItemSetup
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private int cost;
        private Color borderColor;

        public Sprite Icon => icon;

        public int Cost => cost;

        public Color BorderColor => borderColor;
        

        public void SetColor(Color targetColor)
        {
            borderColor = targetColor;
        }
    }
}