using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace SwipeableView
{
    public class UISwiper : UIBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {

        private Vector2 dragStartPosition;

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            dragStartPosition = eventData.position;
        }

        private Vector2 dragCurrentPosition;

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            UpdatePosition(eventData.position);
        }

        private Vector2 dragEndPosition;

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            dragEndPosition = eventData.position;
        }

        public ISwipeable card { get; set; }
        private void UpdatePosition(Vector2 position)
		{
            dragCurrentPosition = position;

            if (card != null)
            {
                card.Swipe(position);
            }
		}
	}
}

