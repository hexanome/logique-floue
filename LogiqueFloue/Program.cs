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

            // Fuzzification 1.
            Dictionary<FTemperature, double> ponderationsTemperature = GetPonderations<FTemperature>(ensembleTemperature, 6);
            Dictionary<FHumidite, double> ponderationsHumidite = GetPonderations<FHumidite>(ensembleHumidite, 0.5);

            foreach (KeyValuePair<FTemperature, double> ponderation in ponderationsTemperature)
            {
                Console.WriteLine("Pour la température: {0}, on a {1}.", ponderation.Key, ponderation.Value);
            }

            foreach (KeyValuePair<FHumidite, double> ponderation in ponderationsHumidite)
            {
                Console.WriteLine("Pour l'humidité: {0}, on a {1}.", ponderation.Key, ponderation.Value);
            }

            // Inférence/Agrégation.
            Dictionary<FDuree, double> dureesAgregees = InferenceAgregation<FHumidite, FTemperature, FDuree>(reglesHumiditeTemperature, ponderationsHumidite, ponderationsTemperature);

            foreach (KeyValuePair<FDuree, double> agregation in dureesAgregees)
            {
                Console.WriteLine("Valeur agrégée pour {0} : {1}", agregation.Key, agregation.Value);
            }

            // Fuzzification 2.
            Dictionary<FNappe, double> ponderationsNappe = GetPonderations<FNappe>(ensembleNappe, 1.4);



            // Fin.
            Console.Read();
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
                    || (previous != null && value > previous.Right)
                    || (next != null && value < next.Left))
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
    }
}