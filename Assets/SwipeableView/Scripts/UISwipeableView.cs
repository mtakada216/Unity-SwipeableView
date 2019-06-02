using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SwipeableView
{
    public class UISwipeableView<TData, TContext> : MonoBehaviour where TContext : class
    {
        [SerializeField] GameObject _cardPrefab = default;
        [SerializeField] Transform _cardRoot = default;
        [SerializeField] UISwiper _swiper = default;
        [SerializeField] SwipeableViewData _viewData = default;

        /// <summary>
        /// Is the card swiping.
        /// </summary>
        public bool IsAutoSwiping { get; private set; }

        /// <summary>
        /// Is the card exists.
        /// </summary>
        protected bool ExistsCard { get; private set; }

        protected TContext Context { get; }

        List<TData> _data = new List<TData>();

        readonly List<UISwipeableCard<TData, TContext>> _cards = new List<UISwipeableCard<TData, TContext>>(_maxCreateCardCount);
        const int _maxCreateCardCount = 2;


        /// <summary>
        /// Initialize of SwipeableView
        /// </summary>
        /// <param name="data"></param>
        protected void Initialize(List<TData> data)
        {
            _data = data;

            int createCount = data.Count > _maxCreateCardCount ?
                _maxCreateCardCount : data.Count;

            for (int i = 0; i < createCount; ++i)
            {
                var card = CreateCard();
                card.DataIndex = i;
                UpdateCardPosition(card);
                _cards.Add(card);
            }
        }

        /// <summary>
        /// Auto Swipe to the specified derection.
        /// </summary>
        /// <param name="direction"></param>
        public void AutoSwipe(SwipeDirection direction)
        {
            if (!ExistsCard) return;
            IsAutoSwiping = true;
            _swiper.AutoSwipe(direction);
        }

        UISwipeableCard<TData, TContext> CreateCard()
        {
            var cardObject = Instantiate(_cardPrefab, _cardRoot);
            var card = cardObject.GetComponent<UISwipeableCard<TData, TContext>>();
            card.SetContext(Context);
            card.SetVisible(false);
            card.ActionSwipedRight += UpdateCardPosition;
            card.ActionSwipedLeft += UpdateCardPosition;
            card.ActionSwipingRight += MoveToFrontNextCard;
            card.ActionSwipingLeft += MoveToFrontNextCard;

            return card;
        }

        void UpdateCardPosition(UISwipeableCard<TData, TContext> card)
        {
            // move to the back
            card.transform.SetAsFirstSibling();
            card.UpdatePosition(Vector3.zero);
            card.UpdateRotation(Vector3.zero);

            var childCount = transform.childCount;
            card.UpdateScale(childCount == 1 ? 1f : _viewData.BottomCardScale);

            var target = childCount == 1 ? card.gameObject : transform.GetChild(1).gameObject;
            _swiper.SetTarget(target, target.GetComponent<ISwipeable>());

            // When there are three or more data,
            // Replace card index with the seconde index from here.
            int index = _cards.Count < 2 ? card.DataIndex : card.DataIndex + 2;
            UpdateCard(card, index);
        }

        void UpdateCard(UISwipeableCard<TData, TContext> card, int dataIndex)
        {
            IsAutoSwiping = false;
            ExistsCard = dataIndex != _data.Count + 1;
            // if data doesn't exist hide card
            if (dataIndex < 0 || dataIndex > _data.Count - 1)
            {
                card.SetVisible(false);
                return;
            }

            card.SetVisible(true);
            card.DataIndex = dataIndex;
            card.UpdateContent(_data[dataIndex]);
        }

        void MoveToFrontNextCard(UISwipeableCard<TData, TContext> card, float rate)
        {
            var nextCard = _cards.FirstOrDefault(c => c.DataIndex != card.DataIndex);
            if (nextCard == null) return;

            var t = _viewData.CardAnimationCurve.Evaluate(rate);
            nextCard.UpdateScale(Mathf.Lerp(_viewData.BottomCardScale, 1f, t));
        }
    }

    /// <summary>
    /// Direction on the swipe.
    /// </summary>
    public enum SwipeDirection
    {
        Right,
        Left,
    }

    public sealed class SwipeableViewNullContext { }
    public class UISwipeableView<TData> : UISwipeableView<TData, SwipeableViewNullContext>
    { }
}