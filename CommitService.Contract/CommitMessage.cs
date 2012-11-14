using System;

namespace CommitService.Contract
{
    public class CommitMessages
    {
        public long[] Ids { get; set; }

        public CommitMessages(params long[] ids)
        {
            Ids = ids;
        }
    }

    public class CommitMessage //: IReturn<CommitResponse>
    {
        public long Id { get; set; }
        public string MessageId { get; set; }
        public string Name { get; set; }
        public string SourceCommit { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
        public string Hash { get; set; }
        public DateTime Date { get; set; }
        public string Author { get; set; }
    }
}