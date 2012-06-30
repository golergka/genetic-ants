using System;
using Genetic;

namespace StringExample
{
	
	class MainClass
	{
		
		public static bool readYNstring(string YNstring) {
			
			if (YNstring == "")
				return true;
			
			if (YNstring.ToUpper().StartsWith("Y"))
			    return true;
			    
			return false;
			
		}
		
		public static void Main (string[] args)
		{
			
			bool run = true;
			
			string target = "Default target";
			bool useCache = false;
			int generations = 1000;
			int populationSize = 100;
			int selectionSize = 10;
			int mutationSize = 10;
			
			while (run) {
				
				Console.WriteLine ("Input target string for evolution ( " +
					target + " ):"
				);
				
				string newTarget = Console.ReadLine ();
				if (newTarget != "")
					target = newTarget;
				
				StringProblem<StringIndividual> problem = new StringProblem<StringIndividual> (target);
				
				Console.WriteLine("Use caching in problem set ( " + useCache + " ) ?");
				string newUseCache = Console.ReadLine ();
				if (newUseCache != "")
					useCache = readYNstring (newUseCache);
				
				ProblemSet<StringIndividual> tester = new ProblemSet<StringIndividual> (problem, useCache);
				
				Console.WriteLine ("Input generations amount ( " + generations + " ):");
				string newGenerations = Console.ReadLine ();
				if (newGenerations != "")
					generations = Convert.ToInt32 (newGenerations);
				
				Console.WriteLine ("Input population size ( " + populationSize + " ):");
				string newPopulationSize = Console.ReadLine ();
				if (newPopulationSize != "")
					populationSize = Convert.ToInt32 (newPopulationSize);
				
				Console.WriteLine ("Input selection size ( " + selectionSize + " ): ");
				string newSelectionSize = Console.ReadLine ();
				if (newSelectionSize != "")
					selectionSize = Convert.ToInt32 (newSelectionSize);
				
				Console.WriteLine ("Input mutation size ( " + mutationSize + " ): ");
				string newMutationSize = Console.ReadLine ();
				if (newMutationSize != "")
					mutationSize = Convert.ToInt32 (newMutationSize);
				
				Genetic.Evolution<StringIndividual> evolution = new Evolution<StringIndividual> (generations, 
				                                                                        populationSize, 
				                                                                        selectionSize, 
				                                                                        mutationSize, 
				                                                                        tester, true);
				
				Console.WriteLine ("Evolution result: " + evolution.win ().ToString ());
				
				Console.WriteLine ("Do you want to run again (Y/N)?");
				string runAgain = Console.ReadLine ();
				
				run = readYNstring(runAgain);
				
			}
		}
	}
}
