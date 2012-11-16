using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using CommitService.Contract;
using Infrastructure.Composition;
using ServiceStack.Common;
using ServiceStack.Messaging;
using ServiceStack.Redis;
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

        // TODO: what about exceptions, and what about exceptions when partial success, but not all??
        public object Any(CommitAttempt request)
        {
            var translationResult = Translate(request);

            if (translationResult != null)
            {
                var redisClient = new RedisClient();
                using (var commitsStore = redisClient.As<CommitMessage>())
                {
                    foreach (var commit in translationResult.Commits)
                    {
                        commit.Id = commitsStore.GetNextSequence();
                        commitsStore.Store(commit);
                    }
                }

                return new CommitAcknowledge { CanProcess = true };
            }

            return new CommitAcknowledge { CanProcess = false };
        }

        public object Get(CommitMessages request)
        {
            var redisClient = new RedisClient();
            using (var store = redisClient.As<CommitMessage>())
            {
                return request.Ids.IsEmpty()
                           ? store.GetAll()
                           : store.GetByIds(request.Ids);
            }
        }

        private TranslateCommitAttemptResult Translate(CommitAttempt attempt)
        {
            foreach (var translator in _translators)
            {
                try
                {
                    translator.Execute(attempt);

                    if (!translator.CanProcess(attempt))
                        continue;

                    var result = translator.Execute(attempt);

                    if (result.Success)
                        return result;

                    throw new TranslatorException(translator, attempt);
                }
                catch (Exception ex)
                {
                    throw new TranslatorException(translator, attempt, ex);
                }
            }

            //return null;
            throw new TranslatorNotFoundException(attempt);
        }
    }
}
