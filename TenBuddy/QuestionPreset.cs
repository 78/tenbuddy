using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenBuddy
{
    public enum QuestionType
    {
        QT_FRIEND,
        QT_GROUP,
        QT_INPUT,
    };

    public class Question
    {
        public QuestionType Type;
        public string Text = "";
    }

    public class QuestionPreset
    {
        List<Question> Questions;
        Dictionary<QuestionType, string> DefaultQuestions;

        public QuestionPreset()
        {
            Questions = new List<Question>();
            DefaultQuestions = new Dictionary<QuestionType, string>();
        }

        public void AddQuestion(QuestionType type, string text)
        {
            var q = new Question();
            q.Type = type;
            q.Text = text;
            Questions.Add(q);
            if (!DefaultQuestions.ContainsKey(type))
            {
                DefaultQuestions[type] = text;
            }
        }

        public List<string> GetQuestions(QuestionType type)
        {
            var result = new List<string>();
            foreach (Question q in Questions)
            {
                if (q.Type == type)
                {
                    result.Add(q.Text);
                }
            }
            return result;
        }

        public void SetDefaultQuestion(QuestionType type, string text)
        {
            DefaultQuestions[type] = text;
        }

        public string GetDefaultQuestion(QuestionType type)
        {
            return DefaultQuestions[type];
        }
    }
}
