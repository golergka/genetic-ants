using System;
using System.Collections.Generic;

namespace Genetic
{
	
	public class Evolution<T> where T : Individual, new() {
		
		int generations;
		int populationSize;
		int selectionSize;
		int mutationSize;
		
		bool stopAtZero;
		uint clearCache;
		
		ProblemSet<T> tester;
		
		public Evolution (int generations,
		                  int populationSize,
		                  int selectionSize,
		                  int mutationSize,
		                  ProblemSet<T> tester,
		                  bool stopAtZero,
		                  uint clearCache = 4)
		{
			this.generations = generations;
			this.populationSize = populationSize;
			this.selectionSize = selectionSize;
			this.mutationSize = mutationSize;
			this.tester = tester;
			this.stopAtZero = stopAtZero;
			this.clearCache = clearCache;
		}
		
		public T win ()
		{
			
			Population<T> currentGeneration = new Population<T> (populationSize);
			
			for (int i=0; i<generations; i++) {
				List<T> parents = currentGeneration.Select (selectionSize, tester, SelectionMethod.Tournament, 10);
				
//				Console.WriteLine ("Generation: " + i + " selected have fitness [ " + 
//					tester.test (parents [0]) + " ; " + 
//					tester.test (parents [parents.Count - 1]) +
//					" ] with finest specimen " +
//					parents [parents.Count - 1].ToString ()
//				);
				
				long avgFitness = 0;
				
				foreach (T parent in parents)
					avgFitness += tester.test (parent);
				
				avgFitness /= parents.Count;
				
				T best = currentGeneration.getBest (tester);
				
				int bestFitness = tester.test (best);
				
				Console.WriteLine ("Generation: " + i +
				                   " average fitness: " + avgFitness +
				                   " best fitness: " + bestFitness +
				                   " best: " + best.ToString() );
				
				if (stopAtZero && tester.test (parents [parents.Count - 1]) >= 0)
					break;
				
				currentGeneration = new Population<T> (populationSize,
				                                       parents,
				                                       populationSize/10,
				                                       best,
				                                       populationSize/20);
				currentGeneration.Mutate (mutationSize);
				
				if (clearCache != 0 &&
					i % clearCache == 0)
					tester.clearCache ();
				
			}
			
			return currentGeneration.getBest(tester);
			
		}
		
		public override string ToString ()
		{
			return ("[Evolution gen: " + generations +
			        " popSize: " + populationSize +
			        " mutSize: " + mutationSize + " " + 
			        tester.ToString() + " ]");
		}
		
	}

}