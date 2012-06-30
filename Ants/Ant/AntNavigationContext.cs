using System;
using System.Collections.Generic;
using Genetic.Programming;

namespace Ants
{
	public class AntNavigationContext : ComputationContext<MoveDirection>
	{
		
		private Ant ant;
		
		private Dictionary<MoveDirection, List<int>> _grassAmount = new Dictionary<MoveDirection, List<int>>();
		public Dictionary<MoveDirection, int> distanceToBorder = new Dictionary<MoveDirection, int>();
		public Dictionary<MoveDirection, int> lookDistance = new Dictionary<MoveDirection, int>();
		
		public int CountGrassInDirection (MoveDirection direction, int distance=1)
		{
			
			if (distance == 0 || direction == MoveDirection.Stay) {
				
				return CountGrass (ant);
				
			} else if (distance > lookDistance [direction]) {
				
				return CountGrassInDirection (direction, lookDistance [direction]);
				
			} else {
				
				if (_grassAmount [direction].Count < distance + 1)
					PrepareGrassAmount (direction, distance);
				
				return _grassAmount [direction] [distance];
				
			}
			
		}
		
		private int CountGrass (int x, int y)
		{
			
			if (ant.field.Validate (x, y))
				return ant.field.grass [x, y];
			else
				return 0;
			
		}
		
		private int CountGrass (Point p)
		{
			return CountGrass (p.x, p.y);
		}
		
		private Dictionary<MoveDirection, Point[][]> pointsInDirectionCache = new Dictionary<MoveDirection, Point[][]>();
		
		private void PrepareGrassAmount (MoveDirection direction, int distance)
		{
			
			if (distance == 0) {
				
				_grassAmount [direction].Clear ();
				_grassAmount [direction].Add (CountGrass (ant));
				
			} else {
				
				if (_grassAmount [direction].Count < distance) {
					
					PrepareGrassAmount (direction, distance - 1);
					
				}
				
				pointsInDirectionCache [direction] =
					ant.LookInDirection (direction, distance, pointsInDirectionCache [direction]);
				
				Point[] pointsToCount = pointsInDirectionCache [direction] [distance];
				
				int grass = 0;
				foreach (Point p in pointsToCount)
					grass += CountGrass (p);
				
				_grassAmount [direction].Add (grass + _grassAmount [direction] [distance - 1]);
				
			}
			
		}
		
		// state of the world doesn't change while this object exists, so it can safely buffer it
		public AntNavigationContext (Ant ant)
		{
			this.ant = ant;
			
			distanceToBorder.Add (MoveDirection.Left, ant.x);
			distanceToBorder.Add (MoveDirection.Right, ant.field.xSize - ant.x);
			distanceToBorder.Add (MoveDirection.Down, ant.y);
			distanceToBorder.Add (MoveDirection.Up, ant.field.ySize - ant.y);
			
			foreach (MoveDirection d in Extensions.aroundDirections) {
				
				lookDistance.Add (d, distanceToBorder [d] * 2);
				_grassAmount.Add (d, new List<int> ());
				pointsInDirectionCache.Add (d, null);
				
			}
			
			
			
		}
		
	}
}

