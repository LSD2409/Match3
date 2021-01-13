using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Match3
{
    static class EntityManager
    {
        public static List<Entity> Entities { get; private set; }

        private static bool IsLock { get; set; }
        public static int ColumnCount { get; private set; }
        public static int RowCount { get; private set; }

        static EntityManager()
        {
            Entities = new List<Entity>();
            ColumnCount = 8;
            RowCount = 8;
            IsLock = false;
        }

        public static void Update(GameTime gameTime)
        {
            if (Entities.Count < 1)
                CreateGameElements();

            if(!IsLock)
            {
                Input.Update(gameTime);

                if (Input.WasClick())
                    ProcessClick();

                SearchForLowering();
            }

            foreach(Entity entity1 in Entities)
            {
                entity1.Update();
            }

            if (!IsLock)
            {
                SearchForAdding();

                GameStatus.IncreaseScores(GetExpiredElements());
                
                if(Entities.GetEntitiesWithDelay().Count > 0)
                {
                    Lock();
                    foreach(Entity entity in Entities.GetEntitiesWithDelay())
                    {
                        entity.Delay -= gameTime.ElapsedGameTime.TotalMilliseconds;

                        if(entity.Delay <= 0)
                        {
                            entity.Delay = -1;
                            entity.IsExpired = true;
                            Unlock();
                        }    
                    }
                }

                Entities = Entities.Where(x => !x.IsExpired).ToList();
            }

            if (!(Entities.Count < RowCount * ColumnCount) && !IsLock)
            {
                MatchSearcher.Update();
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Entity entity in Entities)
            {
                entity.Draw(spriteBatch);
            }
        }

        public static void CreateBonus(GameElement gameElement,BonusType type)
        {
            if (type == BonusType.Bomb)
            {
                Bomb bomb = new Bomb(gameElement);
                Entities.Add(bomb as Bomb);
            }
            else if(type == BonusType.Line)
            {

            }
        }

        private static void ProcessClick() // обработка клика мыши // добавить обработку щелчка не по игровой области когда уже есть выделенный элемент
        {
            List<Entity> selectedEntities = Entities.Where(x => x.IsSelected).ToList();

            if(selectedEntities.Count > 0) //если выбрана хоть одна
            {
                foreach (Entity entity in Entities)
                {
                    if (entity.WasSelected(Input.GetMouseState()))
                    {
                        entity.IsSelected = true;
                        
                        selectedEntities = Entities.Where(x => x.IsSelected).ToList();

                        if (selectedEntities.Count == 2 && selectedEntities[0].IsNearby(selectedEntities[1]))
                        {
                            SwitchElements(selectedEntities[0], selectedEntities[1]);
                            break;
                        }
                        else
                        {
                            foreach (Entity selectedEntity in selectedEntities)
                            {
                                selectedEntity.IsSelected = false;
                            }
                        }
                    }
                }
            }
            else // если ничего не выбрано
            {
                foreach(Entity entity in Entities)
                {
                    if (entity.WasSelected(Input.GetMouseState()))
                    {
                        entity.IsSelected = true;
                    }
                    else
                        entity.IsSelected = false;
                }
            }
        }

        private static void SearchForLowering()
        {
            for(int i = 0; i < ColumnCount; i++)
            {
                List<Entity> elements = GetColumn(i).OrderByDescending(x => x.RowNumb).ToList();

                if(elements.Count < ColumnCount)
                {
                    if(elements.First().RowNumb < RowCount - 1)
                    {
                        LowerElement(elements[0], RowCount - 1);
                    }

                    for(int j = 1; j < elements.Count; j++)
                    {
                        if((elements[j - 1].RowNumb - elements[j].RowNumb) > 1)
                        {
                            LowerElement(elements[j], elements[j].RowNumb + 1);
                        }
                    }
                }
            }
        }

        private static void SearchForAdding()
        {
            for (int i = 0; i < ColumnCount; i++)
            {
                List<Entity> elements = GetColumn(i).ToList();

                if (elements.Count < ColumnCount)
                {
                    AddGameElement(0, i);
                }
            }
        }

        private static void LowerElement(Entity element, int rowNumb)
        {
            element.TargetPostion = GetPosition(element.ColNumb, rowNumb, element.Size);

            element.LastRow = element.RowNumb;
            element.RowNumb = rowNumb;

            element.IsLowering = true;
        }

        private static void AddGameElement(int rowNumb, int colNumb)
        {
            GameElement gameElement = new GameElement(rowNumb, colNumb);
            gameElement.Position = GetPosition(colNumb, rowNumb, gameElement.Size);
            gameElement.TargetPostion = gameElement.Position;
            Entities.Add(gameElement as GameElement);
        }

        private static void CreateGameElements() // создание первоначального набора элементов
        {
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    AddGameElement(j, i);
                }
            }
        }

        public static Vector2 GetPosition(int rowNumb, int colNumb, Vector2 size) // вычисление позиции для нового элемента
        {
            Vector2 result = new Vector2();

            result.X = ((WindowSetting.Width - RowCount * size.Y) / 2) + rowNumb * size.Y;  
            result.Y = ((WindowSetting.Height - ColumnCount * size.X) / 2) + colNumb * size.X;

            return result;
        }

        public static void SwitchElements(Entity element1, Entity element2) // смена элементов местами
        {
            element1.LastColumn = element1.ColNumb;
            element1.LastRow = element1.RowNumb;

            element2.LastColumn = element2.ColNumb;
            element2.LastRow = element2.RowNumb;
            
            element1.TargetPostion = element2.Position;
         
            element1.ColNumb = element2.ColNumb;
            element1.RowNumb = element2.RowNumb;

            element2.TargetPostion = element1.Position;

            element2.ColNumb = element1.LastColumn;
            element2.RowNumb = element1.LastRow;

            element1.IsSelected = false;
            element2.IsSelected = false;

            if (element1 is Bonus)
            {
                Bonus bonus = element1 as Bonus;
                bonus.IsNeedToUse = true;
            }
            if (element2 is Bonus)
            {
                Bonus bonus = element2 as Bonus;
                bonus.IsNeedToUse = true;
            }
        }

        public static void Lock()
        {
            IsLock = true; 
        }

        public static void Unlock()
        {
            IsLock = false;
        }

        public static List<Entity> GetColumn(int columnNumb)
        {
            return Entities.Where(x => x.ColNumb == columnNumb).OrderBy(x => x.RowNumb).ToList();
        }

        public static List<Entity> GetRow(int rowNumb)
        {
            return Entities.Where(x => x.RowNumb == rowNumb).OrderBy(x => x.ColNumb).ToList();
        }

        public static int GetExpiredElements()
        {
            return EntityManager.Entities.Where(x => x.IsExpired).ToList().Count;
        }

        public static List<Entity> GetExpiredList()
        {
            return Entities.Where(x => x.IsExpired).ToList();
        }
    }
}
