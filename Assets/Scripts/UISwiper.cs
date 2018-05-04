using UnityEngine;
using UnityEngine.EventSystems;

namespace SwipeableView
{
    public class UISwiper : UIBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        [SerializeField]
        private float swipeSensitivity = 1f;

        private RectTransform viewRect;

        protected override void Awake()
        {
            base.Awake();

            viewRect = transform as RectTransform;
            card = GetComponent<ISwipeable>();
        }


        private Vector2 pointerStartLocalPosition;
        private Vector2 dragStartPosition;

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            pointerStartLocalPosition = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                viewRect,
                eventData.position,
                eventData.pressEventCamera,
                out pointerStartLocalPosition
            );

            dragStartPosition = dragCurrentPosition;
        }

        private Vector2 dragCurrentPosition;

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            UpdatePosition(GetLocalPosition(eventData));
        }

        private Vector2 dragEndPosition;

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            DecidePosition(GetLocalPosition(eventData));
        }

        private Vector2 GetLocalPosition(PointerEventData eventData)
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
        public void SetCard(ISwipeable card)
        {
            this.card = card;
        }

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

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            Vector2 localCursor = Vector2.zero;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                viewRect,
                eventData.position,
                eventData.pressEventCamera,
                out localCursor
            ))
            {
                return;
            }

            var size = viewRect.rect.size;
            localCursor = GetCorrectedLocalCursor(viewRect, localCursor);
            var pivot = (localCursor + size / 2) / size;
            ChangeViewRectPivot(viewRect, pivot);
        }

        private static readonly Vector2 PIVOT_DEFAULT = new Vector2(0.5f, 0.5f);
        private Vector2 GetCorrectedLocalCursor(RectTransform rect, Vector2 localCursor)
        {
            Vector2 deltaPivot = rect.pivot - PIVOT_DEFAULT;
            Vector2 deltaPosition = GetDeltaPosition(rect, deltaPivot);
            return localCursor + deltaPosition;
        }

        private Vector2 GetDeltaPosition(RectTransform rect, Vector2 deltaPivot)
        {
            Vector2 size = rect.sizeDelta;
            Vector3 scale = rect.localScale;
            Quaternion rotation = rect.rotation;
            return rotation * new Vector3(
                size.x * scale.x * deltaPivot.x,
                size.y * scale.y * deltaPivot.y
            );
        }

        private void ChangeViewRectPivot(RectTransform rect, Vector2 pivot)
        {
            Vector2 deltaPivot = rect.pivot - pivot;
            Vector3 deltaPositon = GetDeltaPosition(rect, deltaPivot);

            rect.pivot = pivot;
            rect.localPosition -= deltaPositon;
        }
	}
}

