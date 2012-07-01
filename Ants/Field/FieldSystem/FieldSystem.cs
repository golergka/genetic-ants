using System;
using System.Collections.Generic;

namespace Ants
{

	public class PrintCharTable
	{

		public SortedDictionary<int, char?> printChars = new SortedDictionary<int, char?>();
		public char? defaultLowChar;
		public char? defaultHiChar;

		public char? Print(int value)
		{

			char? result = defaultLowChar;

			bool aboveMax = true;
			foreach(int i in printChars.Keys)
				if (value >= i)
					result = printChars[i];
				else {
					aboveMax = false;
					break;
				}

			if (aboveMax)
				result = defaultHiChar;

			return result;

		}

	}

	public abstract class FieldSystem : VectorSystem
	{
		
		protected Field field;
		
		protected int [,] values;

		private PrintCharTable table;
		
		public FieldSystem (Field field, PrintCharTable table) : base (field)
		{
			
			this.field = field;
			this.table = table;
			values = new int [xSize, ySize];
			
		}
		
		public int Value(int x, int y)
		{
			if (this.Validate(x,y))	
				return values[x,y];
			else
				return INVALID_COORDINATES_RETURN;
			
		}

		public int Value(Point p)
		{
			return Value(p.x, p.y);
		}

		public void Buffer(ref char [][] printBuffer)
		{
			for (int x=0; x<xSize; x++) {
				for (int y=0; y<ySize; y++) {
					
					int w = values [x, y];
					char? c = table.Print(w);

					if (c != null)
						printBuffer [x][y] = (char) c;
					
				}
			}
		}

	}
}

