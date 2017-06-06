using FriendsQuiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace FriendsQuiz.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        
        Random rdn = new Random();

        static List<Question> questions;
        static List<int> listOfPastQuestions;
        static UsersScore u;
        static int currentImage;

        [HttpPost]
        public ActionResult QuizStart(string user)
        {
            if (String.IsNullOrEmpty(user))
            {
                user = "Guest";
            }

            listOfPastQuestions = new List<int>(); ;
            XMLReader readXml;
           u = new UsersScore();
            u.Name = user;
            u.Score = 0;
            u.date = DateTime.Now.Date;
            u.currentQuestion = 0;
            readXml = new XMLReader();

            List<Question> data = readXml.ListOfQuestions();
            int n = data.Count();

            questions = new List<Question>();
            while (listOfPastQuestions.Count() < 10)
            {
                var aux = rdn.Next(n);

                while (listOfPastQuestions.Contains(aux))
                {
                    aux = rdn.Next(n);
                }

                listOfPastQuestions.Add(aux);

                Question q = data.ElementAt(aux);

                AddQuestion(q);
            }

            ViewBag.currentQuestion = u.currentQuestion+1;
            ViewBag.currentScore = u.Score.ToString();
            currentImage = rdn.Next(1, 15);
            ViewBag.image = currentImage.ToString();

            ViewBag.user = u.Name;
          

            return View(questions[u.currentQuestion]);
        }

        public void AddQuestion(Question q)
        {

            var a = rdn.Next(4);
            var ex = q.Answers;
            q.Answers = new string[4];
            int k = 0;
            for (int j = 0; j < 4; j++)
                if (j == a)
                    q.Answers[j] = q.rightAnswer;
                else
                {
                    q.Answers[j] = ex[k];
                    k++;
                }

            questions.Add(q);

        }

        [HttpPost]
        public ViewResult NextQuestion(string answer)
        {
            
            ViewBag.classAnswer = "wrong";
            ViewBag.answer = "noAnswer";
            if (!String.IsNullOrEmpty(answer))
            {
                ViewBag.answer = answer;
                if ((questions[u.currentQuestion].rightAnswer == answer) && (u.currentQuestion < 10))
                {
                    u.Score += questions[u.currentQuestion].Score;
                    ViewBag.classAnswer = "correct";
                }                      
            }

            ViewBag.user = u.Name;
            ViewBag.currentScore = u.Score;
            ViewBag.currentQuestion = u.currentQuestion + 1;
            ViewBag.image = currentImage.ToString();          
            return View(questions[u.currentQuestion]);
        }

        [HttpPost]
        public ViewResult FinishQuestion()
        {
            u.currentQuestion++;
            ViewBag.user = u.Name;
            ViewBag.currentScore = u.Score;
            ViewBag.currentQuestion = u.currentQuestion + 1;
            currentImage = rdn.Next(1, 15);
            ViewBag.image = currentImage.ToString();
            if (u.currentQuestion >= 10)
            {

                var ms = DateTime.Now.Hour - u.date.Hour;
                ViewBag.time = ms;
                XmlDocument xmlDoc = new XmlDocument();
                string path = Server.MapPath("../App_Data/Scores.xml");
                xmlDoc.Load(path);

                XmlNode node = xmlDoc.CreateNode(XmlNodeType.Element, "User", null);
                XmlNode nodeName = xmlDoc.CreateElement("Name");
                nodeName.InnerText = u.Name;
                XmlNode nodeScore = xmlDoc.CreateElement("Score");
                nodeScore.InnerText = u.Score.ToString();
                XmlNode nodeDate = xmlDoc.CreateElement("Date");
                nodeDate.InnerText = u.date.Date.ToString();

                node.AppendChild(nodeName);
                node.AppendChild(nodeScore);
                node.AppendChild(nodeDate);

                xmlDoc.DocumentElement.AppendChild(node);

                xmlDoc.Save(path);

                return View("QuizFinalScore", u);
            }
            else
            {
                return View("QuizStart", questions[u.currentQuestion]);
            }
            
        }

        public ViewResult HighScores()
        {
            XMLReader x = new XMLReader();
            List<UsersScore> users = x.ListOfUsers();
            List<UsersScore> SortedUsers = users.OrderByDescending(o => o.Score).Take(10).ToList();
            return View(SortedUsers);
        }


        [HttpPost]
        public ViewResult QuizFinalScore()
        {
            return View();
        }
    }
}