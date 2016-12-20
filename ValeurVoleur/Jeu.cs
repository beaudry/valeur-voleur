using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValeurVoleur
{
    public class Jeu
    {
        public const int Largeur = 60;
        public const int Hauteur = 50;
        static Random rng = new Random();
        static char[] emptyBuffer;
        static int Score = 0;

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.WindowWidth = Jeu.Largeur;
            Console.WindowHeight = Jeu.Hauteur;
            char[] buffer = new char[Jeu.Hauteur * Jeu.Largeur];
            emptyBuffer = new char[buffer.Length];
            for (int i = 0; i < emptyBuffer.Length; i++)
            {
                emptyBuffer[i] = ' ';
            }
            ViderBuffer(ref buffer);

            IReadOnlyDictionary<ConsoleKey, Dessin.Deplacement> controles = new Dictionary<ConsoleKey, Dessin.Deplacement>()
            {
                { (ConsoleKey) 0, Dessin.Deplacement.Aucun },
                { ConsoleKey.DownArrow, Dessin.Deplacement.Bas },
                { ConsoleKey.LeftArrow, Dessin.Deplacement.Gauche },
                { ConsoleKey.RightArrow, Dessin.Deplacement.Droite },
                { ConsoleKey.UpArrow, Dessin.Deplacement.Haut }
            };

            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            const string messageDepart = "Appuyez sur une touche pour commencer!";
            Console.SetCursorPosition(Jeu.Largeur / 2 - messageDepart.Length / 2, Console.WindowHeight / 2 - 1);
            Console.Write(messageDepart);
            //Console.ReadKey();
            ConsoleKey curKey = 0;
            ConsoleKey tmpKey = curKey;
            while (true)
            {
                LinkedList<DessinGameplay> obstacles = new LinkedList<DessinGameplay>();

                //AjouterTuyaux(obstacles);
                //AjouterTuyaux(obstacles, Jeu.Largeur / 3);
                //AjouterTuyaux(obstacles, 2 * Jeu.Largeur / 3);
                //AjouterTuyaux(obstacles, 4 * Jeu.Largeur / 3);
                LinkedListNode<DessinGameplay> iter;
                Queue<LinkedListNode<DessinGameplay>> suppression = new Queue<LinkedListNode<DessinGameplay>>();

                Coureur coureur = new Coureur(new Point(0, Jeu.Hauteur / 2 - Coureur.cstHauteur / 2));
                bool gameOver = false;
                int frameNo = 0;
                int tuyauInterval = 45;
                int fps = 60;
                int speed = 1;
                Jeu.Score = 0;
                while (!gameOver)
                {

                    if (Console.KeyAvailable && controles.ContainsKey(curKey = Console.ReadKey(false).Key))
                    {
                        coureur.Deplacer(controles[curKey]);
                        while (Console.KeyAvailable)
                        {
                            Console.ReadKey(false);
                        }
                    }

                    coureur.Dessiner(ref buffer);
                    iter = obstacles.First;
                    while (iter != null)
                    {
                        if (iter.Value is IAutoMove)
                        {
                            ((IAutoMove)(iter.Value)).AutoMove();
                        }

                        if (!iter.Value.Dessiner(ref buffer))
                        {
                            if (iter.Value.PositionCourante.X < 0)
                            {
                                suppression.Enqueue(iter);
                            }
                        }

                        iter = iter.Next;
                    }

                    while (suppression.Count > 0)
                    {
                        obstacles.Remove(suppression.Dequeue());
                    }

                    foreach (var obstacle in obstacles)
                    {
                        if (coureur.HasCollided(obstacle))
                        {
                            gameOver = true;
                            System.Threading.Thread.Sleep(100);
                            break;
                        }
                    }

                    if (frameNo % tuyauInterval == 0)
                    {
                        AjouterTuyaux(obstacles);
                        Jeu.Score += 100;
                    }
                    ShowScore(ref buffer);

                    Console.SetCursorPosition(0, 0);
                    Console.Write(buffer);
                    ViderBuffer(ref buffer);


                    frameNo += speed;

                    System.Threading.Thread.Sleep(1000 / fps);
                }

                Console.Clear();

                Console.Write("Game Over! Appuyez sur une touche pour rejouer!");
                Console.ReadKey();
            }
        }

        static void ShowScore(ref char[] buffer)
        {
            //int log = (int)Math.Floor(Math.Log10(Jeu.Score));

            //char[] displayScore = new char[1+log + log/2]
            string formatte = String.Format("{0:n0}", Jeu.Score);
            Array.Copy(formatte.ToCharArray(), 0, buffer, Jeu.Largeur * 2 - formatte.Length, formatte.Length);
        }

        static void ViderBuffer(ref char[] buffer)
        {
            Array.Copy(emptyBuffer, buffer, Jeu.Hauteur * Jeu.Largeur);
        }

        static void AjouterTuyaux(LinkedList<DessinGameplay> collection)
        {
            AjouterTuyaux(collection, Jeu.Largeur);
        }

        static void AjouterTuyaux(LinkedList<DessinGameplay> collection, int positionHorizontale)
        {
            Point depart = new Point(Jeu.Largeur, Jeu.Hauteur);
            collection.AddLast(new Tuyau(rng.Next(Jeu.Hauteur / 6, Jeu.Hauteur / 2), Tuyau.SourceTuyau.Plafond, new Point(positionHorizontale, 0)));
            int hauteur = 2 * Jeu.Hauteur / 3 - collection.Last.Value.Hauteur;
            collection.AddLast(new Tuyau(hauteur, Tuyau.SourceTuyau.Plancher, new Point(positionHorizontale, Jeu.Hauteur - hauteur)));
        }
    }
}
