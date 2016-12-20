using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValeurVoleur
{
    public abstract class DessinGameplay : Dessin
    {
        public DessinGameplay(Point positionDepart)
            : base(positionDepart)
        {
        }

        public Tuple<Point, Point>[] Hitboxes { get; protected set; }

        public bool HasCollided(DessinGameplay item)
        {
            Tuple<Point, Point> otherHitbox = new Tuple<Point, Point>(
                item.Hitboxes[item.AnimationKey].Item1.Offset(item.PositionCourante),
                item.Hitboxes[item.AnimationKey].Item2.Offset(item.PositionCourante)
            );
            Tuple<Point, Point> thisHitbox = new Tuple<Point, Point>(
                this.Hitboxes[this.AnimationKey].Item1.Offset(this.PositionCourante),
                this.Hitboxes[this.AnimationKey].Item2.Offset(this.PositionCourante)
            );

            return
                thisHitbox.Item1.X >= otherHitbox.Item1.X &&
                thisHitbox.Item1.X <= otherHitbox.Item2.X &&
                thisHitbox.Item1.Y >= otherHitbox.Item1.Y &&
                thisHitbox.Item1.Y <= otherHitbox.Item2.Y ||
                thisHitbox.Item2.X >= otherHitbox.Item1.X &&
                thisHitbox.Item2.X <= otherHitbox.Item2.X &&
                thisHitbox.Item2.Y >= otherHitbox.Item1.Y &&
                thisHitbox.Item2.Y <= otherHitbox.Item2.Y;
        }

        protected Point GetMinimalPoint(IReadOnlyList<Tuple<Point, char[]>> frame)
        {
            return new Point(frame.Min(line => line.Item1.X), frame.Min(line => line.Item1.Y));
        }

        protected Point GetMaximalPoint(IReadOnlyList<Tuple<Point, char[]>> frame)
        {
            return new Point(frame.Max(line => line.Item1.X + line.Item2.Length), frame.Max(line => line.Item1.Y));
        }

        protected Tuple<Point, Point> GetLimitPoints(IReadOnlyList<Tuple<Point, char[]>> frame)
        {
            return Tuple.Create(this.GetMinimalPoint(frame), this.GetMaximalPoint(frame));
        }
    }
}
