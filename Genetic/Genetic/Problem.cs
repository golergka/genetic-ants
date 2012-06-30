using System;
namespace Genetic
{
	abstract public class Problem<T> where T : Individual {
		
		abstract public int test(T subject);
		
	}
}

