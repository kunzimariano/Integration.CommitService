using System;

namespace CommitService.Contract
{
    public class TranslatorNotFoundException : Exception
    {
        public CommitAttempt Attempt { get; set; }

        public TranslatorNotFoundException(CommitAttempt attempt)
            : base(string.Format("Unable to find a translator for incoming CommitAttempt starting with body: {0}",
                                 attempt.RawBody.Substring(0, attempt.RawBody.Length < 50 ? attempt.RawBody.Length : 50)))
        {
            Attempt = attempt;
        }
    }
}