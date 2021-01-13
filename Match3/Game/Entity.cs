using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace Match3
{
    public abstract class Entity
    {
        protected static Random random;

        protected Rectangle entityBounds = new Rectangle();
        public Color color = Color.White;

        protected Vector2 rotateCentr = Vector2.Zero;

        protected int animationNumb;

        private bool IsAnitmated { get; set; }//?
        public bool IsTurningBack { get; set; }
        public bool IsExpired { get; set; }

        public bool IsSelected { get; set; }
        protected float rotation = 0;
        public bool IsLowering { get; set; }

        protected Animation animation;

        private Vector2 position;
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                entityBounds.X = (int)value.X;
                entityBounds.Y = (int)value.Y;
            }
        }

        public Vector2 TargetPostion { get; set; }

        public int LastRow { get; set; }
        public int LastColumn { get; set; }

        public int RowNumb { get; set; }
        public int ColNumb { get; set; }

        private Texture2D sprite;
        protected Texture2D Sprite
        {
            get
            {
                return sprite;
            }
            set
            {
                sprite = value;
                entityBounds.Height = sprite.Height;
                entityBounds.Width = sprite.Width;
            }
        }

        public Vector2 Size
        {
            get
            {
                return new Vector2 { X = sprite.Height, Y = sprite.Width };
            }
        }

        public double Delay { get; set; }

        public virtual void Update()
        {
            SetAnimation();

            ProcessAnimation();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            switch (animation)
            {
                case Animation.Selected:
                    spriteBatch.Draw(sprite, Position, null, color, rotation, rotateCentr, 1, SpriteEffects.None, 0.5f);
                    break;
                case Animation.Destruction:
                    spriteBatch.Draw(Sprite, Position, null, color, rotation, rotateCentr, 1, SpriteEffects.None, 1);
                    break;
                case Animation.Moving:
                    spriteBatch.Draw(sprite, Position, null, color, 0, rotateCentr, 1, SpriteEffects.None, 0.5f);
                    break;
                case Animation.None:
                    spriteBatch.Draw(sprite, Position, null, color, 0, rotateCentr, 1, SpriteEffects.None, 0.5f);
                    break;
            }
        }

        protected virtual void SetAnimation()
        {
            if (Position != TargetPostion && !IsExpired)
            {
                animation = Animation.Moving;
                EntityManager.Lock();
            }
            else if (IsExpired)
            {
                if (TargetPostion.Y != WindowSetting.Height)
                {
                    TargetPostion = new Vector2(Position.X + (float)random.Next(-25, 25), WindowSetting.Height);
                }
                animation = Animation.Destruction;
                EntityManager.Lock();
            }
            else if (IsSelected)
            {
                animation = Animation.Selected;
            }
            else
            {
                animation = Animation.None;
            }
        }

        protected virtual void ProcessAnimation()
        {
            switch (animation)
            {
                case Animation.Destruction:

                    DesctructAnimation();
                    break;

                case Animation.Moving:

                    MoveAnimation();
                    break;
                case Animation.Selected:

                    SelectedAnimation();
                    break;

                case Animation.None:
                    ResetAnimation();
                    break;
            }
        }

        protected abstract void SelectedAnimation();

        protected virtual void DesctructAnimation()
        {
            if (Position.Y >= TargetPostion.Y)
            {
                EntityManager.Unlock();
                return;
            }

            ChangePosition(7f);
            ChangeRotate();
        }

        protected abstract void MoveAnimation();

        protected void ChangePosition(float acceleration)
        {
            if (Vector2.Distance(TargetPostion, Position) <= acceleration)
                acceleration = 1f;

            if (Position.X > TargetPostion.X)
            {
                Position = new Vector2(Position.X - acceleration, Position.Y);
            }

            if (Position.X < TargetPostion.X)
            {
                Position = new Vector2(Position.X + acceleration, Position.Y);
            }

            if (Position.Y > TargetPostion.Y)
            {
                Position = new Vector2(Position.X, Position.Y - acceleration);
            }

            if (Position.Y < TargetPostion.Y)
            {
                Position = new Vector2(Position.X, Position.Y + acceleration);
            }
        }

        protected void ChangeRotate()
        {
            if (rotation == 1f)
                rotation = 0;
            else
                rotation += 0.05f;
        }

        public bool WasSelected(Point point)
        {
            return entityBounds.Contains(point);
        }

        public bool IsNearby(Entity entity)
        {
            if (Math.Abs(this.RowNumb - entity.RowNumb) == 1 && Math.Abs(this.ColNumb - entity.ColNumb) == 0)
                return true;
            else if (Math.Abs(this.ColNumb - entity.ColNumb) == 1 && Math.Abs(this.RowNumb - entity.RowNumb) == 0)
                return true;
            else
                return false;
        }

        protected abstract void ResetAnimation();
    }

    public enum Animation
    {
        None,
        Moving,
        Destruction,
        Selected
    }
}
