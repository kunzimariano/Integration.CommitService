using System.ComponentModel.Composition;
using CommitService.Contract;

namespace GitHubCommitAttemptTranslator
{
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

        public bool CanProcess(CommitAttempt attempt)
        {
            return attempt.Raw.ToLower().Contains("source:'github'");
        }
    }
}