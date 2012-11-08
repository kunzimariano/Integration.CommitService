namespace CommitService.Contract
{
    public class TranslateCommitAttemptResult
    {
        public bool Success { get; set; }
        public CommitMessage Message { get; set; }
    }
}