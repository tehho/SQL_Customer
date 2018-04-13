using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SQL_CRM.ConsoleClasses;

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

            Assert.AreEqual(3, question.PossibleAnswers.Count);
            Assert.AreEqual("Tester", question.PossibleAnswers[0].answer);
            Assert.AreEqual("Testsson", question.PossibleAnswers[1].answer);
            Assert.AreEqual("Another Test", question.PossibleAnswers[2].answer);
        }

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
