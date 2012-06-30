using System;
using System.Collections.Generic;

namespace Ants
{
	
	public interface ILookable {
		
		int Look (Point p);
		Dictionary<MoveDirection, int> LookDistance(Point pov);
		
	}
	
	public class LookSystem
	{
		
		private ILookable lookable;
		private Point pov;
		private Dictionary<MoveDirection, int> lookDistance;
		private Dictionary<MoveDirection, List<int>> lookResult = new Dictionary<MoveDirection, List<int>>();
		private Dictionary<MoveDirection, Point[][]> pointsInDirectionCache =
			new Dictionary<MoveDirection, Point[][]>();
		
		public LookSystem (ILookable lookable, Point pov)
		{
			
			this.lookable = lookable;
			this.pov = pov;
			lookDistance = lookable.LookDistance (pov);
			
			foreach (MoveDirection direction in Extensions.aroundDirections) {
				
				lookResult.Add (direction, new List<int> ());
				pointsInDirectionCache.Add (direction, null);
				
			}
			
		}
		
		public int LookInDirection (MoveDirection direction, int distance = 1)
		{
			
			if (distance <= 0 || direction == MoveDirection.Stay) {
				
				return lookable.Look (pov);
				
			} else if (distance > lookDistance [direction]) {
				
				return LookInDirection (direction, lookDistance [direction]);
				
			} else {
				
				if (lookResult [direction].Count < distance + 1)
					PrepareLookResult (direction, distance);
					
				return lookResult [direction] [distance];
				
			}
			
		}
		
		private void PrepareLookResult (MoveDirection direction, int distance)
		{
			
			if (distance == 0) {
				
				lookResult [direction].Clear ();
				lookResult [direction].Add (lookable.Look (pov));
				
			} else {
				
				if (lookResult [direction].Count < distance)
					PrepareLookResult (direction, distance - 1);
				
				pointsInDirectionCache [direction] =
					pov.LookInDirection (direction, distance, pointsInDirectionCache [direction]);
				
				Point[] pointsToCount = pointsInDirectionCache [direction] [distance];
				
				int result = 0;
				foreach (Point p in pointsToCount)
					result += lookable.Look (p);
				
				lookResult [direction].Add (result + lookResult [direction] [distance - 1]);
				
			}
			
		}
		
	}
}

