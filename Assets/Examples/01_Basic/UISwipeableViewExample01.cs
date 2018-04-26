using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwipeableView
{
    public class UISwipeableViewExample01 : UISwipeableView<CardDataExample01>
    {
        public void UpdateData(List<CardDataExample01> data)
        {
            base.data = data;
            Init();
        }
    }
}
