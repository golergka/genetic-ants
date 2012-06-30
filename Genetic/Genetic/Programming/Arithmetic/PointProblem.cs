using System;
using System.Collections.Generic;

namespace Genetic.Programming.Arithmetic
{
	
	public class PointProblem<T> : Problem <T> where T : Individual, IComputable<int>
	{
		
		public int result;
		public ComputationContext<int> task;
		
		public PointProblem (int x, int y)
		{
			
			this.task = new ComputationContext<int> ();
			task.input = x;
			result = y;
			
		}
		
		public override int test (T subject)
		{
			
			return Math.Max (Math.Min (0 - Math.Abs (subject.Compute (task) - result), Int32.MaxValue), Int32.MinValue);
			
		}
		
	}
}

