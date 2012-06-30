using System;

namespace Genetic.Programming.Stack
{
	public class Pop<T> : Terminal<T>
	{
		public Pop ()
		{
		}
		
		public override T Compute (ComputationContext<T> task)
		{
			
			if (task.stack.Count > 0)
				return task.stack.Pop ();
			else
				return task.input;
			
		}
		
		public override string ToString ()
		{
			return "p";
		}
	}
}

