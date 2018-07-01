using UnityEngine;
using UnityEngine.UI;

namespace SwipeableView
{
    public class UISwipeableCardDemo : UISwipeableCard<DemoCardData>
    {
        [SerializeField]
        private Image bg;

        [SerializeField]
        private CanvasGroup imgLike;

        [SerializeField]
        private CanvasGroup imgNope;

        public override void UpdateContent(DemoCardData data)
        {
            bg.color = data.color;

            imgLike.alpha = 0;
            imgNope.alpha = 0;
        }

        public override void SwipingToRight(float rate)
        {
            imgLike.alpha = rate;
            imgNope.alpha = 0;
        }

        public override void SwipingToLeft(float rate)
        {
            imgNope.alpha = rate;
            imgLike.alpha = 0;
        }
    }
}