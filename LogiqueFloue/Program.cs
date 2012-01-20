using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationFloue
{
    public enum FHumidite
    {
        Sec,
        Humide,
        Trempe
    }

    public enum FTemperature
    {
        Froide,
        Douce,
        Normale,
        Chaude,
        Caniculaire
    }

    public enum FNappe
    {
        Insuffisant,
        Faible,
        Suffisant
    }

    public enum FDuree
    {
        Courte,
        Moyenne,
        Longue
    }

    public class Intervalle
    {
        public Intervalle(double left, double right)
        {
            this.Left = left;
            this.Right = right;
        }

        public double Left;
        public double Right;
    }

    public class Regle<T1, T2, T3>
    {
        public Regle(T1 key1, T2 key2, T3 value)
        {
            this.Key1 = key1;
            this.Key2 = key2;
            this.Value = value;
        }

        public T1 Key1;
        public T2 Key2;
        public T3 Value;
        public double Result;
    }

    class Program
    {
        static void Main(string[] args)
        {
            Boolean continu = true;
            String reponse = String.Empty;
            while (continu)
            {
                exec();
                Console.WriteLine("Continuer? (o/n)");
                reponse = Console.ReadLine();
                if (reponse.Equals("n"))
                {
                    continu = false;
                }
            }
            // Fin.
        }

        static void exec()
        {
            // Définition des ensembles.
            Dictionary<FTemperature, Intervalle> ensembleTemperature = new Dictionary<FTemperature, Intervalle>();
            ensembleTemperature.Add(FTemperature.Froide, new Intervalle(0, 5));
            ensembleTemperature.Add(FTemperature.Douce, new Intervalle(13, 13));
            ensembleTemperature.Add(FTemperature.Normale, new Intervalle(18, 22));
            ensembleTemperature.Add(FTemperature.Chaude, new Intervalle(26, 30));
            ensembleTemperature.Add(FTemperature.Caniculaire, new Intervalle(38, 45));

            Dictionary<FHumidite, Intervalle> ensembleHumidite = new Dictionary<FHumidite, Intervalle>();
            ensembleHumidite.Add(FHumidite.Sec, new Intervalle(0, 0.4));
            ensembleHumidite.Add(FHumidite.Humide, new Intervalle(0.6, 0.7));
            ensembleHumidite.Add(FHumidite.Trempe, new Intervalle(0.8, 1));

            Dictionary<FNappe, Intervalle> ensembleNappe = new Dictionary<FNappe, Intervalle>();
            ensembleNappe.Add(FNappe.Insuffisant, new Intervalle(0, 1));
            ensembleNappe.Add(FNappe.Faible, new Intervalle(1.5, 1.5));
            ensembleNappe.Add(FNappe.Suffisant, new Intervalle(2, 10));

            Dictionary<FDuree, Intervalle> ensembleDuree = new Dictionary<FDuree, Intervalle>();
            ensembleDuree.Add(FDuree.Courte, new Intervalle(0, 5));
            ensembleDuree.Add(FDuree.Moyenne, new Intervalle(10, 10));
            ensembleDuree.Add(FDuree.Longue, new Intervalle(30, 30));

            // Règles en dur.
            // TODO: Lecture JSON des règles.
            List<Regle<FHumidite, FTemperature, FDuree>> reglesHumiditeTemperature = new List<Regle<FHumidite, FTemperature, FDuree>>();
            reglesHumiditeTemperature.Add(new Regle<FHumidite, FTemperature, FDuree>(FHumidite.Sec, FTemperature.Froide, FDuree.Courte));
            reglesHumiditeTemperature.Add(new Regle<FHumidite, FTemperature, FDuree>(FHumidite.Sec, FTemperature.Douce, FDuree.Moyenne));
            reglesHumiditeTemperature.Add(new Regle<FHumidite, FTemperature, FDuree>(FHumidite.Sec, FTemperature.Normale, FDuree.Moyenne));
            reglesHumiditeTemperature.Add(new Regle<FHumidite, FTemperature, FDuree>(FHumidite.Sec, FTemperature.Chaude, FDuree.Longue));
            reglesHumiditeTemperature.Add(new Regle<FHumidite, FTemperature, FDuree>(FHumidite.Sec, FTemperature.Caniculaire, FDuree.Longue));
            reglesHumiditeTemperature.Add(new Regle<FHumidite, FTemperature, FDuree>(FHumidite.Humide, FTemperature.Douce, FDuree.Courte));
            reglesHumiditeTemperature.Add(new Regle<FHumidite, FTemperature, FDuree>(FHumidite.Humide, FTemperature.Normale, FDuree.Moyenne));
            reglesHumiditeTemperature.Add(new Regle<FHumidite, FTemperature, FDuree>(FHumidite.Humide, FTemperature.Chaude, FDuree.Moyenne));
            reglesHumiditeTemperature.Add(new Regle<FHumidite, FTemperature, FDuree>(FHumidite.Humide, FTemperature.Caniculaire, FDuree.Longue));
            reglesHumiditeTemperature.Add(new Regle<FHumidite, FTemperature, FDuree>(FHumidite.Trempe, FTemperature.Caniculaire, FDuree.Courte));

            List<Regle<FDuree, FNappe, FDuree>> reglesDureeNappe = new List<Regle<FDuree, FNappe, FDuree>>();
            reglesDureeNappe.Add(new Regle<FDuree, FNappe, FDuree>(FDuree.Courte, FNappe.Suffisant, FDuree.Courte));
            reglesDureeNappe.Add(new Regle<FDuree, FNappe, FDuree>(FDuree.Moyenne, FNappe.Faible, FDuree.Courte));
            reglesDureeNappe.Add(new Regle<FDuree, FNappe, FDuree>(FDuree.Moyenne, FNappe.Suffisant, FDuree.Moyenne));
            reglesDureeNappe.Add(new Regle<FDuree, FNappe, FDuree>(FDuree.Longue, FNappe.Faible, FDuree.Moyenne));
            reglesDureeNappe.Add(new Regle<FDuree, FNappe, FDuree>(FDuree.Longue, FNappe.Suffisant, FDuree.Longue));

            Console.WriteLine("Temperature ?");
            String temp = Console.ReadLine();
            double dtemp = Convert.ToDouble(temp);
            Console.WriteLine("Humidite ?");
            String humidite = Console.ReadLine();
            double dhumidite = Convert.ToDouble(humidite);
            Console.WriteLine("Nappe ?");
            String nappe = Console.ReadLine();
            double dnappe = Convert.ToDouble(nappe);

            // Fuzzification 1.
            Console.WriteLine("1er controleur");
            double dureeTheorique1 = FuzzificationDefuzzification<FTemperature,FHumidite,FDuree>(ensembleTemperature, ensembleHumidite, ensembleDuree, reglesHumiditeTemperature, dtemp, dhumidite);
            Console.WriteLine(""); 
            Console.WriteLine("2eme controleur");
            double dureeTheorique2 = FuzzificationDefuzzification<FNappe, FDuree, FDuree>(ensembleNappe, ensembleDuree, ensembleDuree, reglesDureeNappe, dnappe, dureeTheorique1);

            /*
            // Fuzzification 2.
            Dictionary<FNappe, double> ponderationsNappe = GetPonderations<FNappe>(ensembleNappe, dnappe);
            Dictionary<FDuree, double> ponderationsDuree = GetPonderations<FDuree>(ensembleDuree, dureeTheorique);

            foreach (KeyValuePair<FNappe, double> ponderation in ponderationsNappe)
            {
                Console.WriteLine("Pour la nappe: {0}, on a {1}.", ponderation.Key, ponderation.Value);
            }

            foreach (KeyValuePair<FDuree, double> ponderation in ponderationsDuree)
            {
                Console.WriteLine("Pour la durée: {0}, on a {1}.", ponderation.Key, ponderation.Value);
            }

            // Inférence/Agrégation.
            Dictionary<FDuree, double> dureesAgregees2 = InferenceAgregation<FDuree, FNappe, FDuree>(reglesDureeNappe, ponderationsDuree, ponderationsNappe);

            foreach (KeyValuePair<FDuree, double> agregation in dureesAgregees2)
            {
                Console.WriteLine("Valeur agrégée pour {0} : {1}", agregation.Key, agregation.Value);
            }
            
            //Defuzzification du controlleur 1
            double dureeTheorique2 = CentreDeGravite<FDuree>(dureesAgregees2, ensembleDuree);
            Console.WriteLine("Duree d'arrosage : {0}", dureeTheorique2);
            Console.WriteLine("");
            */
        }

        private static double FuzzificationDefuzzification<T1, T2, TResult>(Dictionary<T1, Intervalle> ensemble1, Dictionary<T2, Intervalle> ensemble2, Dictionary<TResult, Intervalle> ensembleResult, List<Regle<T2, T1, TResult>> regles, double data1, double data2)
        {
            Dictionary<T1, double> ponderations1 = GetPonderations<T1>(ensemble1, data1);
            Dictionary<T2, double> ponderations2 = GetPonderations<T2>(ensemble2, data2);

            foreach (KeyValuePair<T1, double> ponderation in ponderations1)
            {
                Console.WriteLine("Pour la premiere donnee: {0}, on a {1}.", ponderation.Key, ponderation.Value);
            }

            foreach (KeyValuePair<T2, double> ponderation in ponderations2)
            {
                Console.WriteLine("Pour la deuxieme donnee: {0}, on a {1}.", ponderation.Key, ponderation.Value);
            }

            // Inférence/Agrégation.
            Dictionary<TResult, double> dureesAgregees = InferenceAgregation<T2, T1, TResult>(regles, ponderations2, ponderations1);

            foreach (KeyValuePair<TResult, double> agregation in dureesAgregees)
            {
                Console.WriteLine("Valeur agrégée pour {0} : {1}", agregation.Key, agregation.Value);
            }

            //Defuzzification du controlleur 1
            double dureeTheorique = CentreDeGravite<TResult>(dureesAgregees, ensembleResult);
            Console.WriteLine("Duree : {0}", dureeTheorique);
            Console.WriteLine("");
            return dureeTheorique;
        }

        private static double CentreDeGravite<T>(Dictionary<T, double> dureesAgregees, Dictionary<T, Intervalle> ensembleDuree)
        {            
            List<double[]> coorList = new List<double[]>();
            for (int i = 0; i < Enum.GetValues(typeof(T)).Length; i++)
            {
                T key = (T)Enum.GetValues(typeof(T)).GetValue(i);

                Boolean previousFlou = false;
                Boolean nextFlou = false;
                Intervalle  previous = null;
                Intervalle next = null;

                if (i > 0)
                {
                    previous = ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i - 1)];
                    if(dureesAgregees.ContainsKey((T)Enum.GetValues(typeof(T)).GetValue(i - 1)))
                    {
                        if (dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i-1)] != 0.0)
                        {
                            previousFlou = true;
                        }
                    }
                }

                if (i < ensembleDuree.Keys.Count - 1)
                {
                    next = ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i + 1)];
                    if(dureesAgregees.ContainsKey((T)Enum.GetValues(typeof(T)).GetValue(i + 1)))
                    {
                        if (dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i + 1)] != 0.0)
                        {
                            nextFlou = true;
                        }
                    }
                }

                if(dureesAgregees.ContainsKey((T)Enum.GetValues(typeof(T)).GetValue(i)))
                {
                    if(dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i)] != 0.0)
                    {
                    double x;
                    double y;   
                    if(previousFlou==false)
                    {
                        double[] coordonne = new double[2];
                        if(previous!=null)
                        {
                            coordonne[0] = previous.Right;
                            coordonne[1] = 0;
                            coorList.Add(coordonne);
                            double[] coordonneAp = new double[2];
                            x= ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i-1)].Right +((ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i)].Left - ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i-1)].Right)*dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i)]);
                            coordonneAp[0] = x;
                            coordonneAp[1] = dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i)];
                            coorList.Add(coordonneAp);
                        }
                        else
                        {
                            coordonne[0] = ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i)].Left;
                            coordonne[1] = dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i)];
                            coorList.Add(coordonne);
                        }
                    }
                    else
                    {
                        x = ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i-1)].Right +((ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i)].Left - ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i-1)].Right)*dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i)]);
                        y = ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i)].Left - ((ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i)].Left - ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i-1)].Right)*dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i-1)]);
                     
                        //test segment 1
                        double[] inter = intersection(ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i - 1)].Right, 0, x, dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i)], y,dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i-1)],ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i)].Left, 0);
                        if(inter==null)
                        {
                            inter = intersection(ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i - 1)].Right, 0, x, dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i)], y, dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i - 1)], ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i - 1)].Left, dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i - 1)]);
                        }
                        if(inter!=null)
                        {
                            coorList.Add(inter);
                            double[] coordonneAp = new double[2];
                            coordonneAp[0] = ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i)].Left;
                            coordonneAp[1] = dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i)];
                            coorList.Add(coordonneAp);
                        }
                        else
                        {
                            inter = intersection(x, dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i)], ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i)].Right, dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i)], y, dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i - 1)], ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i)].Left, 0);
                            coorList.Add(inter);
                        }
                    }
                    if(nextFlou==false)
                    {
                        double[] coordonne = new double[2];
                        if(next!=null)
                        {
                            double[] coordonneAv = new double[2];
                            x = ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i + 1)].Left - ((ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i + 1)].Left - ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i)].Right) * dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i)]);
                            coordonneAv[0] = x;
                            coordonneAv[1] = dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i)];
                            coorList.Add(coordonneAv);
                            coordonne[0] = next.Left;
                            coordonne[1] = 0;
                            coorList.Add(coordonne);
                        }
                        else
                        {
                            coordonne[0] = ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i)].Right;
                            coordonne[1] = dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i)];
                            coorList.Add(coordonne);
                        }
                    }
                    else
                    {
                        x = ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i + 1)].Left - ((ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i + 1)].Left - ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i)].Right) * dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i)]);
                        y = ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i)].Right + ((ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i+1)].Left - ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i)].Right)*dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i+1)]);
                        //test segment 1
                        double[] inter = intersection(x,dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i)],ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i+1)].Left,0,ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i)].Right,0,y,dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i+1)]);
                        if(inter==null)
                        {
                            inter = intersection(x,dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i)],ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i+1)].Left,0,y,dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i+1)],ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i+1)].Right,dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i+1)]);
                        }
                        if(inter!=null)
                        {
                            double[] coordonneAp = new double[2];
                            coordonneAp[0] = x;
                            coordonneAp[1] = dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i)];
                            coorList.Add(coordonneAp);
                            coorList.Add(inter);
                        }
                        else
                        {
                             inter = intersection(ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i)].Left,dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i)],x,dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i)],ensembleDuree[(T)Enum.GetValues(typeof(T)).GetValue(i)].Right,0,y,dureesAgregees[(T)Enum.GetValues(typeof(T)).GetValue(i+1)]);
                             coorList.Add(inter);
                        }
                    }
                }}
            }
            double sommeNumerateur = 0;
            double sommeDenominateur = 0;
            for (int i = 0; i < coorList.ToArray().Length-1; i++)
            {
                double[] obj = coorList.ToArray()[i];
                double[] next=null;
                next = coorList.ToArray()[i + 1];
                if(i != coorList.ToArray().Length-2)
                {
                    Boolean continu = true;
                    while(continu)
                    {
                        if(next[0]==obj[0] && next[1] == obj[1])
                        {
                            coorList.RemoveAt(i+1);
                            next = coorList.ToArray()[i + 1];
                        }
                        else
                        {
                            continu = false;
                        }
                    }
                }
                sommeNumerateur += (next[0]-obj[0])*((2*next[0]+obj[0])*next[1]+(2*obj[0]+next[0])*obj[1]);
                sommeDenominateur += (next[0]-obj[0])*(next[1]+obj[1]);
            }
            if (sommeDenominateur != 0)
            {
                return sommeNumerateur / (3 * sommeDenominateur);
            }
            return 0;
            
        }

        static Dictionary<T, double> GetPonderations<T>(Dictionary<T, Intervalle> src, double value)
        {
            List<T> valeursFloues = new List<T>();

            for (int i = 0; i < Enum.GetValues(typeof(T)).Length; i++)
            {
                T key = (T)Enum.GetValues(typeof(T)).GetValue(i);

                Intervalle previous = null;
                Intervalle next = null;

                if (i > 0)
                {
                    previous = src[(T)Enum.GetValues(typeof(T)).GetValue(i - 1)];
                }

                if (i < src.Keys.Count - 1)
                {
                    next = src[(T)Enum.GetValues(typeof(T)).GetValue(i + 1)];
                }

                if ((value >= src[key].Left && value <= src[key].Right)
                    || (previous != null && value > previous.Right && value <= src[key].Left)
                    || (next != null && value < next.Left && value >= src[key].Right))
                {
                    valeursFloues.Add(key);
                }
            }

            Dictionary<T, double> ponderations = new Dictionary<T, double>();
            if (valeursFloues.Count == 1)
            {
                ponderations.Add(valeursFloues[0], 1);
            }
            else
            {
                ponderations.Add(valeursFloues[0], 1 - (value - src[valeursFloues[0]].Right) / (src[valeursFloues[1]].Left - src[valeursFloues[0]].Right));
                ponderations.Add(valeursFloues[1], 1 - (src[valeursFloues[1]].Left - value) / (src[valeursFloues[1]].Left - src[valeursFloues[0]].Right));
            }

            return ponderations;
        }

        static Dictionary<T3, double> InferenceAgregation<T1, T2, T3>(List<Regle<T1, T2, T3>> regles, Dictionary<T1, double> ponderations1, Dictionary<T2, double> ponderations2)
        {
            // Inférence (Mandani).
            foreach (Regle<T1, T2, T3> regle in regles)
            {
                if (ponderations1.ContainsKey(regle.Key1) && ponderations2.ContainsKey(regle.Key2))
                {
                    regle.Result = Math.Min(ponderations1[regle.Key1], ponderations2[regle.Key2]);
                }
                else
                {
                    regle.Result = 0;
                }
            }

            // Agrégation.
            Dictionary<T3, double> agregation = new Dictionary<T3, double>();
            foreach (T3 val in Enum.GetValues(typeof(T3)))
            {
                double maxVal = 0;
                foreach (Regle<T1, T2, T3> regle in regles)
                {
                    if (Enum.Equals(regle.Value, val) && regle.Result > maxVal)
                    {
                        maxVal = regle.Result;
                    }
                }

                agregation.Add(val, maxVal);
            }

            return agregation;
        }

        static double[] intersection(double Ax, double Ay, double Bx, double By, double Cx, double Cy, double Dx, double Dy)
        {
             double r = ((Ay-Cy)*(Dx-Cx)-(Ax-Cx)*(Dy-Cy))/((Bx-Ax)*(Dy-Cy)-(By-Ay)*(Dx-Cx));
             double s = ((Ay-Cy)*(Bx-Ax)-(Ax-Cx)*(By-Ay))/((Bx-Ax)*(Dy-Cy)-(By-Ay)*(Dx-Cx));
             if (r <= 1 && r >= 0 && s <= 1.0 && s >= 0.0)
             {
                 double[] result = new double[2];
                 result[0]=Ax+r*(Bx-Ax);
                 result[1]=Ay+r*(By-Ay);
                 return result;
             }
             return null;
        }
    }
}