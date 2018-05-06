using UnityEngine;
using UnityEngine.EventSystems;

namespace SwipeableView
{
    public class UIPivotChanger : UIBehaviour, IChangeablePivot
    {
        private static readonly Vector2 DEFAULT = new Vector2(0.5f, 0.5f);

        private RectTransform viewRect;
        private Vector2 rectSize;
        private Vector3 rectScale;
        private Quaternion rectRotation;

		protected override void Awake()
		{
            base.Awake();

            viewRect = transform as RectTransform;
            rectSize = viewRect.sizeDelta;
            rectScale = viewRect.localScale;
            rectRotation = viewRect.rotation;
		}

        public Vector2 Change(Vector2 position)
        {
            Vector2 correctedPoint = GetCorrectedPoint(position);
            Vector2 pivot = (correctedPoint + rectSize / 2) / rectSize;

            Vector2 deltaPivot = viewRect.pivot - pivot;
            Vector3 deltaPosition = GetDeltaPosition(deltaPivot);
            viewRect.pivot = pivot;
            viewRect.localPosition -= deltaPosition;
            return position + new Vector2(viewRect.localPosition.x, viewRect.localPosition.y);
        }

        private Vector2 GetCorrectedPoint(Vector2 current)
        {
            Vector2 deltaPivot = viewRect.pivot - DEFAULT;
            Vector2 deltaPosition = GetDeltaPosition(deltaPivot);
            return current + deltaPosition;
        }

        private Vector2 GetDeltaPosition(Vector2 deltaPivot)
        {
            return rectRotation * new Vector3(
                rectSize.x * rectScale.x * deltaPivot.x,
                rectSize.y * rectScale.y * deltaPivot.y
            );
        }
    }
}
