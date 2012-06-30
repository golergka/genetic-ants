using System;
using System.Text.RegularExpressions;
using Genetic;

namespace StringExample
{
	public class StringIndividual : Individual
	{
		
		internal string value;
		
		const int INIT_LENGTH = 1;
		const int MUTATE_ADD_MAX = 1;
		const int MUTATE_RMV_MAX = 1;
		const int MUTATE_ALT_MAX = 5;
		internal const string ALLOWED_CHARS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz ";
		
		static Random rnd = new Random();
		
		static private char randomChar ()
		{
			return ALLOWED_CHARS [rnd.Next (ALLOWED_CHARS.Length)];
		}
		
		static private string randomString (int length)
		{
			
			char[] chars = new char[length];
			for (int i=0; i<length; ++i)
				chars [i] = randomChar ();
			return new string (chars);
			
		}
		
		override public void Mutate ()
		{
			
			int mutationType = rnd.Next (2); // not a bool because more types will be added later
			
			switch (mutationType) {
			case 0: // addition
				{
					int addLength = rnd.Next (MUTATE_ADD_MAX + 1);
					string newString = randomString (addLength);
					int position = rnd.Next (value.Length);
					value = value.Insert (position, newString);
					break;
				}
				
			case 1: // substracton
				{
					int removeMax = Math.Min (value.Length, MUTATE_RMV_MAX);
					int subLength = rnd.Next (removeMax + 1);
					int position = rnd.Next (value.Length - removeMax + 1);
					value = value.Remove (position, subLength);
					break;
				}
				
			case 2: // alternation
				{
					int alt = rnd.Next (MUTATE_ALT_MAX + 1);
					for (int i=0; i<alt; i++) {
						int altIndex = rnd.Next (value.Length);
						value.Remove (altIndex, 1);
						value.Insert (altIndex, new string (randomChar (),1) );
					}
					break;
				}

			}
			
		}
		
		public StringIndividual()
		{
			value = randomString ( INIT_LENGTH );
		}
		
		public StringIndividual (StringIndividual mother, StringIndividual father)
		{
//			value = mother.value + father.value;
			
			string m = mother.value;
			string f = father.value;
			
			bool mIsBigger = (m.Length > f.Length);
			
			int minLength = mIsBigger ? f.Length : m.Length;
			
			char[] chars = new char[minLength];
			
			for (int i=0; i<minLength; i++) {
				if (rnd.Next (1) == 0)
					chars [i] = mother.value [i];
				else
					chars [i] = father.value [i];
			}
			
			value = new string (chars);
			
			int maxLength = mIsBigger ? m.Length : f.Length;
			maxLength -= minLength;
			
			for (int i=minLength; i<maxLength; i++)
				value += (mIsBigger ? m : f) [i];
			
			
		}
		
		override public string ToString ()
		{
			
			return "[StringIndividual: " + value + " ]";
			
		}
	}

	public class StringProblem<T> : Problem<T> where T : StringIndividual {
		
		static Regex targetChecker = new Regex("[" + StringIndividual.ALLOWED_CHARS + "]+");
		
		private string _target;
		
		public StringProblem (string target)
		{
			
			if (targetChecker.Match (target).Success)
				this._target = target;
			else
				throw new ArgumentException ("Target string: " + target +
				                             " doesn't fit constraint: " + targetChecker.ToString ());
			
		}
		
		public override int test (T subject)
		{
			
			int compareLength = Math.Min (subject.value.Length, _target.Length);
			int result = -_target.Length - Math.Abs(_target.Length - subject.value.Length);
			
			for (int i=0; i<compareLength; i++)
				result += (subject.value [i] == _target [i]) ? 1 : 0;
			
			return result;
			
		}
		
		public override string ToString ()
		{
			return ("[StringProblem str: " + _target + " ]");
		}
	}

	

}

