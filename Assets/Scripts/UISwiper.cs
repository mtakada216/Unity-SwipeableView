using UnityEngine;
using UnityEngine.EventSystems;

namespace SwipeableView
{
    public class UISwiper : UIBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private float swipeSensitivity = 1f;

        private RectTransform viewport;

		protected override void Awake()
		{
            base.Awake();

            viewport = GetComponent<RectTransform>();
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
                viewport,
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
                viewport,
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
	}
}

