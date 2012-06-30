using System;
using System.Collections.Generic;

namespace Genetic.Programming.Genome
{
	public class Chromosome<T> : Individual, IComputable <T>, ICrossoverable, IMeasurable
	{
		
		private List<Gene<T>> genes = new List<Gene<T>>();
		
#pragma warning disable 649
		public static ExpressionFactory<T> factory;
#pragma warning restore 649
		static Random rnd = new Random();
		static int length = 0;
		
		const int CROSSOVER_POINTS = 2;
		const int CROSSOVER_POINTS_MIN_DISTANCE = 2;
		const int HEAD_LENGTH = 20;
		const int GENE_COUNT = 1;
		
		public Chromosome ()
		{
			
			for(int i=0; i<GENE_COUNT; i++)
				genes.Add(new Gene<T>(factory, HEAD_LENGTH));
			
		}
		
		private void ReadExpressions (List<Expression<T>> expressions)
		{
			
			genes.Clear ();
			
			for (int i=0; i<GENE_COUNT; i++)
				genes.Add (new Gene<T> (factory, HEAD_LENGTH,
				                        expressions.GetRange ( i * GeneLength(), GeneLength() )));
			
		}
		
		public Chromosome (List<Expression<T>> expressions)
		{
			
			ReadExpressions (expressions);
			
		}
		
		static private int _geneLength = 0;
		
		static private int GeneLength ()
		{
			if (_geneLength == 0)
				_geneLength = HEAD_LENGTH + Gene<T>.calculateTailLength (HEAD_LENGTH, (int)factory.maxArity ());
			
			return _geneLength;
			
		}
		
		static private int Length ()
		{
			
			if (length == 0)
				for (int i=0; i<GENE_COUNT; i++)
					length += HEAD_LENGTH + Gene<T>.calculateTailLength (HEAD_LENGTH, (int) factory.maxArity ());
			
			return length;
			
		}
		
		static private List<int> randomCrossoverPoints ()
		{
			List<int> result = new List<int> ();
			
			int freedom = Length() - CROSSOVER_POINTS * CROSSOVER_POINTS_MIN_DISTANCE;
			
			result.Add (0);
			
			for (int i=1; i<CROSSOVER_POINTS; i++) {
				
				int newPoint = result [i - 1] + CROSSOVER_POINTS_MIN_DISTANCE;
				newPoint += (int)(rnd.Next ((int)freedom) / CROSSOVER_POINTS);
				
				result.Add (newPoint);
					
			}
			
			result.Add (Length ());
			
			return result;
		}
		
		public List<Expression<T>> Expressions ()
		{
			
			List <Expression<T>> result = new List<Expression<T>> ();
			
			foreach (Gene<T> gene in genes)
				result.AddRange (gene.expressions ());
			
			return result;
			
		}
		
		public object Crossover (object mother, object father)
		{
			
			List<Expression<T>> motherExpressions = ((Chromosome<T>)mother).Expressions ();
			List<Expression<T>> fatherExpressions = ((Chromosome<T>)father).Expressions ();
			
			List<Expression<T>> childExpressions = new List<Expression<T>> ();
			
			List<int> points = randomCrossoverPoints ();
			
			bool motherTurn = true;
			
			for (int i=0; i<points.Count-1; i++) {
			
				List<Expression<T>> newExpressions = (motherTurn ? motherExpressions : fatherExpressions).
					GetRange (points [i], points [i + 1] - points[i]);
				childExpressions.AddRange (newExpressions);
				
				motherTurn = ! motherTurn;
			}
			
			ReadExpressions (childExpressions);
			
			return this;
			
		}
		
		public Chromosome (Chromosome<T> mother, Chromosome<T> father)
		{
			
			this.Crossover (mother, father);
			
		}
		
		public T Compute (ComputationContext<T> task)
		{
			
			task.computedResults.Clear ();
			
			foreach (Gene<T> gene in genes)
				task.computedResults.Add (gene.Compute (task));
			
			return task.computedResults [task.computedResults.Count - 1];
			
		}
		
		override public void Mutate ()
		{
			
			genes [rnd.Next (genes.Count)].Mutate ();
			
		}
		
		public override string ToString ()
		{
			string result = genes.Count + "<";
			
			foreach (Gene<T> gene in genes)
				result += gene.ToString () + ";";
			
			return result + ">";
		}
		
		public int Complexity ()
		{
			int result = 0;
			
			foreach (Gene<T> gene in genes)
				result += gene.Complexity ();
			
			return result;
		}
		
	}
}

