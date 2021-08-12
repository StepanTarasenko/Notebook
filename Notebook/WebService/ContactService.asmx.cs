using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        public void Post(int id, string name)
        {
            var maxId = ContactDto.Get().Max(x => x.Id) + 1;
            var employee = new ContactDto { Id = maxId, Name = name };
            ContactDto.Contacts.Add(employee);
            HttpContext.Current.Response.Write(maxId);
        }

        [WebMethod]
        public void Delete(int id)
        {
            var employee = ContactDto.Contacts.FirstOrDefault(x => x.Id == id);
            ContactDto.Contacts.Remove(employee);
        }

        [WebMethod]
        public void Put(int id, string name)
        {
            var employee = ContactDto.Contacts.FirstOrDefault(x => x.Id == id);

            employee.Name = name;
        }


    }

    public class ContactDto
    {
        public static IList<ContactDto> Contacts { get; set; }

        public IList<ContactDto> Get()
        {
            Contacts = Contacts ?? new List<ContactDto>
            {
                new ContactDto {Id=1,Name="anson" },
                new ContactDto {Id=2,Name="jacky" }
            };
            return Contacts;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
