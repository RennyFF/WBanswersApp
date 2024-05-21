using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WBNEWANSWEARS.MVVM.Model
{
    public class UsersStructure
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string TokenContent { get; set; }
        public string TokenFeedBack { get; set; }
        public string Preset { get; set; }
        public List<AnswersStructure> Answers { get; set; }

        public UsersStructure(int id, string userName, string tokenContent, string tokenFeedBack, string preset, List<AnswersStructure> answers)
        {
            Id = id;
            UserName = userName;
            TokenFeedBack = tokenFeedBack;
            TokenContent = tokenContent;
            Preset = preset;
            Answers = new List<AnswersStructure>();
        }

        public UsersStructure()
        {
            
        }
    }

    public class AnswersStructure
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Priority { get; set; }
        public bool IsUsed { get; set; }
        public string TargetRating { get; set; }
        public string Text { get; set; }
        public int UserId { get; set; }
        public AnswersStructure(string title, int priority, bool isUsed, string targetRating, string text, int userId)
        {
            Title = title;
            Priority = priority;
            IsUsed = isUsed;
            TargetRating = targetRating;
            Text = text;
            UserId = userId;
        }

        public AnswersStructure()
        {

        }
    }
    public class NMList
    {
        public List<Card> cards { get; set; }
        public Cursor cursor { get; set; }
    }

    public class Cursor
    {
        public string updatedAt { get; set; }
        public long nmID { get; set; }
        public int total { get; set; }
}

    public class Card
    {
        public long nmID { get; set; }
    }

    public class FeedBackData
    {
        public FeedBackList data { get; set; }
    }
    public class FeedBackList
    {
        public List<FeedBack> feedbacks { get; set; }
    }
    public class FeedBack
    {
        public string id { get; set; }
        public int productValuation { get; set; }
        public string text { get; set; }
    }
    public class AnswerTemplate
    {
        public string id { get; set; }
        public string text { get; set; }
    }

    public class FeedBackCountData
    {
        public FeedBackCount data { get; set; }
    }
    public class FeedBackCount
    {
        public int countUnanswered { get; set; }
    }
}
