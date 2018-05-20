using System.Collections.Generic;
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

        private readonly List<UISwipeableCard<TData, TContext>> cards
            = new List<UISwipeableCard<TData, TContext>>(MAX_CREATE_COUNT);

        private const int MAX_CREATE_COUNT = 2;


		public void Initialize(List<TData> data)
		{
            this.data = data;

            int createCount = data.Count > MAX_CREATE_COUNT 
                    ? MAX_CREATE_COUNT : data.Count;
            
            for (int i = 0; i < createCount; ++i)
            {
                var card = CreateCard();
                UpdateCardPosition(card, i);
                cards.Add(card);
            }
		}

		public void SetContext(TContext context)
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
            cardPrefab.SetActive(true);
            cardObject.transform.SetAsLastSibling();

			var swipeTarget = transform.childCount == 1 ? cardObject : transform.GetChild(0).gameObject;
			swiper.SetCard(swipeTarget);

            var card = cardObject.GetComponent<UISwipeableCard<TData, TContext>>();
            card.SetContext(context);
            card.SetVisible(false);
            card.ActionRightSwipe += UpdateCardPosition;
            card.ActionLeftSwipe += UpdateCardPosition;

            return card;
        }

        protected void UpdateCardPosition(UISwipeableCard<TData, TContext> card, int dataIndex)
        {
            // 再背面に移動
            card.transform.SetAsFirstSibling();
            card.UpdatePosition(Vector3.zero);
            card.UpdateRotation(Vector3.zero);
            card.UpdatePivot(new Vector2(0.5f, 0.5f));
            // 3枚目以降のカードだった場合、
            // 次のカードはすでに表示されているため、そのさらに次のカードを表示する
            int index = cards.Count < 2 ? dataIndex : dataIndex + 2;
            UpdateCard(card, index);
        }

        private void UpdateCard(UISwipeableCard<TData, TContext> card, int dataIndex)
        {
            // データが存在しなければ非表示
            if (dataIndex < 0 || dataIndex > data.Count - 1)
            {
                card.SetVisible(false);
                return;
            }

            card.SetVisible(true);
            card.DataIndex = dataIndex;
            card.UpdateContent(data[dataIndex]);
        }
	}

    public class SwipeableViewNullContext {}

    public class UISwipeableView<TData> : UISwipeableView<TData, SwipeableViewNullContext>
    {
    }
}

