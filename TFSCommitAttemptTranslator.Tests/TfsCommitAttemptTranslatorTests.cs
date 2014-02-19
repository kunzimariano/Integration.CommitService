using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using ApprovalTests;
using ApprovalTests.Reporters;
using ApprovalUtilities.Utilities;
using CommitService.Contract;
using NUnit.Framework;

namespace TFSCommitAttemptTranslator.Tests
{
	[TestFixture]
	public class TfsCommitAttemptTranslatorTests
	{
		private readonly TfsCommitAttemptTranslator _translator = new TfsCommitAttemptTranslator();
		private CommitAttempt _validAttempt;
		private CommitAttempt _invalidAttempt;


		[SetUp]
		public void SetUp()
		{
			var validSampleData = File.ReadAllText("ValidSample.xml");
			_validAttempt = new CommitAttempt()
			{
				RawBody = validSampleData,
				UserAgent = "Team Foundation (TfsJobAgent.exe, 10.0.40219.1)"
			};

			var inValidSampleData = File.ReadAllText("InValidSample.xml");
			_invalidAttempt = new CommitAttempt()
			{
				RawBody = inValidSampleData,
				UserAgent = "Team Foundation (TfsJobAgent.exe, 10.0.40219.1)"
			};
		}

		[Test]
		public void CanProcess_is_true_for_valid_CommitAttempt_from_TFS_request()
		{
			bool canProcess = _translator.CanProcess(_validAttempt);
			Assert.True(canProcess);
		}

		[Test]
		public void CanProcess_is_false_for_invalid_UserAgent()
		{
			_invalidAttempt.UserAgent = "nonsense";
			bool canProcess = _translator.CanProcess(_invalidAttempt);
			Assert.False(canProcess);
		}

		[Test]
		public void Execute_succeeds_for_valid_CommitAttempt()
		{
			var result = _translator.Execute(_validAttempt);

			Assert.IsTrue(result.Success);
			Assert.AreEqual(1, result.Commits.Count);
		}

		[Test]
		[ExpectedException("TFSCommitAttemptTranslator.InvalidTfsAttemptException")]
		public void Expect_InvalidTfsAttemptException_on_non_parsable_input()
		{
			var result = _translator.Execute(_invalidAttempt);

			Assert.IsFalse(result.Success);
			Assert.AreEqual(1, result.Commits.Count);
		}


		[Test]
		[UseReporter(typeof(DiffReporter))]
		public void Execute_Result_Matches_Expectation()
		{
			TranslateCommitAttemptResult result = _translator.Execute(_validAttempt);
			CommitMessage cm = result.Commits[0];
			Approvals.Verify(StringUtils.WritePropertiesToString(cm));
		}
	}
}
