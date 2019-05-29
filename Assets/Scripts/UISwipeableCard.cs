using System;
using System.Collections;
using UnityEngine;

namespace SwipeableView
{
    public class UISwipeableCard<TData, TContext> : MonoBehaviour, ISwipeable where TContext : class
    {
        /// <summary>
        /// Index of Card Data.
        /// </summary>
        public int DataIndex { get; set; }

        /// <summary>
        /// Callbacks
        /// </summary>
        public event Action<UISwipeableCard<TData, TContext>> ActionSwipedRight;
        public event Action<UISwipeableCard<TData, TContext>> ActionSwipedLeft;
        public event Action<UISwipeableCard<TData, TContext>, float> ActionSwipingRight;
        public event Action<UISwipeableCard<TData, TContext>, float> ActionSwipingLeft;

        protected TContext Context { get; private set; }

        RectTransform cachedRect;

        const float _epsion = 1.192093E-07f;
        const float _maxInclinedAngle = 10f;
        const float _moveDuration = 0.25f;

        void OnEnable()
        {
            cachedRect = transform as RectTransform;
        }

        void Update()
        {
            var rectPosX = cachedRect.localPosition.x;
            if (Math.Abs(rectPosX) < _epsion)
            {
                SwipingRight(0);
                SwipingLeft(0);
                return;
            }

            var t = GetCurrentPosition(rectPosX);
            var maxAngle = rectPosX < 0 ? _maxInclinedAngle : -_maxInclinedAngle;
            UpdateRotation(Vector3.Lerp(Vector3.zero, new Vector3(0f, 0f, maxAngle), t));

            if (rectPosX > 0)
            {
                SwipingRight(t);
                ActionSwipingRight?.Invoke(this, t);
            }
            else if (rectPosX < 0)
            {
                SwipingLeft(t);
                ActionSwipingLeft?.Invoke(this, t);
            }
        }


        /// <summary>
        /// Updates the Content.
        /// </summary>
        /// <param name="data"></param>
        public virtual void UpdateContent(TData data)
        { }

        /// <summary>
        /// Set the Context.
        /// </summary>
        /// <param name="context"></param>
        public virtual void SetContext(TContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Set the visible.
        /// </summary>
        /// <param name="visible"></param>
        public virtual void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }

        /// <summary>
        /// Updates the position.
        /// </summary>
        /// <param name="position"></param>
        public virtual void UpdatePosition(Vector3 position)
        {
            cachedRect.localPosition = position;
        }

        /// <summary>
        /// Updates the rotaion.
        /// </summary>
        /// <param name="rotation"></param>
        public virtual void UpdateRotation(Vector3 rotation)
        {
            cachedRect.localEulerAngles = rotation;
        }

        /// <summary>
        /// Update the scale.
        /// </summary>
        /// <param name="scale"></param>
        public virtual void UpdateScale(float scale)
        {
            cachedRect.localScale = scale * Vector3.one;
        }

        /// <summary>
        /// Right swiping.
        /// </summary>
        /// <param name="rate"></param>
        protected virtual void SwipingRight(float rate)
        { }

        /// <summary>
        /// Left swiping.
        /// </summary>
        /// <param name="rate"></param>
        protected virtual void SwipingLeft(float rate)
        { }

#region ISwipeable
        public void Swipe(Vector2 position)
        {
            UpdatePosition(cachedRect.localPosition + new Vector3(position.x, position.y, 0));
        }

        public void EndSwipe()
        {
            // over required distance -> Auto swipe
            if (IsSwipedRight(cachedRect.localPosition))
            {
                AutoSwipeRight(cachedRect.localPosition);
            }
            else if (IsSwipedLeft(cachedRect.localPosition))
            {
                AutoSwipeLeft(cachedRect.localPosition);
            }
            // Not been reached required distance -> Return to default position
            else
            {
                StartCoroutine(MoveCoroutine(cachedRect.localPosition, Vector3.zero));
            }
        }

        public void AutoSwipeRight(Vector3 from)
        {
            var to = new Vector3(cachedRect.rect.size.x * 1.5f, from.y, from.z);
            StartCoroutine(MoveCoroutine(from, to, () =>
            {
                ActionSwipedRight?.Invoke(this);
            }));
        }

        public void AutoSwipeLeft(Vector3 from)
        {
            var to = new Vector3(-(cachedRect.rect.size.x * 1.5f), from.y, from.z);
            StartCoroutine(MoveCoroutine(from, to, () =>
            {
                ActionSwipedLeft?.Invoke(this);
            }));
        }
#endregion

        bool IsSwipedRight(Vector3 position)
        {
            return position.x > 0 && position.x > GetRequiredDistance(position.x);
        }

        bool IsSwipedLeft(Vector3 position)
        {
            return position.x < 0 && position.x < GetRequiredDistance(position.x);
        }

        float GetRequiredDistance(float positionX)
        {
            return positionX > 0 ? cachedRect.rect.size.x / 2 : -(cachedRect.rect.size.x / 2);
        }

        float GetCurrentPosition(float positionX)
        {
            return positionX / GetRequiredDistance(positionX);
        }

        IEnumerator MoveCoroutine(Vector3 from, Vector3 to, Action onComplete = null)
        {
            float endTime = Time.time + _moveDuration;

            while (true)
            {
                float diff = endTime - Time.time;
                if (diff <= 0)
                {
                    break;
                }

                float rate = 1 - Mathf.Clamp01(diff / _moveDuration);
                cachedRect.localPosition = Vector3.Lerp(from, to, rate);
                yield return null;
            }

            cachedRect.localPosition = to;
            onComplete?.Invoke();
        }
    }

    public class UISwipeableCard<TData> : UISwipeableCard<TData, SwipeableViewNullContext>
    { }
}