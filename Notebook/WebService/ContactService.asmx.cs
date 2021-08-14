using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Notebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace Notebook.WebService
{
    /// <summary>
    /// Сводное описание для ContactService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Чтобы разрешить вызывать веб-службу из скрипта с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
    [System.Web.Script.Services.ScriptService]
    public class ContactService1 : System.Web.Services.WebService
    {
        ContactDto ContactDto = new ContactDto();

        private JsonSerializerSettings ConvertToCamelCase()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }
        
        [WebMethod]
        public void Get()
        {
            HttpContext.Current.Response.ContentType = "application/json;charset=utf-8";
            var json = JsonConvert.SerializeObject(ContactDto.Get(), ConvertToCamelCase());
            HttpContext.Current.Response.Write(json);
        }

        [WebMethod]
        public void Post(Contact newContact)
        {
            var maxId = ContactDto.Get().Max(x => x.Id) + 1;
            var contact = new Contact();
            contact.CopyState(newContact);
            contact.Id = maxId;
            ContactDto.Contacts.Add(contact);
            HttpContext.Current.Response.Write(maxId);
        }

        [WebMethod]
        public void Delete(int id)
        {
            var contact = ContactDto.Contacts.FirstOrDefault(x => x.Id == id);
            ContactDto.Contacts.Remove(contact);
        }

        [WebMethod]
        public void Put(Contact renewContact)
        {
            var contact = ContactDto.Contacts.FirstOrDefault(x => x.Id == renewContact.Id);

            contact.CopyState(renewContact);
        }


    }

    public class ContactDto
    {
        public static IList<Contact> Contacts { get; set; }

        public IList<Contact> Get()
        {
            Contacts = Contacts ?? new List<Contact>
            {
                new Contact {Id=1, Name="anson", SurName="svit", Patronymic="jacerson", Email="anson12@gmail.com", PhoneNumber="+79232134286", DateOfBirth = DateTime.Now, Gender = Gender.Male },
                new Contact {Id=2, Name="jacky", SurName="abromson", Patronymic="deluck", Email="jacky42@gmail.com", PhoneNumber="+79242134644", DateOfBirth = DateTime.Now, Gender = Gender.Male }
            };
            return Contacts;
        }
    }
}
