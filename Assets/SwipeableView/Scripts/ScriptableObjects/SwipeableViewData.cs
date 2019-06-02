using UnityEngine;

namespace SwipeableView
{
    [CreateAssetMenu(menuName = "ScriptableObject/Create SwipeableViewData", fileName = "SwipeableViewData")]
    public class SwipeableViewData : ScriptableObject
    {
        [SerializeField] float _swipeDuration = 0.28f;
        [SerializeField] float _bottomCardScale = 0.92f;
        [SerializeField] int _maxInclinationAngle = 10;
        [SerializeField] AnimationCurve _cardAnimationCurve = new AnimationCurve(
            new Keyframe(0f, 0f, 0f, 2f), new Keyframe(1f, 1f, 0f, 0f));

        public float SwipeDuration => _swipeDuration;
        public float BottomCardScale => _bottomCardScale;
        public int MaxInclinationAngle => _maxInclinationAngle;
        public AnimationCurve CardAnimationCurve => _cardAnimationCurve;
    }
}