namespace SwipeableView
{
    public interface ISwipeable
    {
        void Move(UnityEngine.Vector2 position);
        void Rotate(float degree);
        void EndSwipe();
    }
}
