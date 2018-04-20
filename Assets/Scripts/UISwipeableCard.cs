using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwipeableView
{
    public class UISwipeableCard<TData, TContext> : MonoBehaviour where TContext : class
	{
        private TContext context;

        public virtual void SetContext(TContext context)
        {
            this.context = context;
        }

        public virtual void SetVisible(bool visible)
        {
        }
	}
}

