using System;
using System.Reflection;

namespace Ants
{
	public class Spawn : FieldEvent
	{
		
		private Type fieldObjectType;
		private double _probability;
		
		public override double probability {
			get {
				return _probability;
			}
		}
		
		public Spawn (Type fieldObjectType, double probability = 1)
		{
			
			this.fieldObjectType = fieldObjectType;
			this._probability = probability;
			
		}
		
		public override void Happen (Field field)
		{
			ConstructorInfo constructor = fieldObjectType.GetConstructor(
				new Type[] { typeof(Field), typeof(int), typeof(int) } );
			FieldObject newObject =
				(FieldObject)constructor.Invoke (new object [] { field, field.randomX (), field.randomY () } );
//			newObject.x = field.randomX ();
//			newObject.y = field.randomY ();
//			newObject.field = field;
			field.AddFieldObject (newObject);
			
		}
	}
}

