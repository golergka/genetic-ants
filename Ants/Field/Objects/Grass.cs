//using System;
//using System.Collections.Generic;
//
//namespace Ants
//{
//	public class Grass : FieldObject
//	{
//		
//		const double GROW_PROBABILITY = 0.01;
//		const int DENSITY_LIMIT = 3;
//		
//		static Random rnd = new Random();
//		
//		public override char symbol {
//			get {
//				return ',';
//			}
//		}
//		
//		public override int symbolPriority {
//			get {
//				return 1;
//			}
//		}
//		
//		public static int grassCount (List<FieldObject> objects)
//		{
//			
//			int result = 0;
//			foreach (FieldObject obj in objects)
//				if (obj is Grass)
//					result++;
//			return result;
//			
//		}
//		
//		public Grass (Field field, int x, int y)
//		{
//			
//			this.field = field;
//			this.x = x;
//			this.y = y;
//			
//		}
//		
//		public override void Turn ()
//		{
//			
//			if (Grass.grassCount (field.ObjectsAt (x, y)) > DENSITY_LIMIT)
//				this.Dispose ();
//			
//			if (rnd.NextDouble () < GROW_PROBABILITY) {
//				
//				int newX = x + rnd.Next (-1, 2);
//				int newY = y + rnd.Next (-1, 2);
//				
//				if (field.Validate (newX, newY))
//				if (field.water [newX, newY] == 0)
//					field.AddFieldObject (new Grass (field, newX, newY));
//				
////				foreach (FieldObject obj in field.ObjectsAt(x,y))
////					if (obj is Water) {
////						hasWater = true;
////						break;
////					}
////				
////				if (field.Validate (newX, newY) && !hasWater)
////					field.AddFieldObject (new Grass (field, newX, newY));
//			}
//				
//		}
//		
//	}
//}
//
