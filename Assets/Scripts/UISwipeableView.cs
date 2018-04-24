using System.Collections;
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


        private List<TData> data = new List<TData>();
        private TContext context;

        private readonly List<UISwipeableCard<TData, TContext>> cards
            = new List<UISwipeableCard<TData, TContext>>(2);



        private UISwipeableCard<TData, TContext> CreateCard()
        {
            var cardObject = Object.Instantiate(cardPrefab, cardRoot);
            cardPrefab.SetActive(true);
            cardObject.transform.SetAsLastSibling();

            var card = cardObject.GetComponent<UISwipeableCard<TData, TContext>>();
            card.SetContext(context);
            card.SetVisible(false);

            return card;
        }

        public void SetContext(TContext context)
        {
            this.context = context;

            for (int i = 0, count = cards.Count; i < count; ++i)
            {
                cards[i].SetContext(context);
            }
        }

        private void UpdateCard(int index)
        {
            // データが存在しなければ非表示
            if (index < 0 || index > data.Count - 1)
            {
                //cards[index].SetVisible(false);
                return;
            }

            var card = cards[index];
            card.SetVisible(true);
            card.DataIndex = index;
            card.UpdateContent(data[index]);
        }
	}
}

