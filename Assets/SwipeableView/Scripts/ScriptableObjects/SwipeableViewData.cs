using UnityEngine;

namespace SwipeableView
{
    [CreateAssetMenu(menuName = "ScriptableObject/Create SwipeableViewData", fileName = "SwipeableViewData")]
    public class SwipeableViewData : ScriptableObject
    {
        [SerializeField] float _swipeDuration = 0.25f;
        [SerializeField] float _bottomCardScale = 0.92f;
        [SerializeField] int _maxInclinationAngle = 10;

        public float SwipeDuration => _swipeDuration;
        public float BottomCardScale => _bottomCardScale;
        public int MaxInclinationAngle => _maxInclinationAngle;
    }
}