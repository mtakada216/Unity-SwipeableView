using UnityEngine;
using UnityEngine.EventSystems;

namespace SwipeableView
{
    public class UISwiper : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        RectTransform cachedRect;

        ISwipeable swipeable;

        /// <summary>
        /// Set the target.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="swipeable"></param>
        public void SetTarget(GameObject target, ISwipeable swipeable)
        {
            cachedRect = target.transform as RectTransform;
            this.swipeable = swipeable;
        }

        /// <summary>
        /// Auto Swipe to the specified derection.
        /// </summary>
        /// <param name="direction"></param>
        public void AutoSwipe(SwipeDirection direction)
        {
            if (direction == SwipeDirection.Right)
            {
                swipeable.AutoSwipeRight(Vector3.zero);
            }
            else
            {
                swipeable.AutoSwipeLeft(Vector3.zero);
            }
        }

#region DragHandler
        Vector2 pointerStartLocalPosition;
        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            if (cachedRect == null || !cachedRect.gameObject.activeInHierarchy)
            {
                return;
            }

            pointerStartLocalPosition = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                cachedRect,
                eventData.position,
                eventData.pressEventCamera,
                out pointerStartLocalPosition
            );
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            if (cachedRect == null || !cachedRect.gameObject.activeInHierarchy)
            {
                return;
            }

            Vector2 localCursor;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    cachedRect,
                    eventData.position,
                    eventData.pressEventCamera,
                    out localCursor
                ))
            {
                return;
            }

            var pointerDelta = localCursor - pointerStartLocalPosition;
            swipeable.Swipe(pointerDelta);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            if (cachedRect == null || !cachedRect.gameObject.activeInHierarchy)
            {
                return;
            }

            swipeable.EndSwipe();
        }
#endregion
    }
}