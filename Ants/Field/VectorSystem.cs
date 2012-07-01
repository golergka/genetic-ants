using System;

namespace Ants
{
	public abstract class VectorSystem
	{
		
		public readonly int xSize;
		public readonly int ySize;
		
		protected static Random rnd = new Random();
		
		public VectorSystem (int xSize, int ySize)
		{
			
			this.xSize = xSize;
			this.ySize = ySize;
			
		}
		
		public VectorSystem(VectorSystem system)
		{
			
			this.xSize = system.xSize;
			this.ySize = system.ySize;
			
		}
		
		/*
		 * Coordinates
		 */
		
		public int randomX ()
		{
			return rnd.Next (0, xSize);
		}
		
		public int randomY ()
		{
			return rnd.Next (0, ySize);
		}
		
		public Point randomPoint ()
		{
			return new Point (randomX (), randomY ());
		}
		
		public bool Validate (int x, int y)
		{
			if ((x < 0) ||
				(y < 0) ||
				(x >= xSize) ||
				(y >= ySize))
				return false;
			else
				return true;
		}
		
		public bool Validate (Point point)
		{
			return this.Validate (point.x, point.y);
		}
		
		/*
		 * Behavior
		 */
		
		public abstract void Turn();
		
	}
}

