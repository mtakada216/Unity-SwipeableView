using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwipeableView
{
    public class UISwipeableViewDemo : UISwipeableView<DemoCardData>
    {
        public void UpdateData(List<DemoCardData> data)
        {
            base.Initialize(data);
        }
    }
}