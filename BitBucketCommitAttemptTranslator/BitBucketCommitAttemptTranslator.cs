using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using CommitService.Contract;

namespace BitBucketCommitAttemptTranslator
{
    [Export(typeof(ITranslateCommitAttempt))]
    public class BitBucketCommitAttemptTranslator : ITranslateCommitAttempt
    {
        public TranslateCommitAttemptResult Execute(CommitAttempt attempt)
        {
            return new TranslateCommitAttemptResult
                       {
                           Success = true,
                           Commits = new [] { new CommitMessage() { Name = "BitBucket", SourceCommit = attempt.ToString() } }
                       };
        }

        private readonly Regex _taster = new Regex(@"['""]canon_url['""]\s*?:\s*?['""]https:\\/\\/bitbucket.org['""]", RegexOptions.IgnoreCase);

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