using System;
using System.Reflection;

namespace Genetic.Programming
{
	
	public abstract class ExpressionTree<T> : ICloneable, IComputable<T>, IMeasurable
	{
		
		public ExpressionTree () { }
		
		public abstract int childrenShortage();
		
		public abstract void addChild(ExpressionTree<T> child);
		
		public virtual object Clone ()
		{
			
			Type type = this.GetType ();
			ConstructorInfo constructor = type.GetConstructors () [0];
			ExpressionTree<T> result = (ExpressionTree<T>)constructor.Invoke (null);
			return result;
			
		}
		
		public abstract T Compute(ComputationContext<T> task);
		
		public abstract int arity();
		
		public abstract int Complexity();
		
	}
}

