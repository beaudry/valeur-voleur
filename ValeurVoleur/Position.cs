using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValeurVoleur
{
    public struct Point : System.IEquatable<Point>
    {
        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public readonly int X;
        public readonly int Y;

        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return this.Equals((Point)obj);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + this.X.GetHashCode();
                hash = hash * 23 + this.X.GetHashCode();
                return hash;
            }
        }

        public bool Equals(Point other)
        {
            return this.Equals(other.X, other.Y);
        }

        public bool Equals(int x, int y)
        {
            return this.X == x && this.Y == y;
        }

        public Point Offset(int xOffset, int yOffset, bool flip)
        {
            if (flip)
            {
                return this.Offset(yOffset, xOffset);
            }
            return this.Offset(xOffset, yOffset);
        }

        public Point Offset(int xOffset, int yOffset)
        {
            return new Point(this.X + xOffset, this.Y + yOffset);
        }

        public Point Offset(Point pointRelatif)
        {
            return this.Offset(pointRelatif.X, pointRelatif.Y);
        }

        public bool EstInvisible()
        {
            return this.X < 0 || this.X >= Jeu.Largeur || this.Y < 0 || this.Y >= Jeu.Hauteur;
        }
    }
}
