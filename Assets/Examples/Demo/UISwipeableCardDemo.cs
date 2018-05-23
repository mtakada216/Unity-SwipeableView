using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SwipeableView
{
    public class UISwipeableCardDemo : UISwipeableCard<DemoCardData>
    {
        [SerializeField]
        private Image image;

        private RectTransform rect;

		private void Awake()
		{
            rect = transform as RectTransform;
		}

		public override void UpdateContent(DemoCardData data)
		{
            image.color = data.color;
		}
	}
}

