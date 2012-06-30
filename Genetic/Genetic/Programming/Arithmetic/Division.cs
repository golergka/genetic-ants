using System;

namespace Genetic.Programming.Arithmetic
{
	public class Division : Int2Function
	{
		public override int Compute (ComputationContext<int> task)
		{
			
			int possiblyZero = secondOperand.Compute (task);
			
			if (possiblyZero == 0)
				return 0;
			else 
				return firstOperand.Compute (task) / possiblyZero;
		}
		
		protected override string operand ()
		{
			return "/";
		}
	}
}

