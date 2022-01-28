using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TRexGame.Entities
{
    public interface ICollidable
    {
        Rectangle CollisionBox { get;  }
    }
}
