using System;
using UnityEngine;

namespace SwipeableView
{
    public class UISwipeableCard<TData, TContext> : MonoBehaviour, ISwipeable where TContext : class
    {
		[SerializeField]
		private float maxInclinedAngle = 10f;

        public int DataIndex { get; set; }
        public Action<UISwipeableCard<TData, TContext>> ActionRightSwiped { get; set; }
        public Action<UISwipeableCard<TData, TContext>> ActionLeftSwiped { get; set; }
		public Action<UISwipeableCard<TData, TContext>, float> ActionRightSwiping { get; set; }
		public Action<UISwipeableCard<TData, TContext>, float> ActionLeftSwipeing { get; set; }

		private RectTransform cachedRect;
        void OnEnable()
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
            cachedRect.localPosition = position;
        }

        public virtual void UpdateRotation(Vector3 rotation)
        {
			cachedRect.localEulerAngles = rotation;
        }

        #region Swipe
		public void Swipe(Vector2 position)
		{
			UpdatePosition(cachedRect.localPosition + new Vector3(position.x, position.y, 0));

			var t = cachedRect.localPosition.x / GetRequiredDistance(cachedRect.localPosition.x);
			var maxAngle = cachedRect.localPosition.x < 0 ? maxInclinedAngle : -maxInclinedAngle;
			var rotation = Vector3.Lerp(Vector3.zero, new Vector3(0f, 0f, maxAngle), t);
			UpdateRotation(rotation);

			if (position.x > 0)
			{
				if (ActionRightSwiping != null)
				{
					ActionRightSwiping.Invoke(this, t);
				}
			}
			else
			{
				if (ActionLeftSwipeing != null)
				{
					ActionLeftSwipeing.Invoke(this, t);
				}
			}
		}

        public void EndSwipe()
        {
            if (IsSwipedRight(cachedRect.localPosition))
            {
				if (ActionRightSwiped != null)
				{
					ActionRightSwiped.Invoke(this);
				}
            }
            else if (IsSwipedLeft(cachedRect.localPosition))
            {
				if (ActionLeftSwiped != null)
				{
					ActionLeftSwiped.Invoke(this);
				}
            }
        }

		/// <summary>
		/// 右側にスワイプされた
		/// </summary>
        private bool IsSwipedRight(Vector3 position)
        {
			return position.x > GetRequiredDistance(position.x);
        }

		/// <summary>
		/// 左側にスワイプされた
		/// </summary>
        private bool IsSwipedLeft(Vector3 position)
        {
			return position.x < GetRequiredDistance(position.y);
        }

		/// <summary>
		/// スワイプに必要な距離
		/// </summary>
		private float GetRequiredDistance(float positionX)
		{
			return positionX > 0 ? cachedRect.rect.size.x / 2 : -(cachedRect.rect.size.x / 2);
		}
        #endregion
    }

    public class UISwipeableCard<TData> : UISwipeableCard<TData, SwipeableViewNullContext>
    {
    }
}

