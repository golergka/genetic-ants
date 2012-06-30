using System;
using System.Collections.Generic;

namespace Ants
{
	public class Field
	{
		
		private List<FieldObject> objects = new List<FieldObject>();
		public readonly List<List<List<FieldObject>>> objectsOnField = new List<List<List<FieldObject>>>();

		public int [,] height;
		public int [,] water;
		public int [,] grass;
		
		public readonly int xSize;
		public readonly int ySize;
		
		static Random rnd = new Random();
		
		/*
		 * Global
		 */
		
		public Field (int xSize = 40, int ySize = 120)
		{
			
			this.xSize = xSize;
			this.ySize = ySize;
			
			// creating object table
			
			for (int x=0; x<xSize; x++)
				objectsOnField.Add (new List<List<FieldObject>> ());
			
			foreach (List<List<FieldObject>> column in objectsOnField)
				for (int y=0; y<ySize; y++)
					column.Add (new List<FieldObject> ());
				
			// creating height table
			
			height = new int[xSize, ySize];	
			GenerateTerrain ();
			
			// creating water
			
			water = new int[xSize, ySize];
			GenerateWater ();
			
			// creating grass table
			
			grass = new int [xSize, ySize];
			
			// creating buffer for printing
			
			buffer = new char[xSize][];
			
			for (int x=0; x<xSize; x++)
				buffer[x] = new char[ySize];
			
		}
		
		public void Turn ()
		{
			
			// water flows
			WaterFlow ();
			// grass grows
			GrassSpawn ();
			GrassGrow ();
			
			List<FieldObject> oldObjects = objects.GetRange(0, objects.Count);
			
			// little fucks are doing whatever they fancy
			foreach (FieldObject fieldObject in oldObjects)
				fieldObject.Turn ();
			
			// and god recounts his minions again
			foreach (List<List<FieldObject>> column in objectsOnField)
				foreach (List<FieldObject> cell in column)
					cell.Clear ();
			
			foreach (FieldObject fieldObject in objects)
				objectsOnField [fieldObject.x] [fieldObject.y].Add (fieldObject);
			
		}
		
		/*
		 * Coordinates
		 */
		
		public int randomX ()
		{
			return rnd.Next (0, xSize);
		}
		
		public int randomY ()
		{
			return rnd.Next (0, ySize);
		}
		
		public Point randomPoint ()
		{
			return new Point (randomX (), randomY ());
		}
		
		public bool Validate (int x, int y)
		{
			if ((x < 0) ||
				(y < 0) ||
				(x >= xSize) ||
				(y >= ySize))
				return false;
			else
				return true;
		}
		
		public bool Validate (Point point)
		{
			return this.Validate (point.x, point.y);
		}
		
		/*
		 * Objects
		 */
		
		public List<FieldObject> ObjectsAt (int x, int y)
		{
			
			List<FieldObject> result;
			
			if (Validate (x,y))
				result = objectsOnField [x] [y];
			else
				result = new List<FieldObject> ();
			
			return result;
				
			
		}
		
		public List<FieldObject> ObjectsAt (Point point)
		{
			return this.ObjectsAt (point.x, point.y);
		}
		
		public void AddFieldObject (FieldObject newObject)
		{
			
			objects.Add (newObject);
			objectsOnField [newObject.x] [newObject.y].Add (newObject);
			
		}
		
		public void RemoveFieldObject (FieldObject removingObject)
		{
			
			objects.Remove (removingObject);
			objectsOnField [removingObject.x] [removingObject.y].Remove (removingObject);
			
		}
		
		/*
		 * Terrain
		 */
		
		const int TERRAIN_DROP_AMOUNT = 20;
		const int TERRAIN_DROP_MIN = 100;
		const int TERRAIN_DROP_MAX = 200;
		const int TERRAIN_INIT_LIMIT = 100;
		const int TERRAIN_MAX_DELTA = 3;
		
		private bool TerrainFlow ()
		{
			
			bool result = false;
		
			for (int x=0; x<xSize; x++) {
				for (int y=0; y<ySize; y++) {
						
					for (int i=x-1; i<x+1; i++) {
						for (int j=y-1; j<y+1; j++) {
								
							if (Validate (i, j)) {
								
								if (height [i, j] < (height [x, y] - TERRAIN_MAX_DELTA)) {
										
									result = true;
									int deltaLevel = (height [x, y] - height [i, j]) / 2;
									height [x, y] -= deltaLevel;
									height [i, j] += deltaLevel;
										
								}	
							}		
						}
					}		
				}
			}
			
			return result;
			
		}
		
		private void GenerateTerrain ()
		{
			
			// drop peaks
			for (int i=0; i<TERRAIN_DROP_AMOUNT; i++)
				height [randomX (), randomY ()] = rnd.Next (TERRAIN_DROP_MIN, TERRAIN_DROP_MAX);
			
			// smooth
			
			for (int i=0; i<TERRAIN_INIT_LIMIT; i++)
				if (!TerrainFlow ())
					break;
			
			
		}
		
		public int HeightAt (Point p)
		{
			if (this.Validate (p))
				return height [p.x, p.y];
			else
				return 0;
		}
		
		/*
		 * Water
		 */
		
		const int WATER_DROP_AMOUNT = 10;
		const int WATER_DROP_MIN = 400;
		const int WATER_DROP_MAX = 500;
		const int WATER_INIT_LIMIT = 100;
		const int WATER_MAX_DELTA = 0;
		
		private void GenerateWater ()
		{
			
			for (int i=0; i<WATER_DROP_AMOUNT; i++) {
			
				int size = rnd.Next (WATER_DROP_MIN, WATER_DROP_MAX);
				water [randomX (), randomY ()] = size;
				
			}
			
			for (int i=0; i<WATER_INIT_LIMIT; i++)
				if (!WaterFlow ())
					break;
			
		}
		
		private int SurfaceLevel (int x, int y)
		{
			return height [x, y] + water [x, y];
		}
		
		private class Flow {
			
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
		
		private Flow CalculateFlowFrom (int x, int y)
		{
			Flow result = new Flow ();
			
			for (int i=0; i<3; i++) {
				for (int j=0; j<3; j++) {
					
					int xd = i + x - 1;
					int yd = j + y - 1;
					
					if (Validate (xd, yd)) {
						
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
		
		// returns true if something flowed
		private bool PerformFlowFrom (int x, int y, Flow flow)
		{
			
			bool result = false;
		
			int[,] normalizedFlow = flow.NormalizedFlow ();
			
			for (int i=0; i<3; i++) {
				for (int j=0; j<3; j++) {
					
					int xd = i + x - 1;
					int yd = j + y - 1;
					
					int flowThere = normalizedFlow [i, j];
					
					if (flowThere > 0) {
						
						result = true;
						water [xd, yd] += flowThere;
						water [x, y] -= flowThere;
						
					}
				}
			}
			
			return result;
			
		}
		
		private bool WaterFlow ()
		{
			
			bool result = false;
			
			Flow[,] flows = new Flow[xSize, ySize];
			
			// calculating where the water will flow
			
			for (int x=0; x<xSize; x++) {
				for (int y=0; y<ySize; y++) {
					if (water [x, y] > 0) {
						flows [x, y] = CalculateFlowFrom (x, y);
					}
				}
			}
			
			// performing the water flow
			
			for (int x=0; x<xSize; x++) {
				for (int y=0; y<ySize; y++) {
					if (water [x, y] > 0) {
						
						result = result || PerformFlowFrom (x, y, flows [x, y]);
						
					}
				}
			}
			
			return result;
			
		}
		
		public int WaterAt (Point p)
		{
			if (Validate (p))
				return water [p.x, p.y];
			else
				return 0;
		}
		
		/*
		 * Grass
		 */
		
		const double GRASS_GROW_PROBABILITY = 0.01;
		const double GRASS_SPAWN_PROBABILITY = 0.05;
		const int GRASS_DENSITY_LIMIT = 3;
		const int GRASS_GROW_DISTANCE = 1;
		
		private void GrassSpawn ()
		{
			if (rnd.NextDouble () < GRASS_SPAWN_PROBABILITY) {
				
				int newX = randomX ();
				int newY = randomY ();
				
				if (water [newX, newY] == 0 && grass [newX, newY] < GRASS_DENSITY_LIMIT)
					grass [newX, newY]++;
				
			}
		}
		
		private void GrassGrow ()
		{
			
			for (int x=0; x<xSize; x++) {
				for (int y=0; y<ySize; y++) {
					
					int g = grass [x, y];
					
					if (g > 0 && rnd.NextDouble() < GRASS_GROW_PROBABILITY) {
						
						int newX = x + rnd.Next (-GRASS_GROW_DISTANCE, 1 + GRASS_GROW_DISTANCE);
						int newY = y + rnd.Next (-GRASS_GROW_DISTANCE, 1 + GRASS_GROW_DISTANCE);
						
						if (Validate (newX, newY))
						if (water [newX, newY] == 0)
						if (grass [newX, newY] < GRASS_DENSITY_LIMIT)
							grass [newX, newY] ++;
						
					}
					
				}
			}
			
		}
		
		public int GrassAt (Point p)
		{
			if (Validate (p))
				return grass [p.x, p.y];
			else
				return 0;
		}
		
		/*
		 * Printing
		 */
		
		private char[][] buffer;
		
		public void BufferHeight ()
		{
			
			for (int x=0; x<xSize; x++) {
				for (int y=0; y<ySize; y++) {
					
					if (height [x, y] > 7)
						buffer [x][y] = '^';
					else
						buffer [x][y] = '.';
					
				}
			}
			
		}
		
		public void BufferWater ()
		{
			
			for (int x=0; x<xSize; x++) {
				for (int y=0; y<ySize; y++) {
					
					int w = water [x, y];
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
					
					buffer [x][y] = c;
					
				}
			}
			
		}
		
		public void BufferGrass ()
		{
			
			for (int x=0; x<xSize; x++) {
				for (int y=0; y<ySize; y++) {
					
					if (grass [x, y] > 0)
						buffer [x][y] = '"';
					
				}
			}
			
		}
		
		public void BufferObjets ()
		{
			int[,] priority = new int[xSize, ySize];
			
			foreach (FieldObject obj in objects) {
				int x = obj.x;
				int y = obj.y;
					
				if (priority [x, y] < obj.symbolPriority)
					buffer [x][y] = obj.symbol;
					
			}
		}
		
		public void PrintBuffer ()
		{
			
			for (int x=0; x<xSize; x++)
				Console.WriteLine(new String(buffer[x]));
			
		}
		
		public void Print ()
		{
			
			Console.Clear ();
			
			BufferHeight ();
			BufferWater ();
			BufferGrass ();
			BufferObjets ();
			
			PrintBuffer ();
		
		}
	}
}

