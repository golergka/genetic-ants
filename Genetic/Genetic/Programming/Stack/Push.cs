using System;

namespace Genetic.Programming.Stack
{
	
	public class Push<T> : Function <T>
	{
		
		ExpressionTree<T> pushing;
		
		public Push ()
		{
		}
		
		public override int childrenShortage ()
		{
			
			if (pushing == null)
				return 1;
			else
				return 0;
			
		}
		
		public override void addChild (ExpressionTree<T> child)
		{
			
			if (pushing == null)
				pushing = child;
			else
				throw new OutOfMemoryException ("No place for this child!");
			
		}
		
		public override object Clone ()
		{
			
			Push<T> result = new Push<T> ();
			result.pushing = pushing;
			return result;
			
		}
		
		public override int arity ()
		{
			return 1;
		}
		
		public override int Complexity ()
		{
			return 1 + pushing.Complexity ();
		}
		
		public override T Compute (ComputationContext<T> task)
		{
			
			T result = pushing.Compute (task);
			task.stack.Push (result);
			return result;
			
		}
		
		public override string ToString ()
		{
			return "P(" + pushing.ToString () + ")";
		}
	}
}

