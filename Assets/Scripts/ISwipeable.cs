namespace SwipeableView
{
    public interface ISwipeable
    {
        void Swipe(UnityEngine.Vector2 position);
        void EndSwipe();
        void AutoSwipeToRight(UnityEngine.Vector3 from);
        void AutoSwipeToLeft(UnityEngine.Vector3 from);
    }
}