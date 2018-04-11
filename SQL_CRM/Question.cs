using System;
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

        public string Check(string input)
        {
            for (int i = 0; i < answers.Count; i++)
            {
                if (input == (i + 1).ToString())
                    return answers[i].answer;
                if (answers[i].Check(input))
                    return answers[i].answer;
            }

            throw new InvalidOperationException("Answer does not exist");
        }
    }
}