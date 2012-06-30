using System;
using System.Collections.Generic;

namespace Genetic
{
	public class ProblemSet<T> : Problem<T>, IComparer<T> where T : Individual  {
		
		List<Problem<T>> set = new List<Problem<T>>();
		Dictionary<T,int> cache = new Dictionary<T, int>();
		
		const int MAX_COMPLEXITY = 100;
		
		bool useCache;
		
		public ProblemSet (Problem<T> singleProblem, bool useCache = false)
		{
			set.Add (singleProblem);
			this.useCache = useCache;
		}
		
		public ProblemSet (List<Problem<T>> set, bool useCache = false)
		{
			this.set = set;
			this.useCache = useCache;
		}
		
		private int _test (T subject)
		{
			int sum = 0;
			foreach (Problem<T> p in set) {
				
				int testResult = p.test (subject);
				
				if ((testResult + sum) > 0 &&
					testResult < 0 && sum < 0)
					
					sum = Int32.MinValue;
				else if ((testResult + sum) < 0 &&
					testResult > 0 && sum > 0)
					
					sum = Int32.MaxValue;
				else
					
					sum += testResult;
				
			}
			
			if ((sum == 0) && (subject is IMeasurable)) {
				
				int complexityBonus = MAX_COMPLEXITY - ((IMeasurable)subject).Complexity ();
				
//				Console.WriteLine ("sum: " + sum + " bonus: " + complexityBonus);
				
				sum += complexityBonus;
				
			}
			
			return sum;
		}
		
		override public int test (T subject)
		{
			if (useCache) {
			
				if (!cache.ContainsKey (subject))
					cache.Add (subject, _test (subject));
				
				return cache [subject];
				
			} else
				return _test (subject);
		}
		
		public int test (T subject, bool testing)
		{
			if (testing)
				return this._test (subject);
			else
				return this.test (subject);
		}
		
		public void clearCache ()
		{
			
			cache.Clear ();
			
		}
		
		public int Compare (T x, T y)
		{
			return test (x) - test (y);
		}
		
		public override string ToString ()
		{
			string result = "[ProblemSet:";
			foreach (Problem<T> p in set)
				result += " " + p.ToString ();
			result += " ]";
			return result;
		}
	} 
}

