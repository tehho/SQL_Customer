using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SQL_CRM.UnitTests
{
    [TestClass]
    public class QuestionTests
    {
        Question question = new Question("Test", "Tester", "Testsson", "Another Test");

        [TestMethod]
        public void TestConstructor_ShouldSucceed()
        {
            Assert.AreEqual("Test", question.question);

            Assert.AreEqual(3, question.answers.Count);
            Assert.AreEqual("Tester", question.answers[0].answer);
            Assert.AreEqual("Testsson", question.answers[1].answer);
            Assert.AreEqual("Another Test", question.answers[2].answer);
        }

        [TestMethod]
        public void TestCheck_InputIsInAnswers_ReturnTrue()
        {
            Assert.AreEqual(true, question.Check("Tester"));
            Assert.AreEqual(true, question.Check("Testsson"));
            Assert.AreEqual(true, question.Check("Another Test"));
        }

        [TestMethod]
        public void TestCheck_InputIsNotInAnswers_ReturnFalse()
        {
            Assert.AreEqual(false, question.Check("Hello"));
            Assert.AreEqual(false, question.Check("Should Fail"));
            Assert.AreEqual(false, question.Check(""));
        }
    }
}
