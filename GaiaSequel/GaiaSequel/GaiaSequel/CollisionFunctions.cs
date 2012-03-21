using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GaiaSequel
{
    class CollisionFunctions
    {
        //checking to see if the bounding box of attack hits the enemy
        public bool attackingOtherHit(Rectangle oneAttacking,int attackRange ,int attackingDir, Rectangle beingAttacked){
            //returns true of the attack suceeeds in hiting the player in the correct direction
            switch (attackingDir)
            {
                case 0: return new Rectangle(oneAttacking.X , oneAttacking.Y + oneAttacking.Height/2 ,oneAttacking.Width,attackRange*2).Intersects(beingAttacked);
                case 1: return new Rectangle(oneAttacking.X + oneAttacking.Width/2, oneAttacking.Y ,attackRange*2,oneAttacking.Height).Intersects(beingAttacked);
                case 2: return new Rectangle(oneAttacking.X + oneAttacking.Width / 2 - attackRange*2, oneAttacking.Y, attackRange * 2, oneAttacking.Height).Intersects(beingAttacked);

            }
            return new Rectangle(oneAttacking.X , oneAttacking.Y+oneAttacking.Height/2 - attackRange*2, oneAttacking.Width,attackRange*2).Intersects(beingAttacked); 
        }


        //This detects if the object is intersecting the other object
        public bool intersection(Rectangle collision1, Rectangle collision2)
        {
            return collision1.Intersects(collision2);
        }

        //This tests to see if the object is passed top the one inside of it
        public bool pastTop(Rectangle outside, Rectangle inside)
        {

            return ((inside.Y + inside.Height - 12) < outside.Y) && (outside.X < inside.X && (outside.X + outside.Width) > inside.X);

        }
        //This tests to see if the object is passed bottom the one inside of it
        public bool pastBottom(Rectangle outside, Rectangle inside)
        {
            return ((inside.Y + inside.Height) > (outside.Y + outside.Height - 6)) && (outside.X < inside.X && (outside.X + outside.Width) > inside.X);
        }
        //This tests to see if the object is passed right the one inside of it
        public bool pastRight(Rectangle outside, Rectangle inside)
        {
            return (inside.X + 20) > (outside.X + outside.Width);
        }
        //This tests to see if the object is passed left the one inside of it
        public bool pastLeft(Rectangle outside, Rectangle inside)
        {
            return (inside.X + inside.Width - 20) < outside.X;
        }
        //Calculates the rectangle ahead depending on the movement speed given by y and x
        //and then returns it.
        public Rectangle rectAhead(Vector2 position, Rectangle spriteRect, int x, int y)
        {
            return new Rectangle((int)position.X + x, (int)position.Y + y, spriteRect.Width, spriteRect.Height);
        }
        
    }
}
