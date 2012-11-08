using System.ComponentModel.Composition;
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
                           Message = new CommitMessage() { Name = "BitBucket", OriginalAttempt = attempt }
                       };
        }

        public bool CanProcess(CommitAttempt attempt)
        {
            return attempt.Raw.ToLower().Contains("source:'bitbucket'");
        }
    }
}