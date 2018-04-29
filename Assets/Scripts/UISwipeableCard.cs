using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwipeableView
{
    public class UISwipeableCard<TData, TContext> : MonoBehaviour, ISwipeable where TContext : class
    {
        public int DataIndex { get; set; }
        public Action<UISwipeableCard<TData, TContext>, int> ActionRightSwipe { get; set; }
        public Action<UISwipeableCard<TData, TContext>, int> ActionLeftSwipe { get; set; }


        private RectTransform cachedRect;

        void Start()
        {
            cachedRect = transform as RectTransform;
        }

        public virtual void SetContext(TContext context)
        {
        }

        public virtual void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }

        public virtual void UpdateContent(TData data)
        {
        }

        public virtual void UpdatePosition(Vector3 position)
        {
        }

        public virtual void Swipe(Vector2 position)
        {
            cachedRect.localPosition += new Vector3(position.x, position.y, 0);
        }
	}

    public class UISwipeableCard<TData> : UISwipeableCard<TData, SwipeableViewNullContext>
    {
    }
}

