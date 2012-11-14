using System;
using System.Collections.Generic;
using CommitService.Contract;

namespace GitHubCommitAttemptTranslator.Tests
{
    public static class TestData
    {
        public static readonly string FullyFormedAttemptBody =
@"{
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
        ""email"": ""dev@null.org"",
        ""name"": ""The Null Developer""
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

        public static readonly string ThreeValidCommitsFragment =
@"
{
    'commits': 
    [
        {
          'author': {
            'email': 'dev@null.org',
            'name': 'The Null Developer'
          },
          'message': 'okay i give in',
          'timestamp': '2010-02-15T14:00:00-08:00',
        },
        {
          'author': {
            'email': 'author@github.com',
            'name': 'Doc U. Mentation'
          },
          'message': 'Updating the docs, that\'s my job',
          'timestamp': '2011-02-15T14:00:00-08:00'
        },
        {
          'author': {
            'email': 'author@github.com',
            'name': 'Doc U. Mentation'
          },
          'message': 'Oops, typos',
          'timestamp': '2012-02-15T14:00:00-08:00'
        }
    ]
}
";

        public static readonly CommitMessage ExpectedValidCommitMessage1 = new CommitMessage
        {
            Author = "The Null Developer <dev@null.org>",
            Comment = "okay i give in",
            Date = DateTime.Parse("2010-02-15T14:00:00-08:00"),
            SourceCommit =
@"{
  ""author"": {
    ""email"": ""dev@null.org"",
    ""name"": ""The Null Developer""
  },
  ""message"": ""okay i give in"",
  ""timestamp"": ""2010-02-15T17:00:00-05:00""
}"
        };

        public static readonly CommitMessage ExpectedValidCommitMessage2 = new CommitMessage
        {
            Author = "Doc U. Mentation <author@github.com>",
            Comment = "Updating the docs, that's my job",
            Date = DateTime.Parse("2011-02-15T14:00:00-08:00"),
            SourceCommit =
@"{
  ""author"": {
    ""email"": ""author@github.com"",
    ""name"": ""Doc U. Mentation""
  },
  ""message"": ""Updating the docs, that's my job"",
  ""timestamp"": ""2011-02-15T17:00:00-05:00""
}"
        };

        public static readonly CommitMessage ExpectedValidCommitMessage3 = new CommitMessage
        {
            Author = "Doc U. Mentation <author@github.com>",
            Comment = "Oops, typos",
            Date = DateTime.Parse("2012-02-15T14:00:00-08:00"),
            SourceCommit =
@"{
  ""author"": {
    ""email"": ""author@github.com"",
    ""name"": ""Doc U. Mentation""
  },
  ""message"": ""Oops, typos"",
  ""timestamp"": ""2012-02-15T17:00:00-05:00""
}"

        };

        public static readonly IEnumerable<CommitMessage> ExpectedValidCommitMessages = new List<CommitMessage>(
            new[] { ExpectedValidCommitMessage1, ExpectedValidCommitMessage2, ExpectedValidCommitMessage3 });
    }
}
