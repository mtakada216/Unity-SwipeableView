using UnityEngine;
using UnityEngine.UI;

namespace SwipeableView
{
    public class UISwipeableCardBasic : UISwipeableCard<BasicCardData>
    {
        [SerializeField]
        private Image bg = default;

        [SerializeField]
        private CanvasGroup imgLike = default;

        [SerializeField]
        private CanvasGroup imgNope = default;

        public override void UpdateContent(BasicCardData data)
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