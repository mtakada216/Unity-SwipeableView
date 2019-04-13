namespace SwipeableView
{
    public interface ISwipeable
    {
        void Swipe(UnityEngine.Vector2 position);
        void EndSwipe();
        void AutoSwipeRight(UnityEngine.Vector3 from);
        void AutoSwipeLeft(UnityEngine.Vector3 from);
    }
}