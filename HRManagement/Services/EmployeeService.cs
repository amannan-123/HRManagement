using HRManagement.Models;
using System.Data;
using System.Data.SqlClient;

namespace HRManagement.Services
{
	public class EmployeeService : IEmployeesService
	{
		readonly string connectionString;

		//get config builder from DI
		public EmployeeService(IConfiguration config)
		{
			connectionString = config.GetConnectionString("DefaultConnection");
		}

		public bool AddEmployee(Employee employee)
		{
			SqlConnection conn = new(connectionString);
			conn.Open();

			//check if user alreay exists
			SqlCommand cmd = new("SELECT * FROM Employees WHERE [EmployeeCode] = @EmployeeCode", conn);
			cmd.Parameters.AddWithValue("@EmployeeCode", employee.EmployeeCode);
			SqlDataReader reader = cmd.ExecuteReader();
			bool _match = reader.HasRows;
			reader.Close();
			if (_match) return false;

			SqlCommand logger = new("AddEmployee", conn)
			{
				CommandType = CommandType.StoredProcedure
			};
			logger.Parameters.AddWithValue("@EmployeeCode", employee.EmployeeCode);
			logger.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
			logger.Parameters.AddWithValue("@AppointedBy", employee.AppointedBy);
			logger.Parameters.AddWithValue("@JoinDate", employee.JoinDate);
			logger.Parameters.AddWithValue("@Qualification", employee.Qualification);
			logger.Parameters.AddWithValue("@Gender", employee.Gender);
			logger.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
			logger.Parameters.AddWithValue("@Religion", employee.Religion);
			logger.Parameters.AddWithValue("@MaritalStatus", employee.MaritalStatus);
			logger.Parameters.AddWithValue("@Dependants", employee.Dependants);
			logger.Parameters.AddWithValue("@Email", employee.Email);
			logger.Parameters.AddWithValue("@PostBox", employee.PostBox);
			logger.Parameters.AddWithValue("@Country", employee.Country);
			logger.Parameters.AddWithValue("@City", employee.City);
			logger.Parameters.AddWithValue("@Landline", employee.Landline);
			logger.Parameters.AddWithValue("@Mobile", employee.Mobile);
			logger.Parameters.AddWithValue("@Address", employee.Address);
			logger.Parameters.AddWithValue("@Image", employee.Image);
			int _rows = logger.ExecuteNonQuery();

			conn.Close();

			return (_rows > 0);
		}

		public bool DeleteEmployee(int id)
		{
			//delete employee
			SqlConnection conn = new(connectionString);
			conn.Open();
			SqlCommand logger = new("DeleteEmployee", conn)
			{
				CommandType = CommandType.StoredProcedure
			};
			logger.Parameters.AddWithValue("@Id", id);
			int _rows = logger.ExecuteNonQuery();
			conn.Close();

			return (_rows > 0);
		}

		public List<Employee> GetAllEmployees()
		{
			//get all employees
			SqlConnection conn = new(connectionString);
			conn.Open();
			SqlCommand logger = new("GetAllEmployees", conn)
			{
				CommandType = CommandType.StoredProcedure
			};

			SqlDataReader reader = logger.ExecuteReader();
			List<Employee> employees = new();
			while (reader.Read())
			{
				Employee employee = new()
				{
					Id = Convert.ToInt32(reader["Id"]),
					EmployeeCode = reader["EmployeeCode"].ToString() ?? "",
					EmployeeName = reader["EmployeeName"].ToString() ?? "",
					AppointedBy = reader["AppointedBy"].ToString() ?? "",
					JoinDate = Convert.ToDateTime(reader["JoinDate"]),
					Qualification = reader["Qualification"].ToString() ?? "",
					Gender = reader["Gender"].ToString() ?? "",
					DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
					MaritalStatus = reader["MaritalStatus"].ToString() ?? "",
					Religion = reader["Religion"].ToString() ?? "",
					Dependants = Convert.ToInt32(reader["Dependants"]),
					Email = reader["Email"].ToString() ?? "",
					PostBox = reader["PostBox"].ToString() ?? "",
					Country = reader["Country"].ToString() ?? "",
					City = reader["City"].ToString() ?? "",
					Landline = Convert.ToInt32(reader["Landline"]),
					Mobile = Convert.ToInt32(reader["Mobile"].ToString()),
					Address = reader["Address"].ToString() ?? "",
					Image = (reader["Image"] != DBNull.Value) ? (byte[])reader["Image"] : null
				};
				employees.Add(employee);
			}

			conn.Close();
			return employees;
		}

		public Employee? GetEmployee(int id)
		{
			//get employee by id
			SqlConnection conn = new(connectionString);
			conn.Open();
			SqlCommand logger = new("GetEmployeeById", conn)
			{
				CommandType = CommandType.StoredProcedure
			};
			logger.Parameters.AddWithValue("@Id", id);
			SqlDataReader reader = logger.ExecuteReader();

			if (!reader.HasRows)
				return null;

			Employee employee = new();
			while (reader.Read())
			{
				employee = new()
				{
					Id = Convert.ToInt32(reader["Id"]),
					EmployeeCode = reader["EmployeeCode"].ToString() ?? "",
					EmployeeName = reader["EmployeeName"].ToString() ?? "",
					AppointedBy = reader["AppointedBy"].ToString() ?? "",
					JoinDate = Convert.ToDateTime(reader["JoinDate"]),
					Qualification = reader["Qualification"].ToString() ?? "",
					Gender = reader["Gender"].ToString() ?? "",
					DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
					MaritalStatus = reader["MaritalStatus"].ToString() ?? "",
					Religion = reader["Religion"].ToString() ?? "",
					Dependants = Convert.ToInt32(reader["Dependants"]),
					Email = reader["Email"].ToString() ?? "",
					PostBox = reader["PostBox"].ToString() ?? "",
					Country = reader["Country"].ToString() ?? "",
					City = reader["City"].ToString() ?? "",
					Landline = Convert.ToInt32(reader["Landline"]),
					Mobile = Convert.ToInt32(reader["Mobile"].ToString()),
					Address = reader["Address"].ToString() ?? "",
					Image = (reader["Image"] != DBNull.Value) ? (byte[])reader["Image"] : null
				};
			}

			conn.Close();
			return employee;
		}

		public List<Employee> GetEmployees(int pageNumber, int rowCount)
		{
			//get employees by page number and row count
			SqlConnection conn = new(connectionString);
			conn.Open();
			SqlCommand logger = new("GetEmployees", conn)
			{
				CommandType = CommandType.StoredProcedure
			};
			logger.Parameters.AddWithValue("@PageNumber", pageNumber);
			logger.Parameters.AddWithValue("@RowCount", rowCount);
			SqlDataReader reader = logger.ExecuteReader();
			List<Employee> employees = new();
			while (reader.Read())
			{
				Employee employee = new()
				{
					Id = Convert.ToInt32(reader["Id"]),
					EmployeeCode = reader["EmployeeCode"].ToString() ?? "",
					EmployeeName = reader["EmployeeName"].ToString() ?? "",
					AppointedBy = reader["AppointedBy"].ToString() ?? "",
					JoinDate = Convert.ToDateTime(reader["JoinDate"]),
					Qualification = reader["Qualification"].ToString() ?? "",
					Gender = reader["Gender"].ToString() ?? "",
					DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
					MaritalStatus = reader["MaritalStatus"].ToString() ?? "",
					Religion = reader["Religion"].ToString() ?? "",
					Dependants = Convert.ToInt32(reader["Dependants"]),
					Email = reader["Email"].ToString() ?? "",
					PostBox = reader["PostBox"].ToString() ?? "",
					Country = reader["Country"].ToString() ?? "",
					City = reader["City"].ToString() ?? "",
					Landline = Convert.ToInt32(reader["Landline"]),
					Mobile = Convert.ToInt32(reader["Mobile"].ToString()),
					Address = reader["Address"].ToString() ?? "",
					Image = (reader["Image"] != DBNull.Value) ? (byte[])reader["Image"] : null
				};
				employees.Add(employee);
			}

			conn.Close();
			return employees;
		}

		public int GetEmployeesCount()
		{
			//get employees count
			SqlConnection conn = new(connectionString);
			conn.Open();
			SqlCommand logger = new("GetEmployeesCount", conn)
			{
				CommandType = CommandType.StoredProcedure
			};
			int count = Convert.ToInt32(logger.ExecuteScalar());
			conn.Close();
			return count;
		}

		public bool UpdateEmployee(Employee employee)
		{
			//update employee
			SqlConnection conn = new(connectionString);
			conn.Open();
			SqlCommand logger = new("UpdateEmployee", conn)
			{
				CommandType = CommandType.StoredProcedure
			};
			logger.Parameters.AddWithValue("@Id", employee.Id);
			logger.Parameters.AddWithValue("@EmployeeCode", employee.EmployeeCode);
			logger.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
			logger.Parameters.AddWithValue("@AppointedBy", employee.AppointedBy);
			logger.Parameters.AddWithValue("@JoinDate", employee.JoinDate);
			logger.Parameters.AddWithValue("@Qualification", employee.Qualification);
			logger.Parameters.AddWithValue("@Gender", employee.Gender);
			logger.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
			logger.Parameters.AddWithValue("@Religion", employee.Religion);
			logger.Parameters.AddWithValue("@MaritalStatus", employee.MaritalStatus);
			logger.Parameters.AddWithValue("@Dependants", employee.Dependants);
			logger.Parameters.AddWithValue("@Email", employee.Email);
			logger.Parameters.AddWithValue("@PostBox", employee.PostBox);
			logger.Parameters.AddWithValue("@Country", employee.Country);
			logger.Parameters.AddWithValue("@City", employee.City);
			logger.Parameters.AddWithValue("@Landline", employee.Landline);
			logger.Parameters.AddWithValue("@Mobile", employee.Mobile);
			logger.Parameters.AddWithValue("@Address", employee.Address);
			logger.Parameters.AddWithValue("@Image", employee.Image);
			int _rows = logger.ExecuteNonQuery();

			conn.Close();

			return (_rows > 0);
		}
	}

	public interface IEmployeesService
	{
		bool AddEmployee(Employee employee);
		bool UpdateEmployee(Employee employee);
		bool DeleteEmployee(int id);
		List<Employee> GetAllEmployees();
		List<Employee> GetEmployees(int pageNumber, int rowCount);
		Employee? GetEmployee(int id);
		int GetEmployeesCount();
	}
}
