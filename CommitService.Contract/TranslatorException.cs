using System;

namespace CommitService.Contract
{
    public class TranslatorException : Exception
    {
        public ITranslateCommitAttempt Translator { get; set; }
        public CommitAttempt Attempt { get; set; }

        public TranslatorException(ITranslateCommitAttempt translator, CommitAttempt attempt) 
            : base(string.Format("Translator of type '{0}' was unable to process incoming CommitAttempt starting with body: {1}",
                                 translator.GetType(), attempt.Raw.Substring(0, attempt.Raw.Length < 50 ? attempt.Raw.Length : 50)))
        {
            Translator = translator;
            Attempt = attempt;
        }

        public TranslatorException(ITranslateCommitAttempt translator, CommitAttempt attempt, Exception innerException)
            : base(string.Format("Translator of type '{0}' was unable to process incoming CommitAttempt starting with body: {1}",
                                 translator.GetType(), attempt.Raw.Substring(0, attempt.Raw.Length < 50 ? attempt.Raw.Length : 50)), innerException)
        {
            Translator = translator;
            Attempt = attempt;
        }
    }
}