using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValeurVoleur
{
    public abstract class Dessin
    {
        public enum Deplacement
        {
            Aucun = 0,
            Haut,
            Bas,
            Gauche,
            Droite
        }

        public Dessin(Point positionDepart)
        {
            this.PositionCourante = positionDepart;
            this.AnimationKey = 0;
        }

        public abstract int Largeur { get; }
        public abstract int Hauteur { get; }

        public Point PositionCourante { get; private set; }

        public bool FirstStepOfDisappearing { get; private set; }

        public int AnimationKey { get; protected set; }

        public bool EstInvisible()
        {
            return this.PositionCourante.X + this.Largeur < 0 ||
                this.PositionCourante.X >= Jeu.Largeur ||
                this.PositionCourante.Y + this.Hauteur < 0 ||
                this.PositionCourante.Y >= Jeu.Hauteur;
        }

        protected bool SetPositionCourante(Point nouvellePos){
            this.FirstStepOfDisappearing = nouvellePos.EstInvisible() && !PositionCourante.EstInvisible();
            this.PositionCourante = nouvellePos;
            return this.EstInvisible();
        }

        public bool Dessiner(ref char[] buffer)
        {
            if (this.EstInvisible())
            {
                return false;
            }

            IReadOnlyList<Tuple<Point, char[]>> lignes = this.Frames[this.AnimationKey];
            this.AnimationKey = (this.AnimationKey + 1) % this.Frames.Length;

            Point debutLigne;
            int depart;
            for (int i = 0; i < lignes.Count; i++)
            {
                debutLigne = this.PositionCourante.Offset(lignes[i].Item1);
                if (debutLigne.Y >= 0 && debutLigne.Y < Jeu.Hauteur)
                {
                    depart = Math.Max(debutLigne.X, 0);
                    if (depart - debutLigne.X < lignes[i].Item2.Length && debutLigne.X < Jeu.Largeur)
                    {
                        //Console.SetCursorPosition(depart, debutLigne.Y);
                        //for (int j = 0; j < Math.Min(debutLigne.X + this.Lignes[i].Item2.Length, Jeu.Largeur) - depart; j++)
                        //{
                        //    buffer[ + j] = this.Lignes[i].Item2[depart - debutLigne.X + j];
                        //}

                        Array.Copy(lignes[i].Item2, depart - debutLigne.X, buffer, debutLigne.Y * Jeu.Largeur + depart, Math.Min(debutLigne.X + lignes[i].Item2.Length, Jeu.Largeur) - depart);
                    }
                }
            }

            return true;
        }

        protected IReadOnlyList<Tuple<Point, char[]>>[] Frames { get; set; }
        //public bool Dessiner(Point position, PositionHorizontale departHorizontal, PositionVerticale departVertical);
    }
}
