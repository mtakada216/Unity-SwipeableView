using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwipeableView
{
    public class UISwipeableViewBasic : UISwipeableView<BasicCardData>
    {
        public void UpdateData(List<BasicCardData> data)
        {
            base.Initialize(data);
        }
    }
}