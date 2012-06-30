using System;
using Genetic.Programming;

namespace Ants
{
	public class RandomAntBrain : IComputable<MoveDirection>
	{
		
		static Random rnd = new Random();
		
		public virtual MoveDirection Compute (ComputationContext<MoveDirection> task)
		{
			int random = rnd.Next (4);
			
			switch (random) {
				
			case 0:
				return MoveDirection.Down;
				
			case 1:
				return MoveDirection.Left;
				
			case 2:
				return MoveDirection.Right;
				
			case 3:
				return MoveDirection.Up;
				
			default:
				return MoveDirection.Up;
				
			}
		}
		
	}
}

