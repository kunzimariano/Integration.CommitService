namespace CommitService.Contract
{
    public class CommitMessage //: IReturn<CommitResponse>
    {
        public string Name { get; set; }
        public CommitAttempt OriginalAttempt { get; set; }
    }
}