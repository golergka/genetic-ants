using System;

namespace Genetic.Programming
{
	public abstract class Terminal<T> : ExpressionTree <T>
	{
		
		public override int childrenShortage ()
		{
			return 0;
		}
		
		public override void addChild (ExpressionTree<T> child)
		{
			
			throw new Exception("Can't add children to terminal!");
			
		}
		
		public override int arity ()
		{
			return 0;
		}
		
		public override int Complexity ()
		{
			return 1;
		}
		
	}
}

