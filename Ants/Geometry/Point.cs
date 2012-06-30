using System;
using System.Collections.Generic;

namespace Ants
{
	public class Point
	{
		
		public int x = 0;
		public int y = 0;
		
		public Point() { }
		
		public Point (int x, int y)
		{
			this.x = x;
			this.y = y;
		}
		
		public Point MoveInDirection (MoveDirection direction, int distance = 1)
		{
			
			switch (direction) {
				
			case MoveDirection.Stay:
				return new Point(x,y);
				
			case MoveDirection.Down:
				return new Point (x, y - distance);
				
			case MoveDirection.Left:
				return new Point (x - distance, y);
				
			case MoveDirection.Right:
				return new Point (x + distance, y);
				
			case MoveDirection.Up:
				return new Point (x, y + distance);
				
				
			default:
				throw new ArgumentException ("Unknown direction!");
			}
			
		}

//		/*
//		 * Returns points in particular direction, like this:
//		 * 
//		 *    rrrr.
//		 *     rr.
//		 *      X
//		 * 
//		 * X — the initial point
//		 * r — points that are returned
//		 * (this example is valid for MoveDirection.Up)
//		 * 
//		 * for direction == MoveDirection.Stay and distance == 0 the empty list is returned
//		 * 
//		 */
//		public List<Point> LookInDirection (MoveDirection direction, int distance = 1)
//		{
//			
//			List<Point> result = new List<Point> ();
//			
//			if (direction != MoveDirection.Stay && distance != 0) {
//				
//				// this condition isn't neccessary for logic, but a bit of optimization
//				// without the condition, recursion would go not to 1, but to 0 — just another wasted call
//				if (distance != 1)
//					result.AddRange (LookInDirection (direction, distance - 1));
//				
//				result.AddRange (LookInDirectionAtExactDistance (direction, distance));
//				
//			}
//			
//			return result;
//			
//		}
//		
//		/*
//		 * This method returns:
//		 * 
//		 * rrrrrr.
//		 *  .....
//		 *   ...
//		 *    X
//		 * 
//		 */
//		public List<Point> LookInDirectionAtExactDistance (MoveDirection direction, int distance)
//		{
//			List<Point> result = new List<Point> ();
//			
//			if (direction != MoveDirection.Stay && distance != 0) {
//				/*
//				 * BrrMrr.
//				 *  .....
//				 *   ...
//				 *    X
//				 * 
//				 * X — the initial point
//				 * M — rangeMiddle
//				 * B — beginning point of iteration
//				 * 
//				 * (in this example, distance == 3 and direction == MoveDirection.Up)
//				 * 
//				 */
//				
//				Point rangeMiddle = this.MoveInDirection (direction, distance);
//				Point newPoint = rangeMiddle.MoveInDirection (direction.TurnCounterClockwise(), distance);
//				
//				result.Add (newPoint);
//				MoveDirection aroundDirection = direction.TurnClockwise();
//				
//				for (int i=0; i<distance*2 - 1; i++) {
//					newPoint = newPoint.MoveInDirection (aroundDirection);
//					result.Add (newPoint);
//				}
//				
//			}
//			
//			return result;
//		}
		
		/*
		 * Here's how the return looks:
		 * 
		 *     8
		 *    678
		 *   45678
		 *  2345678
		 * x12345678
		 *  .345678
		 *   .5678
		 *    .78
		 *     .
		 * 
		 * x — the initial point
		 * i — the points in result[i]
		 * 
		 * notice that the return isn't symmetrical — so that LookInDirection for 4 directions don't intersect
		 * 
		 * recursiveResult is the result of the same function with distance-1
		 * unless you want some optimizations, it's safe to leave it at null
		 * 
		 */
		public Point[][] LookInDirection (MoveDirection direction, int distance, Point[][] recursiveResult = null)
		{
			
			Point[][] result = new Point [distance + 1][];
			
			if (direction == MoveDirection.Stay || distance == 0) {
				
				result [0] = new Point[1];
				result [0] [0] = new Point (x, y);
			
			} else {
			
				if (recursiveResult == null)
					LookInDirection (direction, distance - 1).CopyTo (result, 0);
				else
					recursiveResult.CopyTo (result, 0);
				
				Point[] pointsAtDistance = new Point[distance];
				Point[] pointsAtPreviousDistance = result [distance - 1];
				
				if (distance == 1) {
					
					pointsAtDistance [0] = pointsAtPreviousDistance [0].MoveInDirection (direction);
					
				} else {
				
					bool evenDistance = (distance % 2) == 0;
					int d = 0;
				
					if (evenDistance && distance > 1) {
						pointsAtDistance [0] =
						pointsAtPreviousDistance [0].MoveInDirection (direction.TurnCounterClockwise ());
						d = 1;
					}
				
					for (int i=0; i<distance-1; i++)
						pointsAtDistance [i + d] =
						pointsAtPreviousDistance [i].MoveInDirection (direction);
				
					if (!evenDistance && distance > 1)
						pointsAtDistance [distance - 1] =
						pointsAtPreviousDistance [distance - 2].MoveInDirection (direction.TurnClockwise ());
					
				}
				
				result [distance] = pointsAtDistance;

			}
					                     
			return result;		                    
					                     
		}
		
	}
}

