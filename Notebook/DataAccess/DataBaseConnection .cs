using Notebook.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Notebook.DataAccess
{
    public class MSSqlDataBaseConnection
    {
        private static string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=notebookdb;Trusted_Connection=True;";
        private static SqlConnection _connection;
        static MSSqlDataBaseConnection()
        {
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
        }

        public void CreateTable()
        {
            try
            {
                SqlCommand create = new SqlCommand();
                create.CommandText = "CREATE TABLE Contacts" +  //IF NOT EXIST don't work  , Age INT NOT NULL
                    "(Id INT PRIMARY KEY IDENTITY," + 
                    "Name NVARCHAR(100),"+
                    "SurName NVARCHAR(100),"+
                    "Patronymic NVARCHAR(100)," +
                    "Email NVARCHAR(20)," +
                    "PhoneNumber NVARCHAR(15)," +
                    "DateOfBirth DATE," +
                    "Gender varchar(10) CHECK (Gender IN('Male', 'Female')))"; 
                create.Connection = _connection;
                create.ExecuteNonQuery();
            }catch(SqlException e)
            {
                throw new Exception("Tabel already exist");
            }
        }
        public void DropTable()
        {
            try
            {
                SqlCommand drop = new SqlCommand();
                drop.CommandText = "DROP TABLE Contacts";
                drop.Connection = _connection;
                drop.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                throw new Exception("Tabel don't exist");
            }
        }
        public void Close()
        {
            _connection.Close();
        }

        public List<Contact> GetContacts()
        {
            string sqlExpression = "SELECT * FROM Contacts";
            SqlCommand command = new SqlCommand(sqlExpression, _connection);
            SqlDataReader reader = command.ExecuteReader();
            List<Contact> result = new List<Contact>();
            if (reader.HasRows == false)
            {
                return new List<Contact>();
            }

            while (reader.Read())
            {
                Contact contact = new Contact { Id = Convert.ToInt32(reader["id"]), Name = reader["name"].ToString() };
                result.Add(contact);
            }

            return result;
        }

        public int AddContact(Contact newContact)
        {
            string sqlExpression = "INSERT INTO Contacts (name) VALUES (@name);SET @id=SCOPE_IDENTITY()";
            SqlCommand command = new SqlCommand(sqlExpression, _connection);

            SqlParameter nameParam = new SqlParameter("@name", newContact.Name);
            command.Parameters.Add(nameParam);

            SqlParameter idParam = new SqlParameter
            {
                ParameterName = "@id",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(idParam);

            command.ExecuteNonQuery();

            return Convert.ToInt32(idParam.Value);           
        }

        public void DeleteContact(int contactId)
        {
            string sqlExpression = "DELETE FROM Contacts WHERE Contacts.id = @id";
            SqlCommand command = new SqlCommand(sqlExpression, _connection);

            SqlParameter idParam = new SqlParameter("@id", contactId);
            command.Parameters.Add(idParam);

            command.ExecuteNonQuery();
        }

        public void UpdateContact(Contact newContact)
        {
            string sqlExpression = "UPDATE Contacts SET name = @name WHERE Contacts.id = @id";
            SqlCommand command = new SqlCommand(sqlExpression, _connection);

            SqlParameter nameParam = new SqlParameter("@name", newContact.Name);
            command.Parameters.Add(nameParam);

            SqlParameter idParam = new SqlParameter("@id", newContact.Id);
            command.Parameters.Add(idParam);

            command.ExecuteNonQuery();
        }

    }
}