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

        private RectTransform cachedRect;

        private const float EPSILON = 1.192093E-07f;
        private const float MAX_INCLINED_ANGLE = 10f;

        void OnEnable()
        {
            cachedRect = transform as RectTransform;
        }

        void Update()
        {
            if (Math.Abs(cachedRect.localPosition.x) < EPSILON)
            {
                SwipingToRight(0);
                SwipingToLeft(0);
                return;
            }

            var t = GetCurrentPosition(cachedRect.localPosition.x);
            var maxAngle = cachedRect.localPosition.x < 0 ? MAX_INCLINED_ANGLE : -MAX_INCLINED_ANGLE;
            UpdateRotation(Vector3.Lerp(Vector3.zero, new Vector3(0f, 0f, maxAngle), t));

            if (cachedRect.localPosition.x > 0)
            {
                SwipingToRight(t);
                ActionSwipingRight?.Invoke(this, t);
            }
            else if (cachedRect.localPosition.x < 0)
            {
                SwipingToLeft(t);
                ActionSwipingLeft?.Invoke(this, t);
            }
        }

        public virtual void SetContext(TContext context)
        { }

        public virtual void UpdateContent(TData data)
        { }

        public virtual void SwipingToRight(float rate)
        { }

        public virtual void SwipingToLeft(float rate)
        { }

        public virtual void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }

        public virtual void UpdatePosition(Vector3 position)
        {
            cachedRect.localPosition = position;
        }

        public virtual void UpdateRotation(Vector3 rotation)
        {
            cachedRect.localEulerAngles = rotation;
        }

        public virtual void UpdateScale(float scale)
        {
            cachedRect.localScale = scale * Vector3.one;
        }


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
                AutoSwipeToRight(cachedRect.localPosition);
            }
            else if (IsSwipedLeft(cachedRect.localPosition))
            {
                AutoSwipeToLeft(cachedRect.localPosition);
            }
            // Not been reached required distance -> Return to default position
            else
            {
                StartCoroutine(MoveCoroutine(cachedRect.localPosition, Vector3.zero));
                StartCoroutine(RotateCoroutine(cachedRect.localEulerAngles, Vector3.zero));
            }
        }

        public void AutoSwipeToRight(Vector3 from)
        {
            Vector3 to = new Vector3(cachedRect.rect.size.x * 1.5f, from.y, from.z);
            StartCoroutine(MoveCoroutine(from, to, () =>
            {
                if (ActionSwipedRight != null)
                {
                    ActionSwipedRight.Invoke(this);
                }
            }));
        }

        public void AutoSwipeToLeft(Vector3 from)
        {
            Vector3 to = new Vector3(-(cachedRect.rect.size.x * 1.5f), from.y, from.z);
            StartCoroutine(MoveCoroutine(from, to, () =>
            {
                if (ActionSwipedLeft != null)
                {
                    ActionSwipedLeft.Invoke(this);
                }
            }));
        }
#endregion

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

        private float GetCurrentPosition(float positionX)
        {
            return positionX / GetRequiredDistance(positionX);
        }

        private const float DURATION = 0.25f;
        private IEnumerator MoveCoroutine(Vector3 from, Vector3 to, Action onComplete = null)
        {
            yield return PlayAnimationCoroutine(
                diff =>
                {
                    float rate = 1 - Mathf.Clamp01(diff / DURATION);
                    cachedRect.localPosition = Vector3.Lerp(from, to, rate);
                },
                () =>
                {
                    cachedRect.localPosition = to;
                    onComplete?.Invoke();
                }
            );
        }

        private IEnumerator RotateCoroutine(Vector3 from, Vector3 to, Action onComplete = null)
        {
            yield return PlayAnimationCoroutine(
                diff =>
                {
                    float rate = 1 - Mathf.Clamp01(diff / DURATION);
                    float angleZ = Mathf.Lerp(from.z > 180 ? from.z - 360 : from.z, to.z, rate);
                    cachedRect.localEulerAngles = new Vector3(from.x, from.y, angleZ);
                },
                () =>
                {
                    cachedRect.localEulerAngles = to;
                    onComplete?.Invoke();
                }
            );
        }

        private IEnumerator PlayAnimationCoroutine(Action<float> onUpdate, Action onComplete)
        {
            float endTime = Time.time + DURATION;

            while (true)
            {
                float diff = endTime - Time.time;
                if (diff <= 0)
                {
                    break;
                }

                onUpdate.Invoke(diff);
                yield return null;
            }

            onComplete.Invoke();
        }
    }

    public class UISwipeableCard<TData> : UISwipeableCard<TData, SwipeableViewNullContext>
    { }
}