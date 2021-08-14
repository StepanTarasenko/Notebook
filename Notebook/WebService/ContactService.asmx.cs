using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Notebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Notebook.DataAccess;

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

        MSSqlDataBaseConnection dataBaseConnection = MSSqlDataBaseConnection.Instance;

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
            var json = JsonConvert.SerializeObject(dataBaseConnection.GetContacts(), ConvertToCamelCase());
            HttpContext.Current.Response.Write(json);
        }

        [WebMethod]
        public void Post(Contact newContact)
        {
            var Id = dataBaseConnection.AddContact(newContact);
            HttpContext.Current.Response.Write(Id);
        }

        [WebMethod]
        public void Delete(int id)
        {
            dataBaseConnection.DeleteContact(id);
        }

        [WebMethod]
        public void Put(Contact renewContact)
        {
            dataBaseConnection.UpdateContact(renewContact);
        }


    }

}
