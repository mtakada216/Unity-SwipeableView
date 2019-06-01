using System.Collections.Generic;

namespace SwipeableView
{
    public class UISwipeableViewBasic : UISwipeableView<BasicCardData>
    {
        public void UpdateData(List<BasicCardData> data)
        {
            Initialize(data);
        }
    }
}