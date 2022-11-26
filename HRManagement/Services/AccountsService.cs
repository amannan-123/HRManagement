using HRManagement.Models;
using System.Data.SqlClient;

namespace HRManagement.Services
{
	public class AccountsService : IAccountsService
	{
		readonly string connectionString;

		//get config builder from DI
		public AccountsService(IConfiguration config)
		{
			connectionString = config.GetConnectionString("DefaultConnection");
		}

		public bool AddEmployee(User employee)
		{
			SqlConnection conn = new(connectionString);
			conn.Open();

			//check if user alreay exists
			SqlCommand cmd = new("SELECT * FROM Users WHERE Email = @Email", conn);
			cmd.Parameters.AddWithValue("@Email", employee.Email);
			SqlDataReader reader = cmd.ExecuteReader();
			bool _match = reader.HasRows;
			reader.Close();
			if (_match) return false;

			//creating add query
			string query = $"INSERT INTO Users (FirstName, LastName, Email, Phone, Password, RoleId, DepartmentId, TimeStamp) VALUES(@first, @last, @email, @phone, @password, @role, @department, @time)";

			SqlCommand logger = new(query, conn);
			logger.Parameters.AddWithValue("@first", employee.FirstName);
			logger.Parameters.AddWithValue("@last", employee.LastName);
			logger.Parameters.AddWithValue("@email", employee.Email);
			logger.Parameters.AddWithValue("@phone", employee.Phone);
			logger.Parameters.AddWithValue("@password", employee.Password);
			logger.Parameters.AddWithValue("@role", employee.RoleId);
			logger.Parameters.AddWithValue("@department", employee.DepartmentId);
			logger.Parameters.AddWithValue("@time", employee.TimeStamp);

			bool res = logger.ExecuteNonQuery() > 0;

			conn.Close();

			return res ;
		}

		public bool DeleteEmployee(int id)
		{
			SqlConnection conn = new(connectionString);
			conn.Open();

			//creating delete query
			string query = $"DELETE FROM Users WHERE Id = @id";

			SqlCommand logger = new(query, conn);
			logger.Parameters.AddWithValue("@id", id);

			bool res = logger.ExecuteNonQuery() > 0;

			conn.Close();

			return res;
		}

		public bool EditEmployee(int id, User employee)
		{
			SqlConnection conn = new(connectionString);
			conn.Open();

			//creating edit query
			string query = $"UPDATE Users SET FirstName = @first, LastName = @last, Email = @email, Phone = @phone, Password = @password, RoleId = @role, DepartmentId = @department WHERE Id = @id";

			SqlCommand logger = new(query, conn);
			logger.Parameters.AddWithValue("@id", id);
			logger.Parameters.AddWithValue("@first", employee.FirstName);
			logger.Parameters.AddWithValue("@last", employee.LastName);
			logger.Parameters.AddWithValue("@email", employee.Email);
			logger.Parameters.AddWithValue("@phone", employee.Phone);
			logger.Parameters.AddWithValue("@password", employee.Password);
			logger.Parameters.AddWithValue("@role", employee.RoleId);
			logger.Parameters.AddWithValue("@department", employee.DepartmentId);

			bool res = logger.ExecuteNonQuery() > 0;

			conn.Close();

			return res;
		}

		public User? GetEmployee(string email, string pwd)
		{
			SqlConnection conn = new(connectionString);
			conn.Open();

			//creating add query
			string query = $"SELECT * FROM Users WHERE Email = @email AND Password = @password";

			SqlCommand logger = new(query, conn);
			logger.Parameters.AddWithValue("@email", email);
			logger.Parameters.AddWithValue("@password", pwd);

			SqlDataReader reader = logger.ExecuteReader();

			if (reader.HasRows)
			{
				reader.Read();
				User emp = new()
				{
					Id = Convert.ToInt32(reader["Id"]),
					FirstName = Convert.ToString(reader["FirstName"]),
					LastName = Convert.ToString(reader["LastName"]),
					Email = Convert.ToString(reader["Email"]),
					Phone = Convert.ToString(reader["Phone"]),
					Password = Convert.ToString(reader["Password"]),
					RoleId = Convert.ToInt32(reader["RoleId"]),
					DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
					TimeStamp = Convert.ToDateTime(reader["TimeStamp"])
				};
				reader.Close();

				conn.Close();
				return emp;
			}
			else
			{
				conn.Close();
				return null;
			}
		}
	}

	public interface IAccountsService
	{
		User? GetEmployee(string email, string pwd);
		bool AddEmployee(User employee);
		bool EditEmployee(int id, User employee);
		bool DeleteEmployee(int id);
	}
}
