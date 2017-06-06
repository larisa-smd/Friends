using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace FriendsQuiz.Models
{
    [Serializable]
    [XmlRoot("Question"), XmlType("Question")]
    public class Question
    {
        public int id { get; set; }
        public string question { get; set; }
        public int Score { get; set; }
        public string rightAnswer { get; set; }
        public string[] Answers { get; set; }


    }
}