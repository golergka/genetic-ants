using System;

namespace Genetic.Programming.Genome
{
	public class ConstantExpression<T> : Expression<T>
	{
		
		private T constantValue;
		
		public override int arity {
			get {
				return 0;
			}
		}
		
		public ConstantExpression (T constantValue)
		{
			
			this.constantValue = constantValue;
			
		}
		
		public override ExpressionTree<T> readExpression ()
		{
			
			Constant<T> result = new Constant<T> ();
			result.constantValue = constantValue;
			return result;
			
		}
	}
}

