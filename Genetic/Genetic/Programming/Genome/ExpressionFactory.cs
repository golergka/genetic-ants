using System;
using System.Collections.Generic;

namespace Genetic.Programming.Genome
{
	public class ExpressionFactory <T>
	{
		
		private List<Expression<T>> functions;
		private List<Expression<T>> terminals;
		
		static Random rnd = new Random();
		
		public ExpressionFactory (List<Expression<T>> functions,
		                          List<Expression<T>> terminals)
		{
			
			this.functions = functions;
			this.terminals = terminals;
			
		}
		
		private void ReadExpressions (List<Expression<T>> expressions)
		{
			
			functions = new List<Expression<T>> ();
			terminals = new List<Expression<T>> ();
			
			foreach (Expression<T> expression in expressions)
				if (expression.arity == 0)
					this.terminals.Add (expression);
				else
					this.functions.Add (expression);
			
		}
		
		public ExpressionFactory (List<Expression<T>> expressions)
		{
			
			ReadExpressions (expressions);
			
		}
		
		public ExpressionFactory (List<ExpressionTree<T>> expressionTrees)
		{
			
			List<Expression<T>> expressions = new List<Expression<T>> ();
			
			foreach (ExpressionTree<T> expressionTree in expressionTrees)
				expressions.Add (new Expression<T> (expressionTree));
			
			ReadExpressions (expressions);
			
		}
		
		public Expression<T> randomFunction ()
		{
			
			return functions[rnd.Next(functions.Count)];
			
		}
		
		public Expression<T> randomTerminal ()
		{
			
			return terminals[rnd.Next(terminals.Count)];
			
		}
		
		public Expression<T> randomExpression ()
		{
			
			int random = rnd.Next (functions.Count + terminals.Count);
			
			if (random < functions.Count)
				return functions[random];
			else return terminals[random - functions.Count];
			
		}
		
		public int maxArity ()
		{
			
			int result = 0;
			
			foreach (Expression<T> f in functions)
				if (f.arity > result)
					result = f.arity;
			
			return result;
		}
	}
}

