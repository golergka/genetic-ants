using System;
namespace Ants
{
	public class Fire : FieldObject
	{
		
		public override char symbol {
			get {
				return '*';
			}
		}
		
		public override int symbolPriority {
			get {
				return 10;
			}
		}
		
		public Fire () { }
		
		public Fire (Field field, int x, int y)
		{
			
			this.field = field;
			this.x = x;
			this.y = y;
			
		}
		
		public override void Turn ()
		{
			
			bool burnedSomething = false;
			
			if (field.grass [x, y] > 0) {
				field.grass [x, y]--;
				burnedSomething = true;
			}
					
			if (!burnedSomething)
				this.Dispose ();
			else {
				for (int i=x-1; i<=x+1; i++) {
					for (int j=y-1; j<=y+1; j++) {
						
						if (field.Validate (i, j)) {
							
							bool alreadyFire = false;
						
							foreach (FieldObject possiblyFire in field.ObjectsAt(i,j)) {
								if (possiblyFire is Fire) {
									alreadyFire = true;
									break;
								}
							}
							
							if (!alreadyFire)
								field.AddFieldObject (new Fire (field, i, j));
							
						}
						
					}
				}
			}
		}
	}
}

