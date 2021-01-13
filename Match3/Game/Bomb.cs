using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Match3 
{
    class Bomb : Bonus
    {
        public Bomb(GameElement element)
        {
            Position = element.Position;
            TargetPostion = element.TargetPostion;

            RowNumb = element.RowNumb;
            ColNumb = element.ColNumb;

            LastColumn = ColNumb;
            LastRow = LastRow;

            color = element.color;

            IsNeedToUse = false;
            IsExpired = false;
            IsLowering = false;
            IsTurningBack = false;

            animation = Animation.None;

            Sprite = Art.Bomb;
        }

        public override void Update()
        {
            base.Update();
        }

        protected override void ResetAnimation()
        {
            Sprite = Art.Bomb;
        }

        protected override void SelectedAnimation()
        {
            Sprite = Art.BombSelected;
        }

        protected override void MoveAnimation()
        {
            ChangePosition(3f);

            if (Position == TargetPostion && IsLowering)
            {
                IsLowering = false;
                EntityManager.Unlock();
            }

            if (Position == TargetPostion && IsTurningBack)
            {
                IsTurningBack = false;
                EntityManager.Unlock();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Use()
        {
            this.IsExpired = true;

            foreach(Entity entity in EntityManager.Entities)
            {
                if (entity.RowNumb == RowNumb - 1 && entity.ColNumb == ColNumb - 1)
                    entity.Delay = 250;
                if (entity.RowNumb == RowNumb - 1 && entity.ColNumb == ColNumb)
                    entity.Delay = 250;
                if (entity.RowNumb == RowNumb - 1 && entity.ColNumb == ColNumb + 1)
                    entity.Delay = 250;
                if (entity.RowNumb == RowNumb + 1 && entity.ColNumb == ColNumb - 1)
                    entity.Delay = 250;
                if (entity.RowNumb == RowNumb + 1 && entity.ColNumb == ColNumb)
                    entity.Delay = 250;
                if (entity.RowNumb == RowNumb + 1 && entity.ColNumb == ColNumb + 1)
                    entity.Delay = 250;
                if (entity.RowNumb == RowNumb && entity.ColNumb == ColNumb - 1)
                    entity.Delay = 250;
                if (entity.RowNumb == RowNumb  && entity.ColNumb == ColNumb + 1)
                    entity.Delay = 250;
            }
        }
    }

}
