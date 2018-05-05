using UnityEngine;
using UnityEngine.EventSystems;

namespace SwipeableView
{
    public class UISwiper : UIBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private float swipeSensitivity = 1f;

        private RectTransform viewRect;

        protected override void Awake()
        {
            base.Awake();

            viewRect = transform as RectTransform;
            card = GetComponent<ISwipeable>();
            pivotChanger = GetComponent<UIPivotChanger>();
        }

        private IChangeablePivot pivotChanger;
        private Vector2 pointerStartLocalPosition;

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            pointerStartLocalPosition = GetLocalPoint(eventData);

            if (pivotChanger != null)
            {
                pivotChanger.Change(pointerStartLocalPosition);
            }
        }

        private Vector2 dragCurrentPosition;

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            UpdatePosition(GetLocalPoint(eventData));
        }

        private Vector2 dragEndPosition;
        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            DecidePosition(GetLocalPoint(eventData));
        }

        private Vector2 GetLocalPoint(PointerEventData eventData)
        {
            Vector2 localCursor = Vector2.zero;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                viewRect,
                eventData.position,
                eventData.pressEventCamera,
                out localCursor
            ))
            {
                return localCursor;
            }

            return (localCursor - pointerStartLocalPosition) * swipeSensitivity;
        }

        private ISwipeable card;
        private void UpdatePosition(Vector2 position)
        {
            dragCurrentPosition = position;

            if (card != null)
            {
                card.Swipe(position);
            }
        }

        private void DecidePosition(Vector2 position)
        {
            dragEndPosition = position;

            if (card != null)
            {
                card.EndSwipe();
            }
        }
	}
}

