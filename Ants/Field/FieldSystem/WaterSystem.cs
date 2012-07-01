using System;

namespace Ants
{
	public class WaterSystem : FieldSystem
	{
		
		const int WATER_DROP_AMOUNT = 10;
		const int WATER_DROP_MIN = 400;
		const int WATER_DROP_MAX = 500;
		const int WATER_INIT_LIMIT = 100;
		const int WATER_MAX_DELTA = 0;
		
		public WaterSystem (Field field) : base (field)
		{
			
			// dropping initial shafts of water
			
			for (int i=0; i<WATER_DROP_AMOUNT; i++) {
			
				int size = rnd.Next (WATER_DROP_MIN, WATER_DROP_MAX);
				values [field.randomX (), field.randomY ()] = size;
				
			}
			
			// to some OK position
			
			for (int i=0; i<WATER_INIT_LIMIT; i++)
				if (!PerformFlow ())
					break;
			
		}
		
		private int SurfaceLevel (int x, int y)
		{
			return field.HeightAt(x,y) + values [x, y];
		}
		
		private Flow CalculateFlowAt(int x, int y)
		{
			Flow result = new Flow();
			
			for (int i=0; i<3; i++) {
				for (int j=0; j<3; j++) {
					
					int xd = i + x - 1;
					int yd = j + y - 1;
					
					if (field.Validate (xd, yd)) {
						
						if (SurfaceLevel (x, y) > SurfaceLevel (xd, yd) + WATER_MAX_DELTA) {
							
							int delta = SurfaceLevel (x, y) - SurfaceLevel (xd, yd);
							result.flowDirection [i, j] = delta;
							result.flowAmount = Math.Max (result.flowAmount, delta);
							
						}
						
					}
					
				}
			}
			
			return result;
			
		}
		
		private bool PerformFlowAt(int x, int y, Flow flow)
		{
			
			bool result = false;
			
			int [,] normalizedFlow = flow.NormalizedFlow();
			
			for (int i=0; i<3; i++) {
				for (int j=0; j<3; j++) {
					
					int xd = i + x - 1;
					int yd = j + y - 1;
					
					int flowThere = normalizedFlow [i, j];
					
					if (flowThere > 0) {
						
						result = true;
						values [xd, yd] += flowThere;
						values [x, y] -= flowThere;
						
					}
				}
			}
			
			return result;
			
		}
		
		private bool PerformFlow()
		{
			
			bool result = false;
			
			Flow[,] flows = new Flow[xSize, ySize];
			
			// calculating where the water will flow
			
			for (int x=0; x<xSize; x++) {
				for (int y=0; y<ySize; y++) {
					if (values [x, y] > 0) {
						flows [x, y] = CalculateFlowAt (x, y);
					}
				}
			}
			
			// performing the water flow
			
			for (int x=0; x<xSize; x++) {
				for (int y=0; y<ySize; y++) {
					if (values [x, y] > 0) {
						
						result = result || PerformFlowAt (x, y, flows [x, y]);
						
					}
				}
			}
			
			return result;
			
		}
		
		override public void Turn() {
			
			PerformFlow();
			
			// TODO: evaporation
			// TODO: rain
			
		}

		override public void Buffer(char [][] printBuffer)
		{
			for (int x=0; x<xSize; x++) {
				for (int y=0; y<ySize; y++) {
					
					int w = values [x, y];
					char c;
					
					if (w == 0)
						continue;
					else if (w < 3)
						c = '-';
					else if (w < 5)
						c = '~';
					else if (w < 7)
						c = '=';
					else
						c = '#';
					
					printBuffer [x][y] = c;
					
				}
			}
		}
	}
}

