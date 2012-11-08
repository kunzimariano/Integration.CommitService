using CommitService.Contract;
using NUnit.Framework;

namespace BitBucketCommitAttemptTranslator.Tests
{
    [TestFixture]
    public class BitBucketCommitAttemptTransaltorTests
    {
        private readonly BitBucketCommitAttemptTranslator subject = new BitBucketCommitAttemptTranslator();

        #region Pos

        [Test]
        public void CanProcess_is_true_for_valid_CommitAttempt_from_BitBucket()
        {
            const string attemptBody = 
@"{
  ""repository"": {
    ""website"": """",
    ""fork"": false,
    ""name"": ""Integration.CommitService.Merc"",
    ""scm"": ""git"",
    ""absolute_url"": ""\/jgough\/integration.commitservice.merc\/"",
    ""owner"": ""jgough"",
    ""slug"": ""integration.commitservice.merc"",
    ""is_private"": false
  },
  ""commits"": [
    {
      ""node"": ""b237d1c10c8e"",
      ""files"": [
        {
          ""type"": ""modified"",
          ""file"": ""README.md""
        }
      ],
      ""branch"": ""master"",
      ""utctimestamp"": ""2012-11-08 17:26:13+00:00"",
      ""author"": ""jogoshugh"",
      ""timestamp"": ""2012-11-08 18:26:13"",
      ""raw_node"": ""b237d1c10c8e31c76193a2dfca84f0a68843a39a"",
      ""parents"": [
        ""6ce4ada21125""
      ],
      ""raw_author"": ""jogoshugh <jgough@JGough.corp.versionone.net>"",
      ""message"": ""Mod to readme\n"",
      ""size"": -1,
      ""revision"": null
    }
  ],
  ""canon_url"": ""https:\/\/bitbucket.org"",
  ""user"": ""jgough""
}
";
            var result = RunCanProcessTest(attemptBody);

            Assert.True(result);
        }

        [Test]
        public void CanProcess_is_true_for_valid_CommitAttempt_with_mixed_case()
        {
            const string attemptBody =
@"{
        ""canon_url"" : ""https:\/\/bitbucket.org"",
}
";
            var result = RunCanProcessTest(attemptBody);

            Assert.IsTrue(result);
        }

        [Test]
        public void CanProcess_is_true_for_valid_CommitAttempt_even_with_mixed_quotes_and_extra_spaces()
        {
            const string attemptBody =
@"{
        'canon_uRl'   :        




            'HTTps:\/\/bItbUckET.oRG',
    ""Random Property""
}
";
            var result = RunCanProcessTest(attemptBody);

            Assert.IsTrue(result);
        }

        #endregion

        #region Neg

        [Test]
        public void CanProcess_is_false_when_CommitAttempt_missing_BitBucket_canon_url()
        {
            const string attemptBody = 
 @"{
  ""repository"": {
    ""website"": """",
    ""fork"": false,
    ""name"": ""Integration.CommitService.Merc"",
    ""scm"": ""git"",
    ""absolute_url"": ""\/jgough\/integration.commitservice.merc\/"",
    ""owner"": ""jgough"",
    ""slug"": ""integration.commitservice.merc"",
    ""is_private"": false
  },
  ""commits"": [
    {
      ""node"": ""b237d1c10c8e"",
      ""files"": [
        {
          ""type"": ""modified"",
          ""file"": ""README.md""
        }
      ],
      ""branch"": ""master"",
      ""utctimestamp"": ""2012-11-08 17:26:13+00:00"",
      ""author"": ""jogoshugh"",
      ""timestamp"": ""2012-11-08 18:26:13"",
      ""raw_node"": ""b237d1c10c8e31c76193a2dfca84f0a68843a39a"",
      ""parents"": [
        ""6ce4ada21125""
      ],
      ""raw_author"": ""jogoshugh <jgough@JGough.corp.versionone.net>"",
      ""message"": ""Mod to readme\n"",
      ""size"": -1,
      ""revision"": null
    }
  ],
 ""MISSING FROM HERE"",
  ""user"": ""jgough""
}";
            var result = RunCanProcessTest(attemptBody);

            Assert.False(result);
        }

        #endregion

        #region Helpers

        private bool RunCanProcessTest(string attemptBody)
        {
            var attemptMessage = new CommitAttempt { Raw = attemptBody };

            var result = subject.CanProcess(attemptMessage);

            return result;
        }

        #endregion
    }
}