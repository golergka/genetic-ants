using System;

namespace Ants
{
	public class Flow {
			
			public int flowAmount = 0;
			public int [,] flowDirection = new int [3,3];
			
			private int DirectionSum ()
			{
				
				int result = 0;
				
				for (int i=0; i<3; i++)
					for (int j=0; j<3; j++)
						result += flowDirection [i, j];
				
				return result;
				
			}
			
			public int[,] NormalizedFlow ()
			{
				
				int[,] result = new int[3, 3];
				int sum = DirectionSum ();
				
				
				if (sum>0)
					for (int i=0; i<3; i++)
						for (int j=0; j<3; j++)
							result [i, j] = Convert.ToInt32 (
								(double)flowAmount
								*
								(
									(double)flowDirection[i,j] / (double)sum
								)
								);
				
				return result;
				
			}
			
		}
}

