using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;
using CommitService.Contract;

namespace TFSCommitAttemptTranslator
{
	[Export(typeof(ITranslateCommitAttempt))]
	public class TfsCommitAttemptTranslator : ITranslateCommitAttempt
	{
		private readonly Regex _eventPattern = new Regex(@"(?<=<eventXml>).*?(?=<\/eventXml>)");
		private readonly Regex _canProcessPattern = new Regex(@"http://schemas.microsoft.com/TeamFoundation/2005/06/Services/Notification/03");

		public TranslateCommitAttemptResult Execute(CommitAttempt attempt)
		{
			XDocument document = DecodeContentAndCreateDocument(attempt);
			var checkIn = ParseXmlDocument(document);

			return new TranslateCommitAttemptResult
			{
				Commits = new List<CommitMessage> { checkIn },
				Success = true
			};
		}

		public bool CanProcess(CommitAttempt attempt)
		{
			if (string.IsNullOrEmpty(attempt.RawBody))
				return false;

			string content = HttpUtility.HtmlDecode(attempt.RawBody);

			return _canProcessPattern.IsMatch(content);
		}

		private XDocument DecodeContentAndCreateDocument(CommitAttempt attempt)
		{
			string content = HttpUtility.HtmlDecode(attempt.RawBody);
			Match match = _eventPattern.Match(content);
			var document = XDocument.Parse(match.Value);
			return document;
		}

		private CommitMessage ParseXmlDocument(XDocument document)
		{
			CommitMessage commitMessage;

			try
			{
				var c = document.Descendants("CheckinEvent").FirstOrDefault();
				commitMessage = new CommitMessage()
				{
					Author = c.Element("Committer").Value,
					Title = c.Element("Title").Value,
					Comment = c.Element("Comment").Value,
					//TODO: what time zone should we parse to?
					//Date = commit.Element("CreationDate").Value
				};
			}
			catch
			{
				throw new InvalidTfsAttemptException();
			}


			return commitMessage;
		}
	}
}
