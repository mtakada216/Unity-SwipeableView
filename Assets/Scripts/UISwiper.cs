using UnityEngine;
using UnityEngine.EventSystems;

namespace SwipeableView
{
    public class UISwiper : UIBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        [SerializeField]
        private float swipeSensitivity = 1f;

        private RectTransform rect;

		protected override void Awake()
		{
            base.Awake();

            rect = transform as RectTransform;
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
                rect,
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
                rect,
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
                rect,
                eventData.position,
                eventData.pressEventCamera,
                out localCursor
            ))
            {
                return;
            }

            Debug.Log(localCursor);
            var size = rect.rect.size;
            var pivot = (localCursor + size / 2) / size;
            SetPivot(rect, pivot);
        }

        private void SetPivot(RectTransform rect, Vector2 pivot)
        {
            Vector3 size = rect.sizeDelta;
            Vector2 scale = rect.localScale;
            Vector2 deltaPivot = rect.pivot - pivot;
            Vector3 deltaPositon =
                rect.rotation * 
                    new Vector3(
                        deltaPivot.x * size.x * scale.x,
                        deltaPivot.y * size.y * scale.y);

            rect.pivot = pivot;
            rect.localPosition -= deltaPositon;
        }
	}
}

