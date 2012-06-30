using System;
using System.Collections.Generic;

namespace Genetic
{
	
	public enum SelectionMethod
	{
		Elitist,
		Tournament
	}
	
	public class Population<T> where T : Individual, new()
	{
		
		const bool INCLUDE_PARENTS = false;
		
		static Random rnd = new Random ();
		
		internal List<T> citizens = new List<T>();
		
		// fill with random individuals
		public Population(int populationSize) {
			
			for(int i=0; i<populationSize;i++)
				citizens.Add(new T());
			
		}
		
		// fill with children of the list of parents
		public Population (int populationSize,
		                   List<T> parents,
		                   int randomCitizens = 0,
		                   T best = null,
		                   int bestClones = 0)
		{
			
			// add random citizens
			for (int i=0; i<randomCitizens; i++)
				citizens.Add (new T ());
			
			// add clones of best
			for (int i=0; i<bestClones; i++)
				citizens.Add (best);
			
			// add parents
			if (INCLUDE_PARENTS)
#pragma warning disable 162
				citizens.AddRange (parents);
#pragma warning restore 162
			
			// add children
			for (int i = 0;
#pragma warning disable 429
			     i < populationSize - citizens.Count;
#pragma warning restore 429
			     i++) {
				
				int m = rnd.Next (parents.Count);
				int f = rnd.Next (parents.Count);
				
				T child = new T();
				
				if (child is ICrossoverable )
					child = (T) ((ICrossoverable) child).Crossover(parents[m], parents[f]);
				else
					child = (T)Activator.CreateInstance (typeof(T), new object[] { parents [m], parents [f] });
				
				citizens.Add (child);
				
			}
			
			
		}
		
		private T randomCitizen ()
		{
			return citizens [rnd.Next (citizens.Count)];
		}
	
		public void Mutate (int mutationSize)
		{
			
			for (int i=0; i<mutationSize; i++)
				citizens [rnd.Next (citizens.Count)].Mutate ();
			
		}
		
		public List<T> Select (int selectionSize,
		                       ProblemSet<T> tester,
		                       SelectionMethod method = SelectionMethod.Elitist,
		                       int tournamentSize = 2)
		{
			
			List<T> result = new List<T>();
			
			switch (method) {
				
			case SelectionMethod.Elitist:
			
				citizens.Sort (tester);
				result = citizens.GetRange (citizens.Count - selectionSize, selectionSize);
				
				break;
				
			case SelectionMethod.Tournament:
				
				for (int i=0; i<selectionSize; i++) {
					
					T winner = citizens [0];
					
					for (int j=1; j<tournamentSize; j++)
						if (tester.test (citizens [j]) > tester.test (winner))
							winner = citizens [j];
					
					result.Add (winner);
					
				}
					
				break;
				
			default:
				
				throw new NotSupportedException ("Not supported selection method!");
				
			}
			
			return result;
			
		}
		
		public T getBest (ProblemSet<T> tester)
		{
			T result = citizens [0];
			
			foreach (T citizen in citizens)
				if (tester.test (citizen) > tester.test (result))
					result = citizen;
			
			return result;
		}

		public override string ToString ()
		{
			string result = "[Population:";
			foreach (T citizen in citizens)
				result += " " + citizen.ToString ();
			result += " ]";
			return result;
		}
	}
}

