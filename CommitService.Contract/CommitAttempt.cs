using System.Collections.Generic;
using System.Collections.Specialized;

namespace CommitService.Contract
{
	public class CommitAttempt // : IReturn<CommitAcknowledge>
	{
		public string UserAgent { get; set; }
		public string RawBody { get; set; }
	}
}