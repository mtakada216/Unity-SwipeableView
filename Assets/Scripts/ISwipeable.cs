namespace SwipeableView
{
    public interface ISwipeable
    {
        void Move(UnityEngine.Vector2 position);
        void Rotate(UnityEngine.Vector2 angle);
        void EndSwipe();
    }
}
