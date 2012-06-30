using System;

namespace Genetic.Programming.Arithmetic
{
	public class Maximum : Int2Function
	{
		
		public Maximum () { }
		
		public override int Compute (ComputationContext<int> task)
		{
			
			int first = firstOperand.Compute (task);
			int second = secondOperand.Compute (task);
			
			return Math.Max (first, second);
				
		}
		
		protected override string operand ()
		{
			return "M";
		}
	}
}

