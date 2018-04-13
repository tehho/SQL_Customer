using System;
using System.Collections.Generic;
using System.Linq;

namespace SQL_CRM.ConsoleClasses
{
    public class Question
    {
        public string question;
        public List<Answer> PossibleAnswers;

        public Question(string question, params string[] possibleAnswers)
        {
            this.question = question;
            PossibleAnswers = possibleAnswers.Select(item => new Answer(item)).ToList();
        }

        public string Check(string input)
        {
            for (int i = 0; i < PossibleAnswers.Count; i++)
            {
                if (input == (i + 1).ToString())
                    return PossibleAnswers[i].answer;
                if (PossibleAnswers[i].Check(input))
                    return PossibleAnswers[i].answer;
            }

            throw new InvalidOperationException("Answer does not exist");
        }
    }
}