using System;
namespace Ants
{
	public abstract class FieldSystem : VectorSystem
	{
		
		protected Field field;
		
		protected int [,] values;
		
		public FieldSystem (Field field) : base (field)
		{
			
			this.field = field;
			values = new int [xSize, ySize];
			
		}
		
		public int Value(int x, int y)
		{
			
			return values[x,y];
			
		}

		public abstract void Buffer(char [][] printBuffer);

	}
}

