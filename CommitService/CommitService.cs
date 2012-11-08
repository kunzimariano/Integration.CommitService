using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CommitService.Contract;
using Infrastructure.Composition;
using ServiceStack.Messaging;
using ServiceStack.ServiceInterface;

namespace CommitService
{
    public class CommitService : Service
    {
        public CommitService()
        {
            new PartsAssembler().ComposeParts(this);
        }

        public IMessageFactory MessageFactory { get; set; }

        [ImportMany]
        private IEnumerable<ITranslateCommitAttempt> _translators = new List<ITranslateCommitAttempt>();

        public object Any(CommitAttempt request)
        {
            var translationResult = Translate(request);

            var translationResults = new List<TranslateCommitAttemptResult> { translationResult };

            using (var producer = MessageFactory.CreateMessageProducer())
            {
                translationResults.Select(t => t.Message).ToList().ForEach(producer.Publish);
            }

            // Note: Should return false when cannot, instead of throwing the exception,
            // which will require manually sending to the DeadLetterQueue I think...
            return new CommitAcknowledge { CanProcess = true };
        }

        private TranslateCommitAttemptResult Translate(CommitAttempt attempt)
        {
            foreach (var translator in _translators)
            {
                try
                {
                    if (translator.CanProcess(attempt))
                    {
                        var result = translator.Execute(attempt);

                        if (result.Success)
                        {
                            return result;
                        }

                        throw new TranslatorException(translator, attempt);
                    }
                }
                catch (Exception ex)
                {
                    throw new TranslatorException(translator, attempt, ex);
                }
            }

            throw new TranslatorNotFoundException(attempt);
        }

        public object Any(CommitMessage message)
        {
            return new CommitResponse { Result = message.Name + " was processed." };
        }
    }
}
