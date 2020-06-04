using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management.Models
{
    public class Message
    {
        public string Title { get; set; }
        public string Note { get; set; }

        public Message()
        {

        }
        public Message(string title, string message)
        {
            this.Title = title;
            this.Note = message;
        }
    }
}
