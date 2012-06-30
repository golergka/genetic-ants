using System;
using System.Collections.Generic;

namespace Genetic.Programming
{
	
	public enum ExpressionTreeCreateMethod
	{
		
		grow,
		full
		
	}
	
	public class ExpressionTreeFactory <T>
	{
		
		List<Terminal<T>> terminals;
		List<Function<T>> functions;
		
		static Random rnd = new Random();
		
		public ExpressionTreeFactory (List<Terminal<T>> terminals, List<Function<T>> functions)
		{
			
			this.terminals = terminals;
			this.functions = functions;
			
		}
		
		private Terminal<T> newTerminal ()
		{
			
			return (Terminal<T>) terminals [rnd.Next (terminals.Count)].Clone();
			
		}
		
		private Function<T> newFunction ()
		{
			
			return (Function<T>) functions [rnd.Next (functions.Count)].Clone();
			
		}
		
		private ExpressionTree<T> newExpressionTree ()
		{
			
			int random = rnd.Next (terminals.Count + functions.Count);
			
			if (random >= terminals.Count)
				return (ExpressionTree<T>)functions [random - terminals.Count].Clone ();
			else
				return (ExpressionTree<T>) terminals[random].Clone ();
			
		}
		
		public ExpressionTree<T> generateTree (uint maxDepth,
		                                   ExpressionTreeCreateMethod createMethod)
		{
			
			if (maxDepth == 0)
				return newTerminal ();
			
			ExpressionTree<T> result;
			
			if (createMethod == ExpressionTreeCreateMethod.grow)
				result = newExpressionTree ();
			else
				result = newFunction ();
			
			for (uint i=0; i<result.childrenShortage(); i++)
				result.addChild (generateTree (maxDepth - 1, createMethod));
			
			return result;
			
		}
		
	}
}

