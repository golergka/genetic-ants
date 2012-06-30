//using System;
//namespace Ants
//{
//	public class Water : FieldObject
//	{
//		
//		public override char symbol {
//			get {
//				if (level < 3)
//					return '-';
//				else if (level < 5)
//					return '~';
//				else if (level < 7)
//					return '=';
//				else
//					return '#';
//			}
//		}
//		
//		public override int symbolPriority {
//			get {
//				return 1;
//			}
//		}
//		
//		private int level;
//		
//		const int MAX_DELTA_LEVEL = 5;
//		const int DEFAULT_LEVEL = 200;
//		const double EVAPORATE_PROBABILITY = 0.001;
//		
//		static Random rnd = new Random();
//		
//		public Water ()
//		{
//		}
//		
//		public Water (Field field, int x, int y)
//		{
//			
//			this.field = field;
//			this.x = x;
//			this.y = y;
//			this.level = DEFAULT_LEVEL;
//			
//		}
//		
//		public Water (Field field, int x, int y, int level)
//		{
//			
//			this.field = field;
//			this.x = x;
//			this.y = y;
//			this.level = level;
//			
//		}
//		
//		public override void Turn ()
//		{
//			
//			if (level > 1) {
//				
//				for (int i=x-1; i<=x+1; i++) {
//					for (int j=y-1; j<=y+1; j++) {
//						
//						if (field.Validate (i, j)) {
//						
//							Water foundWater = null;
//						
//							foreach (FieldObject obj in field.ObjectsAt(i,j)) {
//								if (obj is Water) {
//									foundWater = (Water)obj;
//									break;
//								}
//							}
//						
//							if (foundWater == null) {
//							
//								int newLevel = level / 2 + field.height [i, j] - field.height [x, y];
//							
//								if (newLevel > 0) {
//									field.AddFieldObject (new Water (field, i, j, newLevel));
//									level -= newLevel;
//								}
//							} else if (foundWater.level <
//								(level - MAX_DELTA_LEVEL + field.height [i, j] - field.height [x, y])) {
//								
//								int deltaLevel = ((level - foundWater.level) / 2)
//									+ field.height [i, j] - field.height [x, y];
//								level -= deltaLevel;
//								foundWater.level += deltaLevel;
//								
//							}
//							
//						}
//						
//					}
//				}
//			}
//			
//			if (rnd.NextDouble() < EVAPORATE_PROBABILITY)
//				level--;
//			
//			if (level <= 0)
//				this.Dispose ();
//			
//		}
//		
//	}
//}
//
