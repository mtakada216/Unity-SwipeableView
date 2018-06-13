# Unity-SwipeableView [![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?style=flat)](http://mit-license.org)

UISwipeableView is a simple UI components for implementing swipe views like [Tinder](https://tinder.com/).
Since Only two card objects are generated, performance will not be reduced even if the number of data items increases.

![screenshot1](https://github.com/m4tcha/Unity-SwipeableView/blob/master/Documents/screenshot1.gif)
![screenshot2](https://github.com/m4tcha/Unity-SwipeableView/blob/master/Documents/screenshot2.gif)

## Usage
Check out the [demo](https://github.com/m4tcha/Unity-SwipeableView/archive/master.zip) for an example.

### 1. Create your data object.
```c#
using UnityEngine;

public class DemoCardData
{
    public Color color;
}
```

### 2. Create SwipeableView by extends UISwipeableView.
```c#
using System.Collections.Generic;

public class UISwipeableViewDemo : UISwipeableView<DemoCardData>
{
    public void UpdateData(List<DemoCardData> data)
    {
        base.Initialize(data);
    }
}
```

### 3. Create SwipeableCard by extends UISwipeableCard.
```c#
using UnityEngine;
using UnityEngine.UI;

public class UISwipeableCardDemo : UISwipeableCard<DemoCardData>
{
    [SerializeField]
    private Image bg;

    public override void UpdateContent(DemoCardData data)
    {
        bg.color = data.color;
    }
}
```

### 4. Pass data to the SwipeableView.
```c#
using UnityEngine;
using System.Linq;

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
```

#### Prepared Callbacks
UISwipeableCard has 4 events that can be subscribed.
```c#
public Action<UISwipeableCard<TData, TContext>> ActionRightSwiped;          // Called when finish Swiping to the right.
public Action<UISwipeableCard<TData, TContext>> ActionLeftSwiped;           // Called when finish Swiping to the left.
public Action<UISwipeableCard<TData, TContext>, float> ActionRightSwiping;  // Called when swiping to the right. float(0-1)
public Action<UISwipeableCard<TData, TContext>, float> ActionLeftSwiping;   // Called when swiping to the left. float(0-1)
```


## Environment
Unity 2018.2.0b6

## License
MIT
