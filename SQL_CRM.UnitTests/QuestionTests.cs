using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SQL_CRM.UnitTests
{
    [TestClass]
    public class QuestionTests
    {
        Question question = new Question("Test", "Tester,Test", "Testsson", "Another Test");

        [TestMethod]
        public void TestConstructor_ShouldSucceed()
        {
            Assert.AreEqual("Test", question.question);

            Assert.AreEqual(3, question.answers.Count);
            Assert.AreEqual("Tester", question.answers[0].answer);
            Assert.AreEqual("Testsson", question.answers[1].answer);
            Assert.AreEqual("Another Test", question.answers[2].answer);
        }


        //TODO Rewrite, method now returns correct string, if not throws exception.
        [TestMethod]
        public void TestCheck_InputIsInAnswers_ReturnTrue()
        {
            Assert.AreEqual("Tester", question.Check("Test"));
            Assert.AreEqual("Testsson", question.Check("Testsson"));
            Assert.AreEqual("Another Test", question.Check("Another Test"));
        }

        [TestMethod]
        public void TestCheck_InputIsNotInAnswers_ReturnFalse()
        {
            Assert.ThrowsException<InvalidOperationException>(() => question.Check("Hello"));
            Assert.ThrowsException<InvalidOperationException>(() => question.Check(" "));
            Assert.ThrowsException<InvalidOperationException>(() => question.Check(","));
        }
    }
}
