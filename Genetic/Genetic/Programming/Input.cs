using System;

namespace Genetic.Programming
{
	public class Input<T> : Terminal<T>
	{
		public override T Compute (ComputationContext<T> task)
		{
			return task.input;
		}
		
		public override string ToString ()
		{
			return string.Format ("x");
		}
	}
}

