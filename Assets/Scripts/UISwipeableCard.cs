using System;
using System.Collections;
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
				MoveOutViewRight(cachedRect.localPosition, () =>
				{
					if (ActionRightSwiped != null)
					{
						ActionRightSwiped.Invoke(this);
					}
				});
			}
			else if (IsSwipedLeft(cachedRect.localPosition))
			{
				MoveOutViewLeft(cachedRect.localPosition, () =>
				{
					if (ActionLeftSwiped != null)
					{
						ActionLeftSwiped.Invoke(this);
					}
				});
			}
			else
			{
				StartCoroutine(MoveCoroutine(cachedRect.localPosition, Vector3.zero));
				StartCoroutine(RotateCoroutine(cachedRect.localEulerAngles, Vector3.zero));
			}
		}

        private bool IsSwipedRight(Vector3 position)
        {
			return position.x > 0 && position.x > GetRequiredDistance(position.x);
        }

        private bool IsSwipedLeft(Vector3 position)
        {
			return position.x < 0 && position.x < GetRequiredDistance(position.x);
        }

		private float GetRequiredDistance(float positionX)
		{
			return positionX > 0 ? cachedRect.rect.size.x / 2 : -(cachedRect.rect.size.x / 2);
		}

		private const float DURATION = 0.25f;
		private void MoveOutViewRight(Vector3 from, Action onComplete)
		{
			Vector3 to = new Vector3(cachedRect.rect.size.x * 1.5f, from.y, from.z);
			StartCoroutine(MoveCoroutine(from, to, onComplete));
		}

		private void MoveOutViewLeft(Vector3 from, Action onComplete)
		{
			Vector3 to = new Vector3(-(cachedRect.rect.size.x * 1.5f), from.y, from.z);
			StartCoroutine(MoveCoroutine(from, to, onComplete));
		}

		private IEnumerator MoveCoroutine(Vector3 from, Vector3 to, Action onComplete = null)
		{
			float endTime = Time.time + DURATION;

			while(true)
			{
				float diff = endTime - Time.time;
				if (diff <= 0)
				{
					break;
				}

				float rate = 1 - Mathf.Clamp01(diff / DURATION);
				cachedRect.localPosition = Vector3.Lerp(from, to, rate);
				yield return null;
			}

			cachedRect.localPosition = to;
			if (onComplete != null)
			{
				onComplete.Invoke();
			}
		}

		private IEnumerator RotateCoroutine(Vector3 from, Vector3 to, Action onComplete = null)
		{
			float endTime = Time.time + DURATION;

			while(true)
			{
				float diff = endTime - Time.time;
				if (diff <= 0)
				{
					break;
				}

				float rate = 1 - Mathf.Clamp01(diff / DURATION);
				float angleZ = Mathf.Lerp(from.z > 180 ? from.z - 360 : from.z, to.z, rate);
				cachedRect.localEulerAngles = new Vector3(from.x, from.y, angleZ);
				yield return null;
			}

			cachedRect.localEulerAngles = to;
			if (onComplete != null)
			{
				onComplete.Invoke();
			}
		}

        #endregion
    }

    public class UISwipeableCard<TData> : UISwipeableCard<TData, SwipeableViewNullContext>
    {
    }
}

