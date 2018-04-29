using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SwipeableView
{
    public class UISwipeableCardExample01 : UISwipeableCard<CardDataExample01>
    {
        [SerializeField]
        private Text message;

        private RectTransform rect;

		private void Awake()
		{
            rect = transform as RectTransform;
		}

		public override void UpdateContent(CardDataExample01 data)
		{
            message.text = data.message;
		}

		public override void UpdatePosition(Vector3 position)
		{
            rect.localPosition = position;
		}
	}
}

