using UnityEngine;
using UnityEngine.EventSystems;

namespace SwipeableView
{
    public class UISwiper : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private RectTransform cachedRect;

        private ISwipeable swipeable;
        public void SetCard(GameObject card)
        {
            swipeable = card.GetComponent<ISwipeable>();
            cachedRect = card.transform as RectTransform;
        }

        public void AutoSwipeTo(Direction direction)
        {
            if (direction == Direction.Right)
            {
                swipeable.AutoSwipeToRight(Vector3.zero);
            }
            else
            {
                swipeable.AutoSwipeToLeft(Vector3.zero);
            }
        }

        private Vector2 pointerStartLocalPosition;
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
    }
}