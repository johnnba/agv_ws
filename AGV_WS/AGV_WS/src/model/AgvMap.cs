using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace AGV_WS.src.model
{
    public class AgvMapPath
    {
        public int PathId { get; set; }
        public Point Position1 { get; set; }
        public Point Position2 { get; set; }
    }

    public class AgvMapCard
    {
        public int CardId { get; set; }
        public Point Position { get; set; }
    }

    public class AgvMap
    {
        private int _id;
        private List<AgvMapPath> _paths;
        private List<AgvMapCard> _cards;

        public List<AgvMapPath> Paths { get { return _paths; } }
        public List<AgvMapCard> Cards { get { return _cards; } }

        public AgvMap(int id)
        {
            _id = id;

            _paths = new List<AgvMapPath>();

            _cards = new List<AgvMapCard>();

            init();
        }

        private void init()
        {
            double[] pntX = new double[] { 300, 600, 1100, 1200, 1200, 1100, 700, 300, 200, 200 };
            double[] pntY = new double[] { 100, 100, 100, 200, 400, 500, 500, 500, 400, 200 };

            for (int i = 0; i < 10; i++)
            {
                AgvMapPath path = new AgvMapPath();
                path.PathId = i;

                path.Position1 = new Point(pntX[i], pntY[i]);
                if (i < 10 - 1)
                {
                    path.Position2 = new Point(pntX[i + 1], pntY[i + 1]);
                }
                else
                {
                    path.Position2 = new Point(pntX[0], pntY[0]);
                }
                _paths.Add(path);

                AgvMapCard card = new AgvMapCard();
                card.CardId = i;
                card.Position = new Point(pntX[i], pntY[i]);
                _cards.Add(card);
            }
        }

        public Point getCardPosition(int cardId)
        {
            foreach (AgvMapCard card in _cards)
            {
                if (card.CardId == cardId)
                {
                    return card.Position;
                }
            }
            return new Point();
        }

    }
}
