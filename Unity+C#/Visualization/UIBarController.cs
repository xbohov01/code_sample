using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Source.Visualization
{
    public class UIBarController
    {
        public enum BarDirection
        {
            Vertical,
            Horizontal
        }
        //Value 0-100%
        public int CurrentBarValue
        {
            get { return currentValue; }
            set { ChangeValue(value);}
        }

        private int maxValue = 0;
        private int currentValue;
        private float fraction;
        private Vector3 position;
        private readonly float maxHeight;
        private readonly float maxWidth;
        private RectTransform barElement;
        private BarDirection scaleDirection;

        public UIBarController(RectTransform bar, int maxValue, BarDirection scaleDirection)
        {
            this.barElement = bar;
            position = bar.localPosition;
            maxHeight = bar.localScale.y;
            this.maxValue = maxValue;
            maxWidth = bar.localScale.x;
            fraction = maxHeight / maxValue;
            this.scaleDirection = scaleDirection;
        }

        public UIBarController(RectTransform bar, int maxValue, BarDirection scaleDirection, int startValue)
        {
            this.barElement = bar;
            //position = bar.localPosition;
            maxHeight = bar.localScale.y;
            this.maxValue = maxValue;
            maxWidth = bar.localScale.x;
            fraction = maxHeight / maxValue;
            this.scaleDirection = scaleDirection;
            position = new Vector3(barElement.localPosition.x, barElement.localPosition.y - (fraction * maxValue/2), barElement.localPosition.z);
            ChangeValue(startValue);
        }

        private void ChangeValue(int newValue)
        {
            int difference = newValue - currentValue;
            float newSize;
            //Resize the bar
            if (scaleDirection == BarDirection.Vertical)
            {
                newSize = barElement.sizeDelta.y + (difference * fraction);
                //barElement.rect.Set(position.x, position.y - (newSize/2), maxWidth, barElement.rect.height - difference);
                //barElement.localPosition = new Vector2(position.x, position.y - (newSize / 2));
                //barElement.sizeDelta = new Vector2(barElement.sizeDelta.x, barElement.sizeDelta.y - difference);

                barElement.localScale = new Vector3(barElement.localScale.x, fraction * newValue);
                barElement.localPosition = new Vector3(position.x, position.y + (fraction*newValue)/2, position.z);

                currentValue += difference;
            }
            else
            {
                newSize = barElement.rect.height + (difference * fraction);
                barElement.rect.Set(position.x - (newSize/2), position.y, barElement.rect.width - difference, maxHeight);
                currentValue += difference;
            }
        }
    }
}
