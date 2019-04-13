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

        protected override void SwipingRight(float rate)
        {
            imgLike.alpha = rate;
            imgNope.alpha = 0;
        }

        protected override void SwipingLeft(float rate)
        {
            imgNope.alpha = rate;
            imgLike.alpha = 0;
        }
    }
}