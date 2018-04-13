using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SQL_CRM.ConsoleClasses
{
    public class Answer
    {
        public string answer => PossibleAnswers[0];

        public List<string> PossibleAnswers;

        public Answer(string possibleAnswers)
        {
            var list = Regex.Split(possibleAnswers, "('[^']+')|([^,']+),");
            
            this.PossibleAnswers = list.Where(item => !string.IsNullOrEmpty(item.Trim())).Select(item => item.Trim().Trim("'".ToCharArray())).ToList();
        }

        public bool Check(string input)
        {
            foreach (var possibleAnswer in PossibleAnswers)
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