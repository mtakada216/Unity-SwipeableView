using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwipeableView
{
    public class UISwipeableCard<TData, TContext> : MonoBehaviour, ISwipeable where TContext : class
    {
        private Transform cachedTransform;

        void Awake()
        {
            cachedTransform = this.transform;
        }


        private TContext context;
        public virtual void SetContext(TContext context)
        {
            this.context = context;
        }

        public virtual void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }

        public virtual void Swipe(Vector2 position)
        {
            cachedTransform.localPosition = vec;
        }
	}
}

