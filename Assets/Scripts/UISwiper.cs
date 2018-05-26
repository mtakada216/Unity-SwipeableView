using UnityEngine;
using UnityEngine.EventSystems;

namespace SwipeableView
{
	public class UISwiper : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
		private RectTransform cardRect;

		private ISwipeable card;
		public void SetCard(GameObject card)
		{
			this.card = card.GetComponent<ISwipeable>();
			cardRect = card.transform as RectTransform;
		}

        private Vector2 pointerStartLocalPosition;
        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

			if (cardRect == null || !cardRect.gameObject.activeInHierarchy)
			{
				return;
			}

            pointerStartLocalPosition = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                cardRect,
                eventData.position,
                eventData.pressEventCamera,
                out pointerStartLocalPosition
            );
        }

        private Vector2 dragCurrentPosition;
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

			if (cardRect == null || !cardRect.gameObject.activeInHierarchy)
			{
				return;
			}

            Vector2 localCursor;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                cardRect,
                eventData.position,
                eventData.pressEventCamera,
                out localCursor
            ))
            {
                return;
            }

			var pointerDelta = localCursor - pointerStartLocalPosition;
			card.Swipe(pointerDelta);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

			if (cardRect == null || !cardRect.gameObject.activeInHierarchy)
			{
				return;
			}

            card.EndSwipe();
        }
	}
}

