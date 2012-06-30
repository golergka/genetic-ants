using System;
using System.Reflection;

namespace Genetic.Programming.Genome
{
	public class Expression<T>
	{
		
		private Type expressionTreeType;
		
		private int _arity;
		
		public virtual int arity {
			
			get { return _arity; }
			
		}
		
		public Expression () { }
		
		public Expression (Type expressionTreeType)
		{
			
			this.expressionTreeType = expressionTreeType;
			_arity = readExpression ().arity ();
			
		}
		
		public Expression (ExpressionTree<T> expressionTree)
		{
			
			this.expressionTreeType = expressionTree.GetType ();
			_arity = expressionTree.arity ();
			
		}
		
		public virtual ExpressionTree<T> readExpression ()
		{
			
			ConstructorInfo constructor = expressionTreeType.GetConstructors () [0];
			return (ExpressionTree<T>) constructor.Invoke (null);
			
		}
		
	}
}

