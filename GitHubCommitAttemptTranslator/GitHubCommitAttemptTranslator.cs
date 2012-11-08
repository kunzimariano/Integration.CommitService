using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using CommitService.Contract;

namespace GitHubCommitAttemptTranslator
{
    /// <summary>
    /// Translates incoming post-receive hook POST bodies from GitHub into a CommitMessage. See
    /// https://help.github.com/articles/post-receive-hooks for info on the format. 
    /// </summary>
    [Export(typeof(ITranslateCommitAttempt))]
    public class GitHubCommitAttemptTranslator : ITranslateCommitAttempt
    {
        public TranslateCommitAttemptResult Execute(CommitAttempt attempt)
        {
            return new TranslateCommitAttemptResult
            {
                Success = true,
                Message = new CommitMessage() { Name = "GitHub", OriginalAttempt = attempt }
            };
        }

        private Regex _taster = new Regex(@"['""]repository['""]:\s*?\{\s*['""]url['""]:\s*?[""']http://github.com");

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