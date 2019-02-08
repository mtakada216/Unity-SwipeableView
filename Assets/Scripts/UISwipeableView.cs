using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SwipeableView
{
    public class UISwipeableView<TData, TContext> : MonoBehaviour where TContext : class
    {
        [SerializeField]
        private GameObject cardPrefab;

        [SerializeField]
        private Transform cardRoot;

        [SerializeField]
        private UISwiper swiper;

        private List<TData> data = new List<TData>();
        private TContext context;

        private readonly List<UISwipeableCard<TData, TContext>> cards = new List<UISwipeableCard<TData, TContext>>(MAX_CREATE_COUNT);

        private const int MAX_CREATE_COUNT = 2;

        public void Initialize(List<TData> data)
        {
            this.data = data;

            int createCount = data.Count > MAX_CREATE_COUNT ?
                MAX_CREATE_COUNT : data.Count;

            for (int i = 0; i < createCount; ++i)
            {
                var card = CreateCard();
                card.DataIndex = i;
                UpdateCardPosition(card);
                cards.Add(card);
            }
        }

        public void AutoSwipeTo(Direction direction)
        {
            swiper.AutoSwipeTo(direction);
        }

        protected void SetContext(TContext context)
        {
            this.context = context;

            for (int i = 0, count = cards.Count; i < count; ++i)
            {
                cards[i].SetContext(context);
            }
        }

        private UISwipeableCard<TData, TContext> CreateCard()
        {
            var cardObject = Object.Instantiate(cardPrefab, cardRoot);
            var card = cardObject.GetComponent<UISwipeableCard<TData, TContext>>();
            card.SetContext(context);
            card.SetVisible(false);
            card.ActionSwipedRight += UpdateCardPosition;
            card.ActionSwipedLeft += UpdateCardPosition;
            card.ActionSwipingRight += MoveFrontNextCard;
            card.ActionSwipingLeft += MoveFrontNextCard;

            return card;
        }

        private void UpdateCardPosition(UISwipeableCard<TData, TContext> card)
        {
            // move to the back
            card.transform.SetAsFirstSibling();
            card.UpdatePosition(Vector3.zero);
            card.UpdateRotation(Vector3.zero);
            card.UpdateScale(transform.childCount == 1 ? 1f : 0.92f);

            var swipeTarget = transform.childCount == 1 ? card.gameObject : transform.GetChild(1).gameObject;
            swiper.SetCard(swipeTarget);
            // When there are three or more data,
            // Replace card index with the seconde index from here.
            int index = cards.Count < 2 ? card.DataIndex : card.DataIndex + 2;
            UpdateCard(card, index);
        }

        private void UpdateCard(UISwipeableCard<TData, TContext> card, int dataIndex)
        {
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

        private void MoveFrontNextCard(UISwipeableCard<TData, TContext> card, float rate)
        {
            var nextCard = cards.FirstOrDefault(c => c.DataIndex != card.DataIndex);
            if (nextCard == null)
            {
                return;
            }

            nextCard.UpdateScale(Mathf.Lerp(0.92f, 1f, rate));
        }
    }

    public enum Direction
    {
        Right,
        Left,
    }

    public class SwipeableViewNullContext { }
    public class UISwipeableView<TData> : UISwipeableView<TData, SwipeableViewNullContext>
    { }
}