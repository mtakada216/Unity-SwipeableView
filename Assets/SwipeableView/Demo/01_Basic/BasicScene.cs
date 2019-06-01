using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SwipeableView
{
    public class BasicScene : MonoBehaviour
    {
        [SerializeField]
        private UISwipeableViewBasic swipeableView = default;

        void Start()
        {
            var data = Enumerable.Range(0, 20)
                .Select(i => new BasicCardData
                {
                    color = new Color(Random.value, Random.value, Random.value, 1.0f)
                })
                .ToList();

            swipeableView.UpdateData(data);
        }

        public void OnClickLike()
        {
            if (swipeableView.IsAutoSwiping) return;
            swipeableView.AutoSwipe(SwipeDirection.Right);
        }

        public void OnClickNope()
        {
            if (swipeableView.IsAutoSwiping) return;
            swipeableView.AutoSwipe(SwipeDirection.Left);
        }
    }
}