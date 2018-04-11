using System.Collections.Generic;
using System.Linq;

namespace SQL_CRM
{
    public class Question
    {
        public string question;
        public List<Answer> answers;

        public Question(string question, params string[] possibleAnswers)
        {
            this.question = question;
            answers = possibleAnswers.Select(item => new Answer(item)).ToList();
        }

        public bool Check(string input)
        {
            foreach (var answer in answers)
            {
                if (answer.Check(input))
                    return true;
            }

            return false;
        }
    }
}