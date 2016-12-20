using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValeurVoleur
{
    public class Tuyau : DessinGameplay, IAutoMove
    {
        public enum SourceTuyau
        {
            Plafond,
            Plancher
        }

        public readonly SourceTuyau Source;
        private readonly int _hauteur;
        //public const int cstLargeurBouche = 8;
        //public const int cstHauteurBouche = 2;
        //public const int cstLargeurTube = 6;
        public const int cstLargeur = 6;
        public const int cstStaticFrames = 4;
        static char[] ligneBouche = { '█', '█', '█', '█', '█', '█', '█', '█' };
        static char[] ligneTube = { '█', '█', '█', '█', '█', '█' };
        static char[] charsAnimation = { 'R', 'e', 'v', 'e', 'n', 'u', 'Q', 'u', 'é', 'b', 'e', 'c' };

        public Tuyau(int hauteur, SourceTuyau source, Point pointDepart)
            : base(pointDepart)
        {
            this._hauteur = hauteur;
            this.Source = source;
            this.Frames = this.GenererLignes();
            this.Hitboxes = this.Frames.Select(frame => this.GetLimitPoints(frame)).ToArray();
        }

        private Tuple<Point, char[]>[][] GenererLignes()
        {

            Tuple<Point, char[]>[][] lignes = new Tuple<Point, char[]>[charsAnimation.Length * cstStaticFrames][];
            //int courant;
            //int lNum;
            int sens = 0;
            //if (this.Source == SourceTuyau.Plafond)
            //{
            //    sens = charsAnimation.Length - 1;
            //}

            for (int i = 0; i < lignes.GetLength(0); i += cstStaticFrames)
            {
                lignes[i] = new Tuple<Point, char[]>[this.Hauteur];
                for (int j = 0; j < this.Hauteur; j++)
                {
                    char[] ligne = new char[this.Largeur];
                    for (int k = 0; k < ligne.Length; k++)
                    {
                        ligne[k] = charsAnimation[(i / cstStaticFrames + j) % charsAnimation.Length];
                        //if (k+1 < ligne.Length)
                        //{
                        //    ligne[k + 1] = ' ';
                        //}
                    }
                    lignes[i][j] = new Tuple<Point, char[]>(new Point(0, j), ligne);
                }

                for (int i2 = 1; i2 < cstStaticFrames; i2++)
                {
                    lignes[i + i2] = lignes[i];
                }
                //lignes[i] = new Tuple<Point, char[]>[this.Hauteur];
                //lNum = 0;
                //for (; lNum < lignes[i].Length - cstHauteurBouche - 1; lNum++)
                //{
                //    courant = Math.Abs(sens - lNum);
                //    lignes[i][courant] = Tuple.Create(new Point((this.Largeur - cstLargeurTube) / 2, courant), ligneTube);
                //}

                //for (; lNum < lignes[i].Length - 1; lNum++)
                //{
                //    courant = Math.Abs(sens - lNum);
                //    lignes[i][courant] = Tuple.Create(new Point((this.Largeur - cstLargeurBouche) / 2, courant), ligneBouche);
                //}
            }

            //for (int i = 0; i < charsAnimation.Length; i++)
            //{
            //    List<char> animation = new List<char>();
            //    animation.Add(charsAnimation[i]);
            //    while (animation.Count + 2 <= cstLargeurBouche)
            //    {
            //        animation.AddRange(new[] { ' ', charsAnimation[i] });
            //    }
            //    courant = Math.Abs(sens - this.Hauteur + 1);
            //    lignes[i * 2][courant] = Tuple.Create(new Point((this.Largeur - cstLargeurBouche) / 2, courant), animation.ToArray());
            //    lignes[i * 2 + 1][courant] = lignes[i * 2][courant];
            //    lignes[(i + charsAnimation.Length) * 2][courant] = Tuple.Create(new Point((this.Largeur - cstLargeurBouche) / 2 + 1, courant), animation.ToArray());
            //    lignes[(i + charsAnimation.Length) * 2 + 1][courant] = lignes[(i + charsAnimation.Length) * 2][courant];
            //}

            if (this.Source == SourceTuyau.Plafond)
            {
                return lignes.Reverse().ToArray();
            }
            return lignes;
        }

        public override int Largeur
        {
            //get { return Math.Max(cstLargeurBouche, cstLargeurTube); }
            get { return cstLargeur; }
        }

        public override int Hauteur
        {
            get { return this._hauteur; }
        }

        public bool AutoMove()
        {
            return this.SetPositionCourante(this.PositionCourante.Offset(-1, 0));
        }
    }
}
