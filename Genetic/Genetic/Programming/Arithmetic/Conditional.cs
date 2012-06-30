using System;
using System.Reflection;

namespace Genetic.Programming.Arithmetic
{
	public class Conditional : Function<int>
	{
		
		private ExpressionTree<int> condition;
		private ExpressionTree<int> trueValue;
		private ExpressionTree<int> falseValue;
		
		public Conditional ()
		{
		}
		
		public override int childrenShortage ()
		{
			
			if (condition == null)
				return 3;
			else if (trueValue == null)
				return 2;
			else if (falseValue == null)
				return 1;
			else
				return 0;
			
		}
		
		public override void addChild (ExpressionTree<int> child)
		{
			if (condition == null)
				condition = child;
			else if (trueValue == null)
				trueValue = child;
			else if (falseValue == null)
				falseValue = child;
			else
				throw new OutOfMemoryException ("Already full!");
		}
		
		public override int arity ()
		{
			return 3;
		}
		
		public override object Clone ()
		{
			
			Type type = this.GetType ();
			ConstructorInfo constructor = type.GetConstructors () [0];
			Conditional result = (Conditional)constructor.Invoke (null);
			
			if (condition != null)
				result.condition = condition;
			
			if (trueValue != null)
				result.trueValue = trueValue;
			
			if (falseValue != null)
				result.falseValue = falseValue;
			
			return result;
			
		}
		
		public override string ToString ()
		{
			return ("(" + condition.ToString() +
			        "?" + trueValue.ToString() +
			        ":" + falseValue.ToString() +
			        ")" );
		}
		
		public override int Complexity ()
		{
			return (1 + condition.Complexity () + trueValue.Complexity () + falseValue.Complexity ());
		}
		
		public override int Compute (ComputationContext<int> task)
		{
			if (condition.Compute (task) == 0)
				return trueValue.Compute (task);
			else
				return falseValue.Compute (task);
				
		}
		
	}
}

