using System;
using System.Collections.Generic;
using Genetic.Programming;

namespace Ants
{
	public class SimpleAntBrain : RandomAntBrain
	{
		public SimpleAntBrain ()
		{
		}
		
		public override MoveDirection Compute (ComputationContext<MoveDirection> task)
		{
			if (task is AntNavigationContext)
				return Compute ((AntNavigationContext)task);
			else
				throw new ArgumentException ("ComputationContext should be AntNavigationContext!");
		}
		
		const double RANDOM_MOVE_PROBABILITY = 0.1;
		
		static Random rnd = new Random ();
		
		public virtual MoveDirection Compute (AntNavigationContext task)
		{

			// we need random moves sometime to avoid situations of following other ant step into step
			if (rnd.NextDouble () < RANDOM_MOVE_PROBABILITY)
				return base.Compute (task);
			
			// if we have grass here, we'll stay here
			if (task.CountGrassInDirection (MoveDirection.Stay) > 0)
				return MoveDirection.Stay;
			
			// or we will look arund farther and farther
			
			bool hitBorders;
			
			Dictionary<MoveDirection,bool> hitBorder = new Dictionary<MoveDirection, bool> ();
			
			foreach (MoveDirection d in Extensions.aroundDirections)
				hitBorder [d] = false;
			
			int distance = 1;
			
			do {
				
				hitBorders = true;
				
				// initial value, means indecision
				MoveDirection result = MoveDirection.Stay;
				
				int maxGrass = 0;
				
				foreach (MoveDirection d in Extensions.aroundDirections) {
					
					if (hitBorder [d])
						continue;
					
					if (task.lookDistance [d] < distance) {
						hitBorder [d] = true;
						continue;
					}
					
					hitBorders = false;
					
					if (task.CountGrassInDirection (d, distance) > maxGrass)
						result = d;
					
				}
				
				if (result != MoveDirection.Stay)
					return result;
				
				distance++;
				
			} while (!hitBorders);
				
			// if all else fails, CHILL
			return MoveDirection.Stay;
			
		}
	}
}

