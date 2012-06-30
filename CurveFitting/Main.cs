using System;
using System.Collections.Generic;
using Genetic;
using Genetic.Programming;
using Genetic.Programming.Arithmetic;
using Genetic.Programming.Genome;
using Genetic.Programming.Stack;

namespace CurveFitting
{
	class MainClass
	{
		
		static Random rnd = new Random();
		
		const int MIN_INT = -10000;
		const int MAX_INT = 10000;
		const int POINT_AMOUNT = 20;
		
		public static bool readYNstring(string YNstring) {
			
			if (YNstring == "")
				return true;
			
			if (YNstring.ToUpper().StartsWith("Y"))
			    return true;
			    
			return false;
			
		}
		
		public static void Main (string[] args)
		{
			Console.WriteLine ("Generating random points:");
			
			List<Problem<Chromosome<int>>> problems = new List<Problem<Chromosome<int>>> ();
			
			for (int i=0; i<POINT_AMOUNT; i++) {
				
				int x = rnd.Next (MIN_INT, MAX_INT);
				
				//
				// TARGET FORMULA
				//
				
				int y = x % 5;
				
				Console.WriteLine ("(" + x.ToString () + "," + y.ToString () + ")");
				
				problems.Add (new PointProblem<Chromosome<int>> (x, y));
				
			}
			
			bool useCache = true;
			int generations = 100;
			int populationSize = 1000;
			int selectionSize = 100;
			int mutationSize = 500;
			
			bool run = true;
			
			while (run) {
				
				Console.WriteLine ("Use caching in problem set ( " + useCache + " ) ?");
				string newUseCache = Console.ReadLine ();
				if (newUseCache != "")
					useCache = readYNstring (newUseCache);
				
				//  `System.Collections.Generic.List<Genetic.Programming.Arithmetic.PointProblem<Genetic.Programming.Genome.Chromosome<int>>>'
				//  `System.Collections.Generic.List<Genetic.Problem<Genetic.Programming.Genome.Chromosome<int>>>'
				
				ProblemSet<Chromosome<int>> tester = new ProblemSet<Chromosome<int>> (problems, useCache);
				
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
				
				List<Expression<int>> operations = new List<Expression<int>> ();
				
				operations.Add (new Expression<int> (typeof(Addition)));
				operations.Add (new Expression<int> (typeof(Subtraction)));
				operations.Add (new Expression<int> (typeof(Multiplication)));
				operations.Add (new Expression<int> (typeof(Division)));
				operations.Add (new Expression<int> (typeof(Conditional)));
				operations.Add (new Expression<int> (typeof(Maximum)));
				operations.Add (new Expression<int> (typeof(Minimum)));
				operations.Add (new Expression<int> (typeof(Modulo)));
				operations.Add (new Expression<int> (typeof(Pop<int>)));
				operations.Add (new Expression<int> (typeof(Push<int>)));
				operations.Add (new Expression<int> (typeof(Input<int>)));
				
				operations.Add (new ConstantExpression<int> (1));
				operations.Add (new ConstantExpression<int> (2));
				operations.Add (new ConstantExpression<int> (0));
				
				ExpressionFactory<int> factory = new ExpressionFactory<int> (operations);
				
				Chromosome<int>.factory = factory;
				
				Evolution<Chromosome<int>> evolution = new Evolution<Chromosome<int>> (generations, 
				                                                                        populationSize, 
				                                                                        selectionSize, 
				                                                                        mutationSize, 
				                                                                        tester, false);
				
				Chromosome<int> winner = evolution.win ();
				
				Console.WriteLine ("Evolution result: " + winner.ToString ());
				
				tester.test (winner, true);
				
				foreach (PointProblem<Chromosome<int>> problem in problems) {
					
					int x = problem.task.input;
					int y = winner.Compute (problem.task);
					int expectedY = problem.result;
					
					Console.WriteLine ((y == expectedY ? "+" : "-") + " f(" + x + ") = " + y + " : " + expectedY);
						
				}
				
				if (winner is IMeasurable)
					Console.WriteLine ("Complexity: " + ((IMeasurable)winner).Complexity ());
				
				Console.WriteLine ("Fitness: " + tester.test (winner));
				
				Console.WriteLine ("Do you want to run again (Y/N)?");
				string runAgain = Console.ReadLine ();
				
				run = readYNstring (runAgain);
				
			}
			
		}
	}
}
