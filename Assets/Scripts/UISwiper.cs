using UnityEngine;
using UnityEngine.EventSystems;

namespace SwipeableView
{
	public class UISwiper : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private float swipeSensitivity = 1f;

        private RectTransform viewRect;

		private ISwipeable card;
		public void SetCard(GameObject card)
		{
			this.card = card.GetComponent<ISwipeable>();
			viewRect = card.transform as RectTransform;
		}

        private Vector2 pointerStartLocalPosition;
        private Vector2 startDragPosition;
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

            startDragPosition = Vector2.zero;
        }

        private Vector2 dragCurrentPosition;
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            Vector2 localCursor;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                viewRect,
                eventData.position,
                eventData.pressEventCamera,
                out localCursor
            ))
            {
                return;
            }

            // pivot位置のの変更によってpositionが変化している可能性があるため、
            // このタイミングでドラッグ開始位置を取得する
            if (startDragPosition == Vector2.zero)
            {
                startDragPosition = localCursor;
            }

            var pointerDelta = localCursor - startDragPosition;

            if (card != null)
            {
				card.Swipe(pointerDelta);
            }
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            if (card != null)
            {
                card.EndSwipe();
            }
        }
	}
}

