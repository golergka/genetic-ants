using System;
using System.Reflection;

namespace Genetic.Programming.Arithmetic
{
	public abstract class Int2Function : Function<int>
	{
		
		protected ExpressionTree<int> firstOperand;
		protected ExpressionTree<int> secondOperand;
		
		public Int2Function ()
		{
		}
		
		public override int childrenShortage ()
		{
			if (firstOperand == null)
				return 2;
			else if (secondOperand == null)
				return 1;
			else
				return 0;
		}
		
		public override void addChild (ExpressionTree<int> child)
		{
			if (firstOperand == null)
				firstOperand = child;
			else if (secondOperand == null)
				secondOperand = child;
			else
				throw new OutOfMemoryException ("Already full!");
		}
		
		public override int arity ()
		{
			return 2;
		}
		
		public override object Clone ()
		{
			
			Type type = this.GetType ();
			ConstructorInfo constructor = type.GetConstructors () [0];
			Int2Function result = (Int2Function)constructor.Invoke (null);
			
			if (firstOperand != null)
				result.firstOperand = (ExpressionTree<int>)firstOperand.Clone ();
			
			if (secondOperand != null)
				result.secondOperand = (ExpressionTree<int>)secondOperand.Clone ();
			
			return result;
		}
		
		protected virtual string operand ()
		{
		
			return "?";
			
		}
		
		public override string ToString ()
		{
			return "(" + firstOperand.ToString () + operand () + secondOperand.ToString() + ")";
		}
		
		public override int Complexity ()
		{
			return 1 + firstOperand.Complexity () + secondOperand.Complexity ();
		}
		
	}
}

