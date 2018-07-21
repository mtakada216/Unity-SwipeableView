using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwipeableView
{
    public class UISwipeableViewLoadTexture : UISwipeableView<LoadTextureCardData>
    {
        public void UpdateData(List<LoadTextureCardData> data)
        {
            base.Initialize(data);
        }
    }
}