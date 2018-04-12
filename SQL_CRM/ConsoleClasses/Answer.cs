using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SQL_CRM
{
    public class Answer
    {
        public string answer => possibleAnswers[0];

        public List<string> possibleAnswers;

        public Answer(string possibleAnswers)
        {
            var list = Regex.Split(possibleAnswers, "('[^']+')|([^,']+),");
            
            this.possibleAnswers = list.Where(item => !string.IsNullOrEmpty(item.Trim())).Select(item => item.Trim().Trim("'".ToCharArray())).ToList();
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