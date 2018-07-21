using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SwipeableView
{
    public class LoadTextureScene : MonoBehaviour
    {
        [SerializeField]
        private UISwipeableViewLoadTexture swipeableView;

        [SerializeField]
        private TextureDownloader textureDownloader;

        private static readonly string[] imageUrls = new string[]
        {
            "https://images.pexels.com/photos/1245063/pexels-photo-1245063.jpeg?cs=srgb&dl=agriculture-countryside-crop-1245063.jpg&fm=jpg",
            "https://images.pexels.com/photos/1232594/pexels-photo-1232594.jpeg?cs=srgb&dl=alps-clouds-dawn-1232594.jpg&fm=jpg",
            "https://images.pexels.com/photos/1212487/pexels-photo-1212487.jpeg?cs=srgb&dl=android-wallpaper-bloom-blossom-1212487.jpg&fm=jpg",
            "https://images.pexels.com/photos/899634/pexels-photo-899634.jpeg?cs=srgb&dl=arch-architecture-building-899634.jpg&fm=jpg",
            "https://images.pexels.com/photos/699466/pexels-photo-699466.jpeg?cs=srgb&dl=architecture-art-blue-699466.jpg&fm=jpg",
        };

        void Start()
        {
            var data = imageUrls
                .Select(imageUrl => new LoadTextureCardData
                {
                    url = imageUrl
                })
                .ToList();

            swipeableView.UpdateData(data);
        }

        public void OnClickLike()
        {
            swipeableView.AutoSwipeTo(Direction.Right);
        }

        public void OnClickNope()
        {
            swipeableView.AutoSwipeTo(Direction.Left);
        }
    }
}