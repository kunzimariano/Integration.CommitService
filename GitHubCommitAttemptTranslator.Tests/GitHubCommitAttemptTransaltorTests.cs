using CommitService.Contract;
using NUnit.Framework;

namespace GitHubCommitAttemptTranslator.Tests
{
    [TestFixture]
    public class GitHubCommitAttemptTransaltorTests
    {
        private GitHubCommitAttemptTranslator subject = new GitHubCommitAttemptTranslator();

        #region Pos

        [Test]
        public void CanProcess_is_true_for_valid_CommitAttempt_from_GitHub_docs()
        {
            const string attemptBody = @"{
  ""before"": ""5aef35982fb2d34e9d9d4502f6ede1072793222d"",
  ""repository"": {
    ""url"": ""http://github.com/defunkt/github"",
    ""name"": ""github"",
    ""description"": ""You're lookin' at it."",
    ""watchers"": 5,
    ""forks"": 2,
    ""private"": 1,
    ""owner"": {
      ""email"": ""chris@ozmm.org"",
      ""name"": ""defunkt""
    }
  },
  ""commits"": [
    {
      ""id"": ""41a212ee83ca127e3c8cf465891ab7216a705f59"",
      ""url"": ""http://github.com/defunkt/github/commit/41a212ee83ca127e3c8cf465891ab7216a705f59"",
      ""author"": {
        ""email"": ""chris@ozmm.org"",
        ""name"": ""Chris Wanstrath""
      },
      ""message"": ""okay i give in"",
      ""timestamp"": ""2008-02-15T14:57:17-08:00"",
      ""added"": [""filepath.rb""],
      ""removed"": [""deadfile.rb"", ""deadfile2.rb""],
      ""modified"": [""modfile.rb"", ""modfile2.rb""]
    },
    {
      ""id"": ""de8251ff97ee194a289832576287d6f8ad74e3d0"",
      ""url"": ""http://github.com/defunkt/github/commit/de8251ff97ee194a289832576287d6f8ad74e3d0"",
      ""author"": {
        ""email"": ""chris@ozmm.org"",
        ""name"": ""Chris Wanstrath""
      },
      ""message"": ""update pricing a tad"",
      ""timestamp"": ""2008-02-15T14:36:34-08:00""
    }
  ],
  ""after"": ""de8251ff97ee194a289832576287d6f8ad74e3d0"",
  ""ref"": ""refs/heads/master""
}
";
            var result = RunCanProcessTest(attemptBody);

            Assert.True(result);
        }

        [Test]
        public void CanProcess_is_true_for_valid_CommitAttempt_with_mixed_case()
        {
            const string attemptBody = @"{
  ""before"": ""5aef35982fb2d34e9d9d4502f6ede1072793222d"",
  ""repository"": {
    ""uRl"": ""HtTp://gItHUB.cOm/defunkt/github"",
    ""name"": ""github"",
    ""description"": ""You're lookin' at it."",
    ""watchers"": 5,
    ""forks"": 2,
    ""private"": 1,
    ""owner"": {
      ""email"": ""chris@ozmm.org"",
      ""name"": ""defunkt""
    }
  }
";
            var result = RunCanProcessTest(attemptBody);

            Assert.IsTrue(result);
        }

        [Test]
        public void CanProcess_is_true_for_valid_CommitAttempt_even_with_mixed_quotes_and_extra_spaces()
        {
            const string attemptBody = @"{
  ""before"": ""5aef35982fb2d34e9d9d4502f6ede1072793222d"",
  ""repository"": {
                            'uRl'  : 
                    ""HtTp://gItHUB.cOm/defunkt/github"",
    ""name"": ""github"",
    ""description"": ""You're lookin' at it."",
    ""watchers"": 5,
    ""forks"": 2,
    ""private"": 1,
    ""owner"": {
      ""email"": ""chris@ozmm.org"",
      ""name"": ""defunkt""
    }
  }
";
            var result = RunCanProcessTest(attemptBody);

            Assert.IsTrue(result);
        }


        #endregion

        #region Neg

        [Test]
        public void CanProcess_is_false_when_CommitAttempt_missing_repository()
        {
            const string attemptBody = @"{
  ""commits"": [
    {
      ""id"": ""41a212ee83ca127e3c8cf465891ab7216a705f59"",
      ""url"": ""http://github.com/defunkt/github/commit/41a212ee83ca127e3c8cf465891ab7216a705f59"",
      ""author"": {
        ""email"": ""chris@ozmm.org"",
        ""name"": ""Chris Wanstrath""
      },
      ""message"": ""okay i give in"",
      ""timestamp"": ""2008-02-15T14:57:17-08:00"",
      ""added"": [""filepath.rb""],
      ""removed"": [""deadfile.rb"", ""deadfile2.rb""],
      ""modified"": [""modfile.rb"", ""modfile2.rb""]
    },
    {
      ""id"": ""de8251ff97ee194a289832576287d6f8ad74e3d0"",
      ""url"": ""http://github.com/defunkt/github/commit/de8251ff97ee194a289832576287d6f8ad74e3d0"",
      ""author"": {
        ""email"": ""chris@ozmm.org"",
        ""name"": ""Chris Wanstrath""
      },
      ""message"": ""update pricing a tad"",
      ""timestamp"": ""2008-02-15T14:36:34-08:00""
    }
  ],
  ""after"": ""de8251ff97ee194a289832576287d6f8ad74e3d0"",
  ""ref"": ""refs/heads/master""
}
";
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
