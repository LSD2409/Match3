using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Match3
{
    static class MatchSearcher
    {
        private static List<Entity> sequence;

        static MatchSearcher()
        {
            sequence = new List<Entity>();
        }

        public static void Update()
        {
            
            foreach(Entity entity in EntityManager.Entities)
            {
                if (entity is Bonus)
                {
                    if (TryUse(entity as Bonus))
                        return;
                }
            }
            
            foreach (Entity entity in EntityManager.Entities)
            {
                if (entity is GameElement)
                {
                    FindMatchs(entity as GameElement);

                    if(EntityManager.GetExpiredElements() > 0)
                    {
                        SearchingForBonus();
                        sequence.Clear();
                        break;
                    }
                } 
                sequence.Clear();
            }
        }

        public static void FindMatchs(GameElement gameElement)
        {
            FindHorizontalMatch(gameElement);
            FindVericalMatch(gameElement);

            MarkExpirder();
        }

        private static bool TryUse(Bonus bonus)
        {
            if (bonus.IsNeedToUse)
            {
                bonus.Use();
                return true;
            }
            return false;
        }

        private static void FindHorizontalMatch(GameElement gameElement)
        {
            sequence = EntityManager.Entities.Where(x => x.RowNumb == gameElement.RowNumb).OrderBy(x => x.ColNumb).ToList();
            sequence = sequence.GetMatchs(gameElement);
        }

        private static void FindVericalMatch(GameElement gameElement)
        {
            List<Entity> result = new List<Entity>();

            foreach (Entity element in sequence)
            {
                List<Entity> elements = EntityManager.Entities.Where(x => x.ColNumb == gameElement.ColNumb).OrderBy(x => x.RowNumb).ToList();

                elements = elements.GetMatchs(gameElement as GameElement);

                if(elements.Count > 1)
                {
                    result.AddList(elements);
                    break;
                }
            }
            if (result.Count > 1)
                sequence.AddList(result);
        }

        private static void MarkExpirder()
        {
            if (sequence.Count > 1)
            {
                foreach (Entity entity in sequence)
                {
                    entity.IsExpired = true;
                }
            }
        }

        private static void SearchingForBonus()
        {
            if(EntityManager.GetExpiredElements() > 4)
            {
                List<Entity> entities = EntityManager.GetExpiredList();

                foreach(Entity entity in entities)
                {
                    if (entity is Bonus)
                        return;
                }

                List<GameElement> gameElements = entities.ToGameElementsList();

                if (gameElements.Where(x => x.WasMove).ToGameElementsList().Count > 0)
                    EntityManager.CreateBonus(gameElements.Where(x => x.WasMove).ToGameElementsList().Last(),BonusType.Bomb);
                else
                    EntityManager.CreateBonus(gameElements.Last(), BonusType.Bomb);
            }
        }
    }
    

}
