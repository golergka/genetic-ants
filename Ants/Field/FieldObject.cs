using System;
using System.Collections.Generic;

namespace Ants
{
	
	public abstract class FieldObject : Point, IDisposable
	{
		
		public Field field;
		
		public virtual char symbol {
			get { return '.'; }
		}
		
		public virtual int symbolPriority {
			get { return 0; }
		}
		
		public FieldObject () {}
		
		public FieldObject (Field field, int x, int y)
		{
			
			this.field = field;
			this.x = x;
			this.y = y;
			
		}
		
		public abstract void Turn();
		
		private void MoveUp ()
		{
			
			if (y < field.ySize - 1)
				y++;
			
		}
		
		private void MoveDown ()
		{
			
			if (y > 0)
				y--;
			
		}
		
		private void MoveLeft ()
		{
			
			if (x > 0)
				x--;
			
		}
		
		private void MoveRight ()
		{
			
			if (x < field.xSize - 1)
				x++;
			
		}
		
		protected virtual void Move (MoveDirection direction)
		{
			
			switch (direction) {
				
			case MoveDirection.Down:
				MoveDown ();
				break;
				
			case MoveDirection.Left:
				MoveLeft ();
				break;
				
			case MoveDirection.Right:
				MoveRight ();
				break;
				
			case MoveDirection.Up:
				MoveUp ();
				break;
				
			default: 
				// just chill!
				break;
			}
			
		}
		
		protected List<FieldObject> Neighbours (int radius=0)
		{
			
			int xMin = Math.Max (0, x - radius);
			int xMax = Math.Min (x + radius, field.xSize - 1);
			
			int yMin = Math.Max (0, y - radius);
			int yMax = Math.Min (y + radius, field.ySize - 1);
			
			List<FieldObject> result = new List<FieldObject> ();
			
			for (int i=xMin; i<=xMax; i++)
				for (int j=yMin; j<=yMax; j++) 
					result.AddRange (field.objectsOnField [x] [y]);
			
			return result;
			
		}
		
		public void Dispose ()
		{
			
			field.RemoveFieldObject (this);
			
		}
	}
}