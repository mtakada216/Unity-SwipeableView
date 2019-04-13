using System.Collections.Generic;

namespace SwipeableView
{
    public class UISwipeableViewLoadTexture : UISwipeableView<LoadTextureCardData>
    {
        public void UpdateData(List<LoadTextureCardData> data)
        {
            Initialize(data);
        }
    }
}