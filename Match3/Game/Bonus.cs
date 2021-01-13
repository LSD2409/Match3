using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3
{
    abstract class Bonus : Entity
    {
        public bool IsNeedToUse { get; set; }
        public abstract void Use();

    }

    public enum BonusType
    {
        Bomb,
        Line
    }
}
