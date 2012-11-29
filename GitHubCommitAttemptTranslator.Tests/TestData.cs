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

        public static readonly string FullyFormedAttemptBodySingleQuotes =
@"
{
  'before': '5aef35982fb2d34e9d9d4502f6ede1072793222d',
  'repository': {
    'url': 'http://github.com/defunkt/github',
    'name': 'github',
    'description': 'You\'re lookin\' at it.',
    'watchers': 5,
    'forks': 2,
    'private': 1,
    'owner': {
      'email': 'chris@ozmm.org',
      'name': 'defunkt'
    }
  },
  'commits': [
    {
      'id': '41a212ee83ca127e3c8cf465891ab7216a705f59',
      'url': 'http://github.com/defunkt/github/commit/41a212ee83ca127e3c8cf465891ab7216a705f59',
      'author': {
        'email': 'dev@null.org',
        'name': 'The Null Developer'
      },
      'message': 'okay i give in',
      'timestamp': '2008-02-15T14:57:17-08:00',
      'added': ['filepath.rb'],
      'removed': ['deadfile.rb', 'deadfile2.rb'],
      'modified': ['modfile.rb', 'modfile2.rb']
    },
    {
      'id': 'de8251ff97ee194a289832576287d6f8ad74e3d0',
      'url': 'http://github.com/defunkt/github/commit/de8251ff97ee194a289832576287d6f8ad74e3d0',
      'author': {
        'email': 'chris@ozmm.org',
        'name': 'Chris Wanstrath'
      },
      'message': 'update pricing a tad',
      'timestamp': '2008-02-15T14:36:34-08:00'
    }
  ],
  'after': 'de8251ff97ee194a289832576287d6f8ad74e3d0',
  'ref': 'refs/heads/master'
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

        public static readonly string GitHubNov28 =
@"payload=%7B%22pusher%22%3A%7B%22name%22%3A%22JogoShugh%22%2C%22email%22%3A%22jsgough%40gmail.com%22%7D%2C%22repository%22%3A%7B%22name%22%3A%22Test%22%2C%22size%22%3A112%2C%22created_at%22%3A%222012-08-14T21%3A38%3A40-07%3A00%22%2C%22has_wiki%22%3Atrue%2C%22private%22%3Afalse%2C%22watchers%22%3A0%2C%22language%22%3A%22C%23%22%2C%22fork%22%3Atrue%2C%22url%22%3A%22https%3A%2F%2Fgithub.com%2FJogoShugh%2FTest%22%2C%22pushed_at%22%3A%222012-11-28T13%3A35%3A35-08%3A00%22%2C%22id%22%3A5422086%2C%22has_downloads%22%3Atrue%2C%22open_issues%22%3A0%2C%22has_issues%22%3Afalse%2C%22stargazers%22%3A0%2C%22description%22%3A%22Test%22%2C%22forks%22%3A0%2C%22owner%22%3A%7B%22name%22%3A%22JogoShugh%22%2C%22email%22%3A%22jsgough%40gmail.com%22%7D%7D%2C%22forced%22%3Afalse%2C%22after%22%3A%22284b101ec4691b9a96704cb4139ea0b6e9b25653%22%2C%22head_commit%22%3A%7B%22modified%22%3A%5B%22README.md%22%5D%2C%22added%22%3A%5B%5D%2C%22timestamp%22%3A%222012-11-28T13%3A35%3A34-08%3A00%22%2C%22removed%22%3A%5B%5D%2C%22author%22%3A%7B%22name%22%3A%22Josh%20Gough%22%2C%22username%22%3A%22JogoShugh%22%2C%22email%22%3A%22jsgough%40gmail.com%22%7D%2C%22url%22%3A%22https%3A%2F%2Fgithub.com%2FJogoShugh%2FTest%2Fcommit%2F284b101ec4691b9a96704cb4139ea0b6e9b25653%22%2C%22id%22%3A%22284b101ec4691b9a96704cb4139ea0b6e9b25653%22%2C%22distinct%22%3Atrue%2C%22message%22%3A%22Just%20a%20test%22%2C%22committer%22%3A%7B%22name%22%3A%22Josh%20Gough%22%2C%22username%22%3A%22JogoShugh%22%2C%22email%22%3A%22jsgough%40gmail.com%22%7D%7D%2C%22deleted%22%3Afalse%2C%22ref%22%3A%22refs%2Fheads%2Fmaster%22%2C%22commits%22%3A%5B%7B%22modified%22%3A%5B%22README.md%22%5D%2C%22added%22%3A%5B%5D%2C%22timestamp%22%3A%222012-11-28T13%3A35%3A34-08%3A00%22%2C%22removed%22%3A%5B%5D%2C%22author%22%3A%7B%22name%22%3A%22Josh%20Gough%22%2C%22username%22%3A%22JogoShugh%22%2C%22email%22%3A%22jsgough%40gmail.com%22%7D%2C%22url%22%3A%22https%3A%2F%2Fgithub.com%2FJogoShugh%2FTest%2Fcommit%2F284b101ec4691b9a96704cb4139ea0b6e9b25653%22%2C%22id%22%3A%22284b101ec4691b9a96704cb4139ea0b6e9b25653%22%2C%22distinct%22%3Atrue%2C%22message%22%3A%22Just%20a%20test%22%2C%22committer%22%3A%7B%22name%22%3A%22Josh%20Gough%22%2C%22username%22%3A%22JogoShugh%22%2C%22email%22%3A%22jsgough%40gmail.com%22%7D%7D%5D%2C%22compare%22%3A%22https%3A%2F%2Fgithub.com%2FJogoShugh%2FTest%2Fcompare%2F7c66dba89fac...284b101ec469%22%2C%22before%22%3A%227c66dba89fac11ffee6f976d8856d272b6dd109c%22%2C%22created%22%3Afalse%7D";
    }
}
