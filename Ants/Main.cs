using System;

namespace Ants
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			
			Console.WriteLine ("test");
			
			FieldController fieldController = new FieldController (30000);
			fieldController.events.Add (new Spawn (typeof(Fire), 0.0005));
			fieldController.events.Add (new Spawn (typeof(Ant), 0.001));
			
//			Field field = fieldController.field;
			
//			for (int i=0; i<5; i++)
//				field.AddFieldObject (new Water (field, field.randomX(), field.randomY()));
			
			while (fieldController.turn < fieldController.turnLimit) {
				fieldController.Turn ();
				ConsoleKeyInfo keyInfo = Console.ReadKey ();
				if (keyInfo.Key == ConsoleKey.Enter)
					fieldController.Run ();
				else if (keyInfo.Key == ConsoleKey.Spacebar)
					fieldController.Turn (100);
			}
			
			for (int i=0; i<Ant.bornByGenerations.Count; i++)
				Console.WriteLine ("Ants of ( " + i + " ) : " + Ant.bornByGenerations [i]);
			
		}
	}
}
