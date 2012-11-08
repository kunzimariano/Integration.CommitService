namespace CommitService.Contract
{
    public interface ITranslateCommitAttempt
    {
        TranslateCommitAttemptResult Execute(CommitAttempt attempt);
        bool CanProcess(CommitAttempt attempt);
    }
}