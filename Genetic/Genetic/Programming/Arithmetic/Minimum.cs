using System;

namespace Genetic.Programming.Arithmetic
{
	public class Minimum : Int2Function
	{
		
		public Minimum () { }
		
		public override int Compute (ComputationContext<int> task)
		{
			
			int first = firstOperand.Compute (task);
			int second = secondOperand.Compute (task);
			
			return Math.Min (first, second);
				
		}
		
		protected override string operand ()
		{
			return "m";
		}
	}
}

