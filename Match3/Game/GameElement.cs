using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Match3
{
    class GameElement : Entity
    {
        public bool WasMove { get; set; }

        public ElementType ElementType { get; private set; } 

        public GameElement(int rowNumb, int colNumb)
        {
            random = new Random();

            Delay = -1;

            ElementType = (ElementType)random.Next(0, 5);
            SetSprite(ElementType);

            RowNumb = rowNumb;
            ColNumb = colNumb;

            LastRow = rowNumb;
            LastColumn = colNumb;

            IsTurningBack = false;
            IsExpired = false;
            WasMove = false;

            animationNumb = 1;
        }   
        
        public override void Update()
        {
            WasMove = false;

            base.SetAnimation();

            base.ProcessAnimation();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);   
        }

        private void SetSprite(ElementType elementType) 
        {
            switch(elementType)
            {
                case ElementType.Type1:
                    Sprite = Art.Type1[animationNumb];
                    color = new Color(Color.DodgerBlue, 1f);
                    break;
                case ElementType.Type2:
                    Sprite = Art.Type2[animationNumb];
                    color = new Color(Color.LightGreen, 1f);
                    break;
                case ElementType.Type3:
                    Sprite = Art.Type3[animationNumb];
                    color = new Color(Color.LightPink, 1f);
                    break;
                case ElementType.Type4:
                    Sprite = Art.Type4[animationNumb];
                    color = new Color(Color.Red, 1f);
                    break;
                case ElementType.Type5:
                    Sprite = Art.Type5[animationNumb];
                    color = new Color(Color.LightYellow, 1f);
                    break;
            }   
        }

        protected override void ResetAnimation()
        {
            animationNumb = 1;
            SetSprite(this.ElementType);
        }

        protected override void SelectedAnimation()
        {
            if (animationNumb == 59)
                animationNumb = 1;

            animationNumb++;

            SetSprite(this.ElementType);
        }
        
        protected override void MoveAnimation()
        {
            ChangePosition(3f);

            if (Position == TargetPostion)
            {
                WasMove = true;
                
                if (!IsTurningBack && !IsLowering)
                {
                    MatchSearcher.FindMatchs(this);

                    if (EntityManager.Entities.Where(x => x.IsExpired).ToList().Count == 0)
                    {
                        IsTurningBack = true;
                    }   
                }
                else
                {
                    IsTurningBack = false;
                    IsLowering = false;

                    EntityManager.Unlock();
                }

                List<GameElement> turningBackElements = EntityManager.Entities.ToGameElementsList().Where(x => x.IsTurningBack).ToList();

                if (turningBackElements.Count > 1)
                {
                    EntityManager.SwitchElements(turningBackElements[0], turningBackElements[1]);
                }

            }
        }
    }

    enum ElementType
    {
        Type1,
        Type2,
        Type3,
        Type4,
        Type5
    }
}
