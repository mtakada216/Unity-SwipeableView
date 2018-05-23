using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwipeableView
{
    public class DemoScene : MonoBehaviour
    {
        [SerializeField]
        private UISwipeableViewDemo swipeableView;

        void Start()
        {
            var data = Enumerable.Range(0, 20)
                                 .Select(i => new DemoCardData
                                 {
                                    color = new Color(Random.value, Random.value, Random.value, 1.0f)
                                 })
                                 .ToList();

            swipeableView.UpdateData(data);
        }
    }
}
