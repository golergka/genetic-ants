using System;
using System.Collections.Generic;

namespace Ants
{
	public class FieldController
	{
		
		public Field field;
		public int turnLimit;
		public int turn = 0;
		
		public List<FieldEvent> events = new List<FieldEvent> ();
		
		static Random rnd = new Random();
		
		public FieldController (int turnLimit = 100)
		{
			
			this.turnLimit = turnLimit;
			this.field = new Field ();
			this.field.AddFieldObject (new Ant (field, field.randomX(), field.randomY()));
			
		}
		
		public void Turn ()
		{
			turn++;
			
			field.Turn ();
			
			foreach (FieldEvent e in events)
				if (rnd.NextDouble () < e.probability)
					e.Happen (field);
			
			field.Print ();
			
			Console.WriteLine ("Turn: " + turn);
			
			
		}
		
		public void Turn (int turns)
		{
			for (int i=0; i<turns; i++)
				Turn ();
		}
		
		public void Run ()
		{
			
			for (; turn<this.turnLimit; turn++) {
				
				Turn ();
				
			}
			
		}
	}
}

