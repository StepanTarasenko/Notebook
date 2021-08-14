using System;
using System.ComponentModel.DataAnnotations;

namespace Notebook.Models
{  
    [Serializable]
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Patronymic { get; set; }
        public Gender Gender { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
    
    public static class ContactExtension
    {
        public static void CopyState(this Contact copyTo, Contact copyFrom)
        {
            copyTo.Name = copyFrom.Name;
            copyTo.SurName = copyFrom.SurName;
            copyTo.Patronymic = copyFrom.Patronymic;
            copyTo.Gender = copyFrom.Gender;
            copyTo.Email = copyFrom.Email;
            copyTo.PhoneNumber = copyFrom.PhoneNumber;
            copyTo.DateOfBirth = copyFrom.DateOfBirth;
        }
    }
}