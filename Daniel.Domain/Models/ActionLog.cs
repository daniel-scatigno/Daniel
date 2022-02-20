using Daniel.Domain.Lib.Models;
using System;

namespace Daniel.Domain.Models
{
    public class  ActionLog : BaseModel
    {
        public int IdUser  { get; set; }
        public string Login{get;set;}
        public int IdRecord  { get; set; }
        public ActionType Action { get; set; }

        public string Change{get;set;}
        public string ObjectType { get; set; }

        public DateTime Date { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public object Entity{get;set;}
        
    }
}