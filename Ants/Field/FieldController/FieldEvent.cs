using System;
namespace Ants
{
	public abstract class FieldEvent
	{
		
		public virtual double probability {
			get { return 0; }
		}
		
		public FieldEvent ()
		{
		}
		
		public abstract void Happen(Field field);
		
	}
}

