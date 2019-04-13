using UnityEngine;
using UnityEngine.UI;

namespace SwipeableView
{
    public class UISwipeableCardLoadTexture : UISwipeableCard<LoadTextureCardData>
    {
        [SerializeField]
        private RawImage image = default;

        [SerializeField]
        private CanvasGroup imgLike = default;

        [SerializeField]
        private CanvasGroup imgNope = default;

        public override void UpdateContent(LoadTextureCardData data)
        {
            TextureDownloader.I.Load(data.url, texture =>
            {
                image.texture = texture;
            });

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