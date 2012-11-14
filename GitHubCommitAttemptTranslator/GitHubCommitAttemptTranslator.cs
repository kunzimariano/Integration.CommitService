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
            dynamic root = JObject.Parse(attempt.Raw);

            var commits = new List<CommitMessage>();

            foreach (var commitIem in root.commits)
            {
                var commit = new CommitMessage
                                 {
                                     Author = commitIem.author.name + " <" + commitIem.author.email + ">",
                                     Comment = commitIem.message,
                                     Date = commitIem.timestamp,
                                     SourceCommit = commitIem.ToString()
                                 };
                commits.Add(commit);
            }

            return new TranslateCommitAttemptResult
            {
                Success = true,
                Commits = commits
            };
        }

        private readonly Regex _taster = new Regex(@"['""]repository['""]\s*?:\s*?\{\s*['""]url['""]\s*?:\s*?[""']http://github.com", RegexOptions.IgnoreCase);

        public bool CanProcess(CommitAttempt attempt)
        {
            if (string.IsNullOrEmpty(attempt.Raw))
            {
                return false;
            }

            var isMatch = _taster.IsMatch(attempt.Raw);

            return isMatch;
        }
    }
}