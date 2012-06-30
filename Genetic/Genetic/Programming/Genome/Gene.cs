using System;
using System.Collections;
using System.Collections.Generic;
using Genetic.Programming;

namespace Genetic.Programming.Genome
{
	public class Gene<T> : IComputable<T>, IMutatable, IMeasurable
	{
		
		private List<Expression<T>> head = new List<Expression<T>>();
		private List<Expression<T>> tail = new List<Expression<T>>();
		private int index = 0;
		
		private ExpressionTree<T> tree;
		private ExpressionFactory<T> factory;
		
		static Random rnd = new Random();
		
		public static int calculateTailLength (int headLength, int maxArity)
		{
			return headLength * (maxArity - 1) + 1;
		}
		
		public Gene (ExpressionFactory<T> factory, int headLength)
		{
			
			this.factory = factory;
			
			for (int i = 0; i < headLength; i++)
				head.Add (factory.randomExpression ());
			
			int tailLength = calculateTailLength (headLength, factory.maxArity());
			
			for (int i = 0; i < tailLength; i++)
				tail.Add (factory.randomTerminal ());
			
		}
		
		public Gene (ExpressionFactory<T> factory, int headLength, List<Expression<T>> expressions)
		{
			
			this.factory = factory;
			
			head = expressions.GetRange (0, (int) headLength);
			tail = expressions.GetRange ( (int) headLength,
			                             (int) calculateTailLength (headLength, factory.maxArity()));
			
		}
		
		public T Compute (ComputationContext<T> task)
		{
			
			return Tree().Compute (task);
			
		}
		
		public ExpressionTree<T> Tree ()
		{
			
			if (tree == null)
				GenerateTree ();
			
			return tree;
			
		}
		
		private void GenerateTree ()
		{
			
			ExpressionTree<T> result = Read ().readExpression ();
			Queue<ExpressionTree<T>> childlessTrees = new Queue<ExpressionTree<T>> ();
			childlessTrees.Enqueue (result);
			
			while (childlessTrees.Count > 0) {
				
				ExpressionTree<T> childless = childlessTrees.Dequeue ();
				
				while (childless.childrenShortage() > 0) {
					
					ExpressionTree<T> newChild = Read ().readExpression ();
					childless.addChild (newChild);
					childlessTrees.Enqueue (newChild);
					
				}
				
			}
			
			this.tree = result;
			
		}
		
		private Expression<T> Read ()
		{
			
			if (index < head.Count)
				return head [index++];
			else if (index < head.Count + tail.Count)
				return tail [index++ - head.Count];
			else
				return null;
			
		}
		
		public void Mutate ()
		{
			
			int mutatationType = rnd.Next (2);
			
			switch (mutatationType) {
				
			case 0:
			
				int random = rnd.Next (head.Count + tail.Count);
			
				if (random < head.Count) {
				
					Expression<T> newExpression = factory.randomExpression ();
					head [random] = newExpression;
				
				} else {
				
					Expression<T> newTerminal = factory.randomTerminal ();
					tail [random - head.Count] = newTerminal;
				
				}
				
				break;
				
			case 1:
				
				int randomIndex = rnd.Next (head.Count);
				
				for (int i=randomIndex; i<head.Count; i++)
					if (head [i].arity > 0) {
					
						int randomLength = rnd.Next (head.Count - i);
						List<Expression<T>> moving = head.GetRange (i, randomLength);
						head.RemoveRange (i, randomLength);
						head.InsertRange (0, moving);
					
					}
				
				break;
				
			};
			
		}
		
		public List<Expression<T>> expressions ()
		{
			List<Expression<T>> result = new List<Expression<T>> ();
			
			result.AddRange (head);
			result.AddRange (tail);
			
			return result;
		}
		
		public int Length ()
		{
			return head.Count + tail.Count;
		}
		
		public override string ToString ()
		{
			return Tree ().ToString ();
		}
		
		public int Complexity ()
		{
			
			return this.Tree ().Complexity ();
			
		}
	}
}

