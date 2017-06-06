using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FriendsQuiz.Models
{
    class XMLReader
    {

        public List<Question> ListOfQuestions()
        {
            string xmlData = HttpContext.Current.Server.MapPath("~/App_Data/QuizData.xml");
            DataSet ds = new DataSet();
            ds.ReadXml(xmlData);
            var questions = new List<Question>();
           
                questions = (from rows in ds.Tables[0].AsEnumerable()
                             select new Question
                             {
                                 id = Convert.ToInt32(rows[0].ToString()),
                                 question = rows[1].ToString(),
                                 Score = Convert.ToInt32(rows[2].ToString()),
                                 rightAnswer = rows[3].ToString(),
                                 Answers = rows[4].ToString().Split(',')
                             }).ToList();
           
            return questions;
        }


        public List<UsersScore> ListOfUsers()
        {
            string xmlData = HttpContext.Current.Server.MapPath("~/App_Data/Scores.xml");
            DataSet ds = new DataSet();
            ds.ReadXml(xmlData);
            var users = new List<UsersScore>();
            if (ds.Tables.Count > 0)
            {
                users = (from rows in ds.Tables[0].AsEnumerable()
                         select new UsersScore
                         {
                             Name = rows[0].ToString(),
                             Score = Convert.ToInt32(rows[1].ToString()),
                             date = Convert.ToDateTime(rows[2].ToString())
                         }).ToList();
            }
            else
            {
                var user = new UsersScore
                {
                    Name = "First user",
                    Score = 10,
                    date = DateTime.Now
                };
                users.Add(user);
            }
            return users;
        }

    }
}