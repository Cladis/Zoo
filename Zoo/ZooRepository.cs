﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Zoo
{
    /// <summary>
    /// Wraps around Animals dictionary in try to implement the repository pattern
    /// </summary>
    public class ZooRepository
    {
        private Dictionary<string, Animal> Animals { get; } = new Dictionary<string, Animal>();

        public Animal this[string name]
        {
            get => Animals[name];
            set => Animals[name] = value;
        }

        public int Count => Animals.Count;

        public void Add(string name, Animal beast)
        {
            Animals.Add(name, beast);
        }

        public bool Contains(string name)
        {
            return Animals.ContainsKey(name);
        }

        public bool Contains(Animal beast)
        {
            return Animals.ContainsValue(beast);
        }

        public void Delete(string name)
        {
            Animals.Remove(name);
        }

        public void Clear()
        {
            Animals.Clear(); ;
        }


        /// <summary>
        /// Gets all the animals in the Zoo, sorted by species they belong to
        /// </summary>
        /// <returns></returns>
        public List<Animal> GetAllAnimals()
        {
            return (from beast in Animals.Values orderby beast.Species ascending select beast).ToList();
        }


        public List<Animal> GetAliveAnimals()
        {
            return (from beast in Animals.Values where beast.State != AnimalState.Dead orderby beast.Species ascending select beast).ToList();
        }

        public List<Animal> GetDeadAnimals()
        {
            return (from beast in Animals.Values where beast.State == AnimalState.Dead select beast).ToList();
        }

        /// <summary>
        /// TODO add species validation
        /// </summary>
        /// <param name="species"></param>
        /// <returns></returns>
        public List<Animal> GetAnimalsBySpecies(string species)
        {
            return (from beast in Animals.Values
                    where string.Equals(beast.Species.ToLower(), species, StringComparison.CurrentCultureIgnoreCase)
                    select beast).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public List<Animal> GetAnimalsByState(string state)
        {
            if (!Enum.TryParse(state, true, out AnimalState parsedState))
            {
                Console.WriteLine($"You've supplied a wrong animal state of {state}");
            }
            return (
                from beast
                in Animals.Values
                where beast.State == parsedState
                select beast).ToList();
        }

        public int CountAnimalsByState(string state)
        {
            return GetAnimalsByState(state).Count();
        }

        public List<Animal> GetAnimalsByStateBySpecies(string species, string state)
        {
            if (!Enum.TryParse(state, true, out AnimalState parsedState))
            {
                Console.WriteLine($"You've supplied a wrong animal state of {state}");
            }
            return (
                from beast
                in Animals.Values
                where string.Equals(beast.Species, species, StringComparison.CurrentCultureIgnoreCase) &&
                      beast.State == parsedState
                select beast
                ).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Animal> GetMostHealthy()
        {
            return (from beast in Animals.Values
                    group beast by beast.Species
                into g
                    select g.OrderByDescending(s => s.Health).First()).ToList();
        }

        public Dictionary<string, int> CountMostHealthy()
        {
            return (from beast in Animals.Values
                    group beast by beast.Species into g
                    select new { Species = g.First().Species, Count = g.Count() }).ToDictionary(s => s.Species, s => s.Count);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="species"></param>
        /// <param name="threshold">Animals with Health of more than strictly this threshold will be returned</param>
        /// <returns></returns>
        public List<Animal> GetHealthierThan(string[] species, byte threshold)
        {
            return (
                from beast
                in Animals.Values
                where species.Contains(beast.Species.ToLower())
                && beast.Health > threshold
                select beast
            ).ToList();
        }

        public List<Animal> MinMaxHealth()
        {
            return (from beast in Animals.Values
                    group beast by 1
                    into g
                    select new List<Animal>()
                    {
                            (from b in g where b.Health == g.Max(i => i.Health) select b).Take(1).First(),
                            (from b in g where b.Health == g.Min(i => i.Health) select b).Take(1).First()
                    }).First();
        }

        public double GetAverageHealth()
        {
            return GetAllAnimals().Average(i => (double)i.Health);
        }


        public void PrintListOfAnimals(List<Animal> animals, bool namesOnly = false)
        {
            foreach (var animal in animals)
            {
                Console.WriteLine(namesOnly ? animal.Name : animal.ToString());
            }
        }
    }
}