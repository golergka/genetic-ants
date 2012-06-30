using System;

namespace Genetic.Programming.Arithmetic
{
	public class Addition : Int2Function
	{
		
		public Addition() { }
		
		public override int Compute (ComputationContext<int> task)
		{
			return firstOperand.Compute (task) + secondOperand.Compute (task);
		}
		
		protected override string operand ()
		{
			return "+";
		}
	}
}

