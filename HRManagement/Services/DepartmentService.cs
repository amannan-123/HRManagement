using HRManagement.Models;
using System.Data.SqlClient;
using System.Data;

namespace HRManagement.Services
{
	public class DepartmentService : IDepartmentService
	{
		readonly string connectionString;

		public DepartmentService(IConfiguration config)
		{
			connectionString = config.GetConnectionString("DefaultConnection");
		}

		public bool AddDepartment(Department des)
		{
			SqlConnection conn = new(connectionString);
			conn.Open();

			//check if code alreay exists
			SqlCommand cmd = new("SELECT * FROM Departments WHERE [DepartmentCode] = @DepartmentCode", conn);
			cmd.Parameters.AddWithValue("@DepartmentCode", des.Code);
			SqlDataReader reader = cmd.ExecuteReader();
			bool _match = reader.HasRows;
			reader.Close();
			if (_match) return false;

			SqlCommand logger = new("AddDepartment", conn)
			{
				CommandType = CommandType.StoredProcedure
			};
			logger.Parameters.AddWithValue("@DepartmentCode", des.Code);
			logger.Parameters.AddWithValue("@DepartmentName", des.Name);
			int _rows = logger.ExecuteNonQuery();

			conn.Close();

			return (_rows > 0);
		}

		public bool DeleteDepartment(int id)
		{
			//delete Department
			SqlConnection conn = new(connectionString);
			conn.Open();
			SqlCommand logger = new("DeleteDepartment", conn)
			{
				CommandType = CommandType.StoredProcedure
			};
			logger.Parameters.AddWithValue("@Id", id);
			int _rows = logger.ExecuteNonQuery();
			conn.Close();

			return (_rows > 0);
		}

		public bool UpdateDepartment(Department des)
		{
			//update Department
			SqlConnection conn = new(connectionString);
			conn.Open();

			//check if code alreay exists
			SqlCommand cmd = new("SELECT * FROM Departments WHERE [DepartmentCode] = @DepartmentCode AND NOT [Id] = @Id", conn);
			cmd.Parameters.AddWithValue("@DepartmentCode", des.Code);
			cmd.Parameters.AddWithValue("@Id", des.Id);
			SqlDataReader reader = cmd.ExecuteReader();
			bool _match = reader.HasRows;
			reader.Close();
			if (_match) return false;

			SqlCommand logger = new("UpdateDepartment", conn)
			{
				CommandType = CommandType.StoredProcedure
			};
			logger.Parameters.AddWithValue("@Id", des.Id);
			logger.Parameters.AddWithValue("@DepartmentCode", des.Code);
			logger.Parameters.AddWithValue("@DepartmentName", des.Name);
			int _rows = logger.ExecuteNonQuery();

			conn.Close();

			return (_rows > 0);
		}

		public List<Department> GetDepartments()
		{
			//get all Departments
			SqlConnection conn = new(connectionString);
			conn.Open();
			SqlCommand logger = new("GetDepartments", conn)
			{
				CommandType = CommandType.StoredProcedure
			};

			SqlDataReader reader = logger.ExecuteReader();
			List<Department> dess = new();
			while (reader.Read())
			{
				Department des = new()
				{
					Id = Convert.ToInt32(reader["Id"]),
					Code = reader["DepartmentCode"].ToString() ?? "",
					Name = reader["DepartmentName"].ToString() ?? ""
				};
				dess.Add(des);
			}

			conn.Close();
			return dess;
		}

		public Department? GetDepartment(int id)
		{
			//get Department by id
			SqlConnection conn = new(connectionString);
			conn.Open();
			SqlCommand logger = new("GetDepartmentById", conn)
			{
				CommandType = CommandType.StoredProcedure
			};
			logger.Parameters.AddWithValue("@Id", id);
			SqlDataReader reader = logger.ExecuteReader();

			if (!reader.HasRows)
				return null;

			Department des = new();
			while (reader.Read())
			{
				des = new()
				{
					Id = Convert.ToInt32(reader["Id"]),
					Code = reader["DepartmentCode"].ToString() ?? "",
					Name = reader["DepartmentName"].ToString() ?? "",
				};
			}

			conn.Close();
			return des;
		}
	}

	public interface IDepartmentService
	{
		List<Department> GetDepartments();
		bool AddDepartment(Department des);
		bool UpdateDepartment(Department des);
		bool DeleteDepartment(int id);
		Department? GetDepartment(int id);
	}
}
