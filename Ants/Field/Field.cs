using System;
using System.Collections.Generic;

namespace Ants
{
	public class Field : VectorSystem
	{

		// old vector systems

		// public int [,] height;
		public int [,] grass;

		// new vector systems

		public FlowSystem water;
		public FlowSystem terrain;
		
		/*
		 * Global
		 */
		
		public Field (int xSize = 40, int ySize = 120) : base (xSize, ySize)
		{
			
			// creating object table
			
			for (int x=0; x<xSize; x++)
				objectsOnField.Add (new List<List<FieldObject>> ());
			
			foreach (List<List<FieldObject>> column in objectsOnField)
				for (int y=0; y<ySize; y++)
					column.Add (new List<FieldObject> ());
				
			// creating height table
			
			PrintCharTable terrainPrintTable = new PrintCharTable();

			terrainPrintTable.defaultLowChar = '.';
			terrainPrintTable.printChars.Add(7, '^');
			terrainPrintTable.defaultHiChar = '^';

			terrain = new FlowSystem(
				this,
				terrainPrintTable,
				TERRAIN_DROP_AMOUNT,
				TERRAIN_DROP_MIN,
				TERRAIN_DROP_MAX,
				TERRAIN_INIT_LIMIT,
				TERRAIN_MAX_DELTA
				);
			
			// creating water

			PrintCharTable waterPrintTable = new PrintCharTable();

			waterPrintTable.defaultLowChar = null;
			waterPrintTable.printChars.Add(3, '-');
			waterPrintTable.printChars.Add(5, '~');
			waterPrintTable.printChars.Add(7, '=');
			waterPrintTable.defaultHiChar = '#';

			water = new FlowSystem(
				this,
				waterPrintTable,
				WATER_DROP_AMOUNT,
				WATER_DROP_MIN,
				WATER_DROP_MAX,
				WATER_INIT_LIMIT,
				WATER_MAX_DELTA
				);
			
			// creating grass table
			
			grass = new int [xSize, ySize];
			
			// creating buffer for printing
			
			buffer = new char[xSize][];
			
			for (int x=0; x<xSize; x++)
				buffer[x] = new char[ySize];
			
		}
		
		override public void Turn ()
		{
			
			// water flows
			water.Turn();

			// notice — terrain doesn't turn!
			// that's because we don't really want it to be flowing

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
		 * Objects
		 */

		private List<FieldObject> objects = new List<FieldObject>();
		public readonly List<List<List<FieldObject>>> objectsOnField = new List<List<List<FieldObject>>>();
		
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
		
		public int HeightAt(int x, int y)
		{
			
			if (terrain != null)
				return terrain.Value(x,y);
			else
				return INVALID_COORDINATES_RETURN;
			
		}
		
		public int HeightAt (Point p)
		{

			return HeightAt(p.x, p.y);

		}
		
		/*
		 * Water
		 */

		const int WATER_DROP_AMOUNT = 10;
		const int WATER_DROP_MIN = 400;
		const int WATER_DROP_MAX = 500;
		const int WATER_INIT_LIMIT = 100;
		const int WATER_MAX_DELTA = 0;
		
		public int WaterAt(int x, int y)
		{
			
			return water.Value(x,y);
			
		}

		public int WaterAt (Point p)
		{
			
			return water.Value(p);

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
				
				if (WaterAt(newX, newY) == 0 && grass [newX, newY] < GRASS_DENSITY_LIMIT)
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
						if (WaterAt(newX, newY) == 0)
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
		
		public void BufferTerrain ()
		{
			
			terrain.Buffer(ref buffer);
			
		}
		
		public void BufferWater ()
		{
			
			water.Buffer(ref buffer);
			
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
			
			BufferTerrain ();
			BufferWater ();
			BufferGrass ();
			BufferObjets ();
			
			PrintBuffer ();
		
		}
	}
}

