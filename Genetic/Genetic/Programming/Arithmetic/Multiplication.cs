using System;

namespace Genetic.Programming.Arithmetic
{
	public class Multiplication : Int2Function
	{
		public override int Compute (ComputationContext<int> task)
		{
			return firstOperand.Compute (task) * secondOperand.Compute (task);
		}
		
		protected override string operand ()
		{
			return "*";
		}
	}
}

