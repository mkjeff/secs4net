using System;

namespace Secs4Net.Exceptions
{
	public class SecsException :
		Exception
	{
		public SecsException(string message)
			: base(message)
		{
		}
	}
}