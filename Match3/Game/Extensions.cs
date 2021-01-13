using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Match3
{
    static class Extensions
    {
        public static bool IsEmpty(this List<Entity> entities)
        {
            if (entities.Count == 0)
                return true;
            else 
                return false;
        }

        public static List<GameElement> ToGameElementsList(this IEnumerable<Entity> entities)
        {
            List<GameElement> result = new List<GameElement>();

            foreach(Entity entity in entities)
            {
                if (entity is GameElement)
                    result.Add(entity as GameElement);
            }

            return result;
        }

        public static List<Bonus> ToBonusElementsList(this IEnumerable<Entity> entities)
        {
            List<Bonus> result = new List<Bonus>();

            foreach (Entity entity in entities)
            {
                if (entity is Bomb)
                    result.Add(entity as Bomb);
            }

            return result;
        }

        public static List<Entity> GetMatchs(this List<Entity> entities, GameElement element)
        {
            List<Entity> result = new List<Entity>();

            foreach (Entity entity in entities)
            {
                if (entity is GameElement)
                {
                    GameElement gameElement = entity as GameElement;

                    if (gameElement.ElementType == element.ElementType)
                        result.Add(gameElement as GameElement);

                    else if (gameElement.ElementType != element.ElementType && !result.IsEmpty())
                    {
                        if (result.Contains(element as GameElement))
                            break;
                        else
                            result.Clear();
                    }
                }
                else if (entity is Bonus)
                    result.Clear();
            }

            if (result.Count < 3)
                return new List<Entity> { element};
            else
                return result;
        } 

        public static void AddList(this List<Entity> toAddList, List<Entity> addList)
        {
            foreach(Entity gameElement in addList)
            {
                toAddList.Add(gameElement as GameElement);
            }
        }

        public static bool IsOneTypeElements(this List<GameElement> gameElements)
        {
            ElementType type = gameElements[0].ElementType;

            foreach(GameElement element in gameElements)
            {
                if (element.ElementType != type)
                    return false;
            }

            return true;
        }

        public static List<Entity> GetEntitiesWithDelay(this List<Entity> entities)
        {
            return entities.Where(x => x.Delay > -1).ToList();
        }
    }
}
