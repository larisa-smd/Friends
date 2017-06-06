using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace FriendsQuiz.Models
{
    [Serializable]
    [XmlRoot("Users"), XmlType("UsersScore")]
    public class UsersScore
    {

        public string Name { get; set; }
        public int Score { get; set; }
        public int currentQuestion { get; set; }
        public DateTime date { get; set; }
    }

}