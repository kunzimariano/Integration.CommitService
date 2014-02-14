using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using CommitService.Contract;
using Newtonsoft.Json.Linq;

namespace GitHubCommitAttemptTranslator
{
    /// <summary>
    /// Translates incoming post-receive hook POST bodies from GitHub into a CommitMessage. See
    /// https://help.github.com/articles/post-receive-hooks for info on the format. 
    /// </summary>
    [Export(typeof (ITranslateCommitAttempt))]
    public class GitHubCommitAttemptTranslator : ITranslateCommitAttempt
    {
        public TranslateCommitAttemptResult Execute(CommitAttempt attempt)
        {
            var body = GetDecodedBody(attempt.Raw);

            dynamic root = JObject.Parse(body);

            var commits = new List<CommitMessage>();

            // TODO: make a copy instead of parse if can be done...
            dynamic rootWithoutCommits = root.DeepClone();
            rootWithoutCommits.commits.RemoveAll();

            foreach (var commitItem in root.commits)
            {
                var commit = new CommitMessage
                                 {
                                     MessageId = commitItem.id,
                                     Author = commitItem.author.name + " <" + commitItem.author.email + ">",
                                     Comment = commitItem.message,
                                     Date = commitItem.timestamp,
                                 };
                dynamic fullContextCommit = rootWithoutCommits.DeepClone();
                fullContextCommit.commits.Add(commitItem);
                commit.SourceCommit = fullContextCommit.ToString();
                commits.Add(commit);
            }

            return new TranslateCommitAttemptResult
            {
                Success = true,
                Commits = commits
            };
        }

        private readonly Regex _taster = new Regex(@"['""]repository['""]:.*?\{.*?['""]url['""]:\s*?[""']https?://github\.com", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public bool CanProcess(CommitAttempt attempt)
        {
            if (string.IsNullOrEmpty(attempt.Raw))
            {
                return false;
            }

            var body = GetDecodedBody(attempt.Raw);

            var isMatch = _taster.IsMatch(body);

            return isMatch;
        }

        private string GetDecodedBody(string raw)
        {
            if (raw.Contains("payload="))
            {
                var items = raw.Split(new char[] { '=' });
                raw = System.Web.HttpUtility.UrlDecode(items[1]);
            }
            return raw;
        }
    }
}