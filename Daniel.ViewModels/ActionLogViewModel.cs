using Daniel.ViewModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace Daniel.ViewModels
{
    public class  ActionLogViewModel : DanielViewModel
    {
        public int IdUser  { get; set; }
        public string Login{get;set;}
        public int IdRecord  { get; set; }
        public string Action { get; set; }

        public string Change{get;set;}
        public string ObjectType { get; set; }

        public DateTime Date { get; set; }
        
    }
}