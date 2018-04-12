using System.Collections.Generic;
using System.Linq;

namespace SQL_CRM
{
    public class Answer
    {
        public string answer => possibleAnswers[0];

        public List<string> possibleAnswers;

        public Answer(string possibleAnswers)
        {
            this.possibleAnswers = possibleAnswers.Split(',').Where(item => !string.IsNullOrEmpty(item.Trim())).ToList();
        }

        public bool Check(string input)
        {
            foreach (var possibleAnswer in possibleAnswers)
            {
                if (possibleAnswer == input)
                {
                    return true;
                }
            }

            return false;
        }
    }
}