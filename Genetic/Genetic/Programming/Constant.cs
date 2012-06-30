using System;
using System.Collections.Generic;

namespace Genetic.Programming
{
	public class Constant<T> : Terminal<T>
	{
		
		public T constantValue;
		
		public override T Compute (ComputationContext<T> task)
		{
			return constantValue;
		}
		
		public override string ToString ()
		{
			return string.Format (constantValue.ToString());
		}
		
	}
}

