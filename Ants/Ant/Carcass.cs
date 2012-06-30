using System;
namespace Ants
{
	public class Carcass : FieldObject
	{
		
		public override char symbol {
			get {
				return '&';
			}
		}
		
		public override int symbolPriority {
			get {
				return 5;
			}
		}
		
		private int timeout = 500;
		
		public Carcass ()
		{
		}
		
		public Carcass (Field field, int x, int y)
		{
			
			this.field = field;
			this.x = x;
			this.y = y;
			
		}
		
		public override void Turn ()
		{
			if (timeout-- <= 0)
				this.Dispose ();
		}
	}
}

