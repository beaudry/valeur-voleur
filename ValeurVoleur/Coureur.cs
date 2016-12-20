using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValeurVoleur
{
    public class Coureur : DessinGameplay
    {
        //        public const int cstLargeur;
        //        public const int cstHauteur;
        //        @" ()
        //         ┌─┼─┘
        //           ^
        //          / \";

        public const int cstLargeur = 10;
        public const int cstHauteur = 10;
        public const int cstDistanceDeplacement = 4;
        private List<Tuple<Point, char[]>>[] animation = new List<Tuple<Point, char[]>>[]
        {
            new List<Tuple<Point, char[]>>()
            {
                Tuple.Create(new Point(4, 0),     "╔╗".ToCharArray()),
                Tuple.Create(new Point(4, 1),     "╚╝".ToCharArray()),
                Tuple.Create(new Point(4, 2),     "┌┐".ToCharArray()),
                Tuple.Create(new Point(4, 3),     "║│".ToCharArray()),
                Tuple.Create(new Point(4, 4),     "║│".ToCharArray()),
                Tuple.Create(new Point(3, 5),    "($))".ToCharArray()),
                Tuple.Create(new Point(4, 6),     "└┘".ToCharArray()),
                Tuple.Create(new Point(4, 7),     "║║".ToCharArray()),
                Tuple.Create(new Point(4, 8),     "║║".ToCharArray()),
                Tuple.Create(new Point(4, 9),     "╚╚".ToCharArray()),
            },
            new List<Tuple<Point, char[]>>()
            {
                Tuple.Create(new Point(4, 0),     "╔╗".ToCharArray()),
                Tuple.Create(new Point(4, 1),     "╚╝".ToCharArray()),
                Tuple.Create(new Point(4, 2),     "┌┐ ($)".ToCharArray()),
                Tuple.Create(new Point(1, 3),  "╔══││══╝".ToCharArray()),
                Tuple.Create(new Point(0, 4), "($) ││".ToCharArray()),
                Tuple.Create(new Point(4, 5),     "││".ToCharArray()),
                Tuple.Create(new Point(0, 6), "╔═══└┘═══╝".ToCharArray()),
            }
        };

        private int frameDuration = 6;

        public Coureur(Point positionDepart)
            : base(positionDepart)
        {
            this.Frames = new IReadOnlyList<Tuple<Point, char[]>>[this.animation.Length * this.frameDuration];
            this.Hitboxes = new Tuple<Point, Point>[this.Frames.Length];

            Tuple<Point, Point> hitboxCourant;

            for (int i = 0; i < this.animation.Length; i++)
            {
                hitboxCourant = this.GetLimitPoints(this.animation[i]);
                for (int j = 0; j < this.frameDuration; j++)
                {
                    this.Frames[i * frameDuration + j] = this.animation[i];
                    this.Hitboxes[i * frameDuration + j] = hitboxCourant;
                }
            }

            this.Frames = this.Frames;
        }

        public override int Largeur
        {
            get { return cstLargeur; }
        }

        public override int Hauteur
        {
            get { return cstHauteur; }
        }

        public bool Deplacer(Deplacement deplacement)
        {
            int x = this.PositionCourante.X;
            int y = this.PositionCourante.Y;

            switch (deplacement)
            {
                case Deplacement.Haut:
                    y += -cstDistanceDeplacement;
                    break;
                case Deplacement.Bas:
                    y += cstDistanceDeplacement;
                    break;
                case Deplacement.Gauche:
                    x += -cstDistanceDeplacement;
                    break;
                case Deplacement.Droite:
                    x += cstDistanceDeplacement;
                    break;
                case Deplacement.Aucun:
                default:
                    break;
            }
            
            x = Math.Max(Math.Min(x, Jeu.Largeur - this.Largeur), 0);
            y = Math.Max(Math.Min(y, Jeu.Hauteur - this.Hauteur), 0);
            return this.SetPositionCourante(new Point(x,y));
        }
    }
}
