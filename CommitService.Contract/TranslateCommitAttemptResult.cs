using System.Collections.Generic;

namespace CommitService.Contract
{
    public class TranslateCommitAttemptResult
    {
        public bool Success { get; set; }
        public IList<CommitMessage> Commits { get; set; }
    }
}