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
        private string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=notebookdb;Trusted_Connection=True;";
        private SqlConnection _connection;
        private static MSSqlDataBaseConnection _instance;
        public static MSSqlDataBaseConnection Instance => _instance;
        static MSSqlDataBaseConnection()
        {
            _instance = new MSSqlDataBaseConnection();
        }

        private MSSqlDataBaseConnection()
        {
            _connection = new SqlConnection(_connectionString);
            
        }

        public void CreateTable()
        {
            try
            {
                _connection.Open();
                SqlCommand create = new SqlCommand();
                create.CommandText = "CREATE TABLE Contacts" +  //IF NOT EXIST don't work
                    "(Id INT PRIMARY KEY IDENTITY," + 
                    "name NVARCHAR(100),"+
                    "surname NVARCHAR(100),"+
                    "patronymic NVARCHAR(100)," +
                    "email NVARCHAR(20)," +
                    "phone NVARCHAR(15)," +
                    "date_of_birth DATE," +
                    "gender varchar(10) CHECK (Gender IN('Male', 'Female')))"; 
                create.Connection = _connection;
                create.ExecuteNonQuery();
            }
            catch(SqlException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                _connection.Close();
            }
        }
        public void DropTable()
        {
            try
            {
                _connection.Open();
                SqlCommand drop = new SqlCommand();
                drop.CommandText = "DROP TABLE Contacts";
                drop.Connection = _connection;
                drop.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                throw new Exception("Tabel don't exist");
            }
            finally
            {
                _connection.Close();
            }
        }

        public List<Contact> GetContacts()
        {
            List<Contact> result = new List<Contact>();
            try
            {
                _connection.Open();
                string sqlExpression = "SELECT * FROM Contacts";
                SqlCommand command = new SqlCommand(sqlExpression, _connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows == false)
                {
                    return new List<Contact>();
                }

                while (reader.Read())
                {
                    Contact contact = new Contact
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Name = reader["name"].ToString(),
                        SurName = reader["surname"].ToString(),
                        Patronymic = reader["patronymic"].ToString(),
                        Email = reader["email"].ToString(),
                        PhoneNumber = reader["phone"].ToString(),
                        Gender = ParseToGender(reader["gender"].ToString()),
                        DateOfBirth = DateTime.Parse(reader["date_of_birth"].ToString())
                    };
                    result.Add(contact);
                }
            }
            catch(Exception e)
            {

            }
            finally
            {
                _connection.Close();
            }

            return result;
        }

        public int AddContact(Contact newContact)
        {
            try
            {
                _connection.Open();
                string sqlExpression = "INSERT INTO Contacts (name, surname, patronymic, email, phone, date_of_birth, gender) VALUES (@name, @surname, @patronymic, @email, @phone, @date_of_birth, @gender);SET @id=SCOPE_IDENTITY()";
                SqlCommand command = new SqlCommand(sqlExpression, _connection);

                prepareComand(command, newContact);

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
            catch (Exception e)
            {

            }
            finally
            {
                _connection.Close();
            }
            return -1;
        }

public void DeleteContact(int contactId)
        {
            try {
                _connection.Open();
                string sqlExpression = "DELETE FROM Contacts WHERE Contacts.id = @id";
                SqlCommand command = new SqlCommand(sqlExpression, _connection);

                SqlParameter idParam = new SqlParameter("@id", contactId);
                command.Parameters.Add(idParam);

                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {

            }
            finally
            {
                _connection.Close();
            }
        }

        public void UpdateContact(Contact renewContact)
        {
            try
            {
                _connection.Open();
                string sqlExpression = "UPDATE Contacts " +
                    "SET name = @name, surname = @surname, patronymic = @patronymic, email = @email, phone = @phone, date_of_birth = @date_of_birth, gender = @gender " +
                    "WHERE Contacts.id = @id";
                SqlCommand command = new SqlCommand(sqlExpression, _connection);

                prepareComand(command, renewContact);

                SqlParameter idParam = new SqlParameter("@id", renewContact.Id);
                command.Parameters.Add(idParam);

                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {

            }
            finally
            {
                _connection.Close();
            }
        }

            private void prepareComand(SqlCommand command, Contact contact)
        {
            SqlParameter NameParam = new SqlParameter("@name", contact.Name);
            command.Parameters.Add(NameParam);

            SqlParameter SurNameParam = new SqlParameter("@surname", contact.SurName);
            command.Parameters.Add(SurNameParam);

            SqlParameter PatronymicParam = new SqlParameter("@patronymic", contact.Patronymic);
            command.Parameters.Add(PatronymicParam);

            SqlParameter EmailParam = new SqlParameter("@email", contact.Email);
            command.Parameters.Add(EmailParam);

            SqlParameter PhoneNumberParam = new SqlParameter("@phone", contact.PhoneNumber);
            command.Parameters.Add(PhoneNumberParam);

            SqlParameter DateOfBirthParam = new SqlParameter("@date_of_birth", contact.DateOfBirth);
            command.Parameters.Add(DateOfBirthParam);

            SqlParameter GenderParam = new SqlParameter("@gender", contact.Gender.ToString());
            command.Parameters.Add(GenderParam);
        }

        private Gender ParseToGender(string gender)
        {
            return (Gender)Enum.Parse(typeof(Gender), gender);
        }

    }
}