using System;
namespace Genetic
{
	
	public interface IMutatable
	{
		
		void Mutate();
		
	}
	
	public interface ICrossoverable
	{
		
		object Crossover (object mother, object father);
		
	}
	
	abstract public class Individual : IMutatable
	{
		
		abstract public void Mutate ();
		
		// random generation
		public Individual () { }
		
		// crossover
		public Individual (Individual mother, Individual father) { }
		
	}
}

