using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwipeableView
{
    public class UISwipeableCard<TData, TContext> : MonoBehaviour, ISwipeable where TContext : class
    {
        public int DataIndex { get; set; }

        private Transform cachedTransform;

        void Awake()
        {
            cachedTransform = this.transform;
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

        public virtual void Swipe(Vector2 position)
        {
            cachedTransform.localPosition += new Vector3(position.x, position.y, 0);
        }
	}
}

