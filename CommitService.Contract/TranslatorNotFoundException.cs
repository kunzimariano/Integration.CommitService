using System;

namespace CommitService.Contract
{
    public class TranslatorNotFoundException : Exception
    {
        public CommitAttempt Attempt { get; set; }

        public TranslatorNotFoundException(CommitAttempt attempt)
            : base(string.Format("Unable to find a translator for incoming CommitAttempt starting with body: {0}",
                                 attempt.Raw.Substring(0, 50)))
        {
            Attempt = attempt;
        }
    }
}