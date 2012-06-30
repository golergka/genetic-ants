using System;
namespace Ants
{
	
	public enum MoveDirection
	{ 
		Up,
		Down,
		Left,
		Right,
		Stay
		
		
		
	}
	
	public static class Extensions
	{
		
		public static readonly MoveDirection[] aroundDirections = new MoveDirection[4] {
			MoveDirection.Down,
			MoveDirection.Right,
			MoveDirection.Up,
			MoveDirection.Left,
		};
		
//		public static MoveDirection[] AroundDirections ()
//		{
//			return new MoveDirection[] {
//				MoveDirection.Left,
//				MoveDirection.Down,
//				MoveDirection.Right,
//				MoveDirection.Up
//			};
//		}
		
		public static MoveDirection TurnCounterClockwise (this MoveDirection direction)
		{
			
			switch (direction) {
				
			case MoveDirection.Stay:
				return direction;
				
			case MoveDirection.Down:
				return MoveDirection.Right;
				
			case MoveDirection.Left:
				return MoveDirection.Down;
				
			case MoveDirection.Up:
				return MoveDirection.Left;
				
			case MoveDirection.Right:
				return MoveDirection.Up;
				
			default:
				throw new ArgumentException ("Unknown direction!");
				
			}
			
		}
		
		public static MoveDirection TurnClockwise (this MoveDirection direction)
		{
			
			switch (direction) {
				
			case MoveDirection.Stay:
				return direction;
				
			case MoveDirection.Down:
				return MoveDirection.Left;
				
			case MoveDirection.Left:
				return MoveDirection.Up;
				
			case MoveDirection.Up:
				return MoveDirection.Right;
				
			case MoveDirection.Right:
				return MoveDirection.Down;
				
			default:
				throw new ArgumentException ("Unknown direction!");
				
			}
			
		}
		
		public static MoveDirection TurnAround (this MoveDirection direction)
		{
			
			switch (direction) {
				
			case MoveDirection.Stay:
				return direction;
				
			case MoveDirection.Down:
				return MoveDirection.Up;
				
			case MoveDirection.Left:
				return MoveDirection.Right;
				
			case MoveDirection.Up:
				return MoveDirection.Down;
				
			case MoveDirection.Right:
				return MoveDirection.Left;
				
			default:
				throw new ArgumentException ("Unknown direction!");
				
			}
			
		}
		
	}
	
	
}

