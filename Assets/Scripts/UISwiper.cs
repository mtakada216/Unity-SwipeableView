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

            if (pivotChanger != null)
            {
                pivotChanger.Change(pointerStartLocalPosition);
            }

            startDragPosition = Vector2.zero;
        }

        private ISwipeable card;
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
                card.Move(pointerDelta);
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

        private float GetAim(Vector3 p1, Vector3 p2)
        {
            float dx = p2.x - p1.x;
            float dy = p2.y - p1.y;
            float rad = Mathf.Atan2(dy, dx);
            return rad * Mathf.Rad2Deg;
        }
	}
}

