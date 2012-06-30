using System;
using System.Collections.Generic;

namespace Genetic.Programming
{
	
	public class ComputationContext <T>
	{
		
		public T input;
		public List<T> computedResults = new List<T>();
		public Stack<T> stack = new Stack<T> ();
		
	}
	
	public interface IComputable <T>
	{
		
		T Compute(ComputationContext<T> task);
		
	}
}

