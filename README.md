# Unity-SwipeableView [![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?style=flat)](http://mit-license.org)

UISwipeableView is a simple UI components for implementing swipe views like [Tinder](https://tinder.com/).
Since Only two card objects are generated, performance will not be reduced even if the number of data items increases.

![screenshot1](https://github.com/m4tcha/Unity-SwipeableView/blob/master/Documents/screenshot1.gif)
![screenshot2](https://github.com/m4tcha/Unity-SwipeableView/blob/master/Documents/screenshot2.gif)

## Usage
Check out the [demo](https://github.com/m4tcha/Unity-SwipeableView/archive/master.zip) for an example.

### 1. Create your data object.
```c#
public class DemoCardData
{
    public Color color;
}
```

### 2. Create SwipeableView by extends UISwipeableView.
```c#
public class SwipeableViewDemo : UISwipeableView<DemoCardData>
{
    public void UpdateData(List<DemoCardData> data)
    {
        base.Initialize(data);
    }
}
```

### 3. Create SwipeableCard by extends UISwipeableCard.
```c#
public class SwipeableCardDemo : UISwipeableCard<DemoCardData>
{
    [SerializeField] private Image bg;

    public override void UpdateContent(DemoCardData data)
    {
        bg.color = data.color;
    }
}
```

### 4. Pass data to the SwipeableView.
```c#
public class DemoScene : MonoBehaviour
{
    [SerializeField] private UISwipeableViewDemo swipeableView;

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

## Environment
Unity 2018.3.11f1

## License
MIT

## Author
[kiepng](https://github.com/kiepng)
