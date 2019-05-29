using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SwipeableView
{
    public class UISwipeableView<TData, TContext> : MonoBehaviour where TContext : class
    {
        [SerializeField]
        GameObject cardPrefab = default;

        [SerializeField]
        Transform cardRoot = default;

        [SerializeField]
        UISwiper swiper = default;

        /// <summary>
        /// Is the card swiping.
        /// </summary>
        public bool IsAutoSwiping { get; private set; }

        /// <summary>
        /// Is the card exists.
        /// </summary>
        protected bool ExistsCard { get; private set; }

        protected TContext Context { get; }

        List<TData> data = new List<TData>();

        readonly List<UISwipeableCard<TData, TContext>> cards = new List<UISwipeableCard<TData, TContext>>(_maxCreateCardCount);

        const int _maxCreateCardCount = 2;


        /// <summary>
        /// Initialize of SwipeableView
        /// </summary>
        /// <param name="data"></param>
        protected void Initialize(List<TData> data)
        {
            this.data = data;

            int createCount = data.Count > _maxCreateCardCount ?
                _maxCreateCardCount : data.Count;

            for (int i = 0; i < createCount; ++i)
            {
                var card = CreateCard();
                card.DataIndex = i;
                UpdateCardPosition(card);
                cards.Add(card);
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
            swiper.AutoSwipe(direction);
        }

        UISwipeableCard<TData, TContext> CreateCard()
        {
            var cardObject = Instantiate(cardPrefab, cardRoot);
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
            card.UpdateScale(transform.childCount == 1 ? 1f : 0.92f);

            var target = transform.childCount == 1 ? card.gameObject : transform.GetChild(1).gameObject;
            swiper.SetTarget(target, target.GetComponent<ISwipeable>());
            // When there are three or more data,
            // Replace card index with the seconde index from here.
            int index = cards.Count < 2 ? card.DataIndex : card.DataIndex + 2;
            UpdateCard(card, index);
        }

        void UpdateCard(UISwipeableCard<TData, TContext> card, int dataIndex)
        {
            IsAutoSwiping = false;
            ExistsCard = dataIndex != data.Count + 1;
            // if data doesn't exist hide card
            if (dataIndex < 0 || dataIndex > data.Count - 1)
            {
                card.SetVisible(false);
                return;
            }

            card.SetVisible(true);
            card.DataIndex = dataIndex;
            card.UpdateContent(data[dataIndex]);
        }

        void MoveToFrontNextCard(UISwipeableCard<TData, TContext> card, float rate)
        {
            var nextCard = cards.FirstOrDefault(c => c.DataIndex != card.DataIndex);
            if (nextCard == null) return;

            nextCard.UpdateScale(Mathf.Lerp(0.92f, 1f, rate));
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