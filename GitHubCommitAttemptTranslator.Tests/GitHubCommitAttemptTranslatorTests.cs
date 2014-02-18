using System.Linq;
using CommitService.Contract;
using NUnit.Framework;

namespace GitHubCommitAttemptTranslator.Tests
{
    [TestFixture]
    public class GitHubCommitAttemptTranslatorTests
    {
        private readonly GitHubCommitAttemptTranslator subject = new GitHubCommitAttemptTranslator();

        #region Pos

        #region CanProcess

        [Test]
        public void CanProcess_is_true_for_valid_CommitAttempt_from_GitHub_docs()
        {
            var result = RunCanProcessTest(TestData.FullyFormedAttemptBody);

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

        #region Execute

        [Test]
        public void Execute_succeeds_for_valid_CommitAttempt()
        {
            var commitAttempt = new CommitAttempt() { RawBody = TestData.FullyFormedAttemptBody };
            var result = subject.Execute(commitAttempt);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(2, result.Commits.Count);
        }

        [Test]
        public void Execute_creates_three_valid_CommitMessages_from_single_CommitAttempt_with_three_commits()
        {
            var attempt = new CommitAttempt() {RawBody = TestData.ThreeValidCommitsFragment};

            var result = subject.Execute(attempt);

            Assert.IsTrue(result.Success);
            Assert.True(IsCommitValid(TestData.ExpectedValidCommitMessage1, result.Commits[0]));
            Assert.True(IsCommitValid(TestData.ExpectedValidCommitMessage2, result.Commits[1]));
            Assert.True(IsCommitValid(TestData.ExpectedValidCommitMessage3, result.Commits[2]));
        }

        private bool IsCommitValid(CommitMessage expected, CommitMessage actual)
        {
            var valid = false;

            // Don't nix my crazy separation. It's for debugging:
            valid = expected.Author == actual.Author;
            valid &= expected.Comment == actual.Comment;
            valid &= expected.SourceCommit == actual.SourceCommit;
            valid &= expected.Date == actual.Date;

            return valid;
        }

        [Test]
        public void GitHubNewFormat()
        {
            var attempt = new CommitAttempt() {RawBody = TestData.GitHubNov28};
            var result = subject.CanProcess(attempt);
        }

        #endregion

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
            var attemptMessage = new CommitAttempt { RawBody = attemptBody };

            var result = subject.CanProcess(attemptMessage);

            return result;
        }

        #endregion
    }
}