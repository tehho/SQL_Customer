using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SQL_CRM.UnitTests
{
    [TestClass]
    public class AnswerTests
    {
        Answer answer = new Answer("Test,Testsson,Another Test");

        [TestMethod]
        public void TestConstructor_ShouldSucceed()
        {
            Assert.AreEqual(3, answer.possibleAnswers.Count);
            Assert.AreEqual("Test", answer.possibleAnswers[0]);
            Assert.AreEqual("Testsson", answer.possibleAnswers[1]);
            Assert.AreEqual("Another Test", answer.possibleAnswers[2]);
        }

        [TestMethod]
        public void TestConstructorEdge_ShouldSucceed()
        {
            var emptyAnswer = new Answer(" ");

            Assert.AreEqual(0, emptyAnswer.possibleAnswers.Count);
        }

        [TestMethod]
        public void TestCheck_InputIsInAnswers_ReturnTrue()
        {
            Assert.AreEqual(true, answer.Check("Test"));
            Assert.AreEqual(true, answer.Check("Testsson"));
            Assert.AreEqual(true, answer.Check("Another Test"));
        }

        [TestMethod]
        public void TestCheck_InputIsNotInAnswers_ReturnFalse()
        {
            Assert.AreEqual(false, answer.Check("Hej"));
            Assert.AreEqual(false, answer.Check(" Test "));
        }
    }
}
