using System;
using System.Collections.Generic;
using Genetic.Programming;

namespace Ants
{
	
	public class Ant : FieldObject
	{
		
		public override char symbol {
			get {
				
				int f = food / 30;
				
				if (f < 10)
					return f.ToString() [0];
				else
					return 'X';
			}
		}
		
		public override int symbolPriority {
			get { return 100; }
		}
		
		const int DIE_FOOD = 0;
		const int REPRODUCTION_FOOD = 500;
		const int START_FOOD = 300;
		const int REPRODUCTION_AGE = 20;
		const int GRASS_FOOD = 4;
		
		private int food = START_FOOD;
		private int _generation;
		private int generation {
			get {
				return _generation;
			}
			set {
				_generation = value;
				
				if (bornByGenerations.Count < value + 1)
					bornByGenerations.Add (1);
				else
					bornByGenerations [generation]++;
			}
		}
		private int age = 0;
		
		public static List<int> bornByGenerations = new List<int> ();
		
		public IComputable<MoveDirection> brain = new SimpleAntBrain();
		
		public Ant (Field field, int x, int y)
		{
			
			this.field = field;
			this.x = x;
			this.y = y;
			this.generation = 0;
			
		}
		
		public Ant (Field field, int x, int y, int generation )
		{
			
			this.field = field;
			this.x = x;
			this.y = y;
			this.generation = generation;
			
		}
		
		public override void Turn ()
		{
			
			age++;
			
			// check if there's enough food
			if (food <= DIE_FOOD)
				Die ();
			
			// check if it can reproduce
			
			if (age >= REPRODUCTION_AGE && food >= REPRODUCTION_FOOD) {
				
				Ant child = new Ant (field, x, y, generation + 1);
				child.food = this.food / 2;
				food = food / 2;
				field.AddFieldObject (child);
				
			}
			
//			if (food >= REPRODUCE_FOOD) {
//				
//				Ant child = new Ant (field, x, y, generation+1);
//				child.food = food * 2/3;
//				food = food * 2/3;
//				field.AddFieldObject (child);
//			}
			
//			foreach (FieldObject fieldObject in Neighbours())
//				if (fieldObject is Grass)
//					Eat ((Grass)fieldObject);
			
			if (field.grass [x, y] > 0) {
				field.grass [x, y]--;
				food += GRASS_FOOD;
			}
			
			Move (brain.Compute (new AntNavigationContext (this)));
			
			food--;
			
		}
		
//		private void Eat (Grass grass)
//		{
//			grass.Dispose ();
//			food += 2;
//		}
		
		private void Die ()
		{
			this.Dispose ();
			field.AddFieldObject (new Carcass (field, x, y));
		}
		
		protected override void Move (MoveDirection direction)
		{
			food--;
			base.Move (direction);
		}
		
	}
}

