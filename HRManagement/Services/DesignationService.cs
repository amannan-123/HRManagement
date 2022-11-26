using HRManagement.Models;
using System.Data.SqlClient;
using System.Data;

namespace HRManagement.Services
{
	public class DesignationService : IDesignationService
	{
		readonly string connectionString;

		public DesignationService(IConfiguration config)
		{
			connectionString = config.GetConnectionString("DefaultConnection");
		}

		public bool AddDesignation(Designation des)
		{
			SqlConnection conn = new(connectionString);
			conn.Open();

			//check if code alreay exists
			SqlCommand cmd = new("SELECT * FROM Designations WHERE [DesignationCode] = @DesignationCode", conn);
			cmd.Parameters.AddWithValue("@DesignationCode", des.Code);
			SqlDataReader reader = cmd.ExecuteReader();
			bool _match = reader.HasRows;
			reader.Close();
			if (_match) return false;

			SqlCommand logger = new("AddDesignation", conn)
			{
				CommandType = CommandType.StoredProcedure
			};
			logger.Parameters.AddWithValue("@DesignationCode", des.Code);
			logger.Parameters.AddWithValue("@DesignationName", des.Name);
			int _rows = logger.ExecuteNonQuery();

			conn.Close();

			return (_rows > 0);
		}

		public bool DeleteDesignation(int id)
		{
			//delete designation
			SqlConnection conn = new(connectionString);
			conn.Open();
			SqlCommand logger = new("DeleteDesignation", conn)
			{
				CommandType = CommandType.StoredProcedure
			};
			logger.Parameters.AddWithValue("@Id", id);
			int _rows = logger.ExecuteNonQuery();
			conn.Close();

			return (_rows > 0);
		}

		public bool UpdateDesignation(Designation des)
		{
			//update designation
			SqlConnection conn = new(connectionString);
			conn.Open();
			
			//check if code alreay exists
			SqlCommand cmd = new("SELECT * FROM Designations WHERE [DesignationCode] = @DesignationCode AND NOT [Id] = @Id", conn);
			cmd.Parameters.AddWithValue("@DesignationCode", des.Code);
			cmd.Parameters.AddWithValue("@Id", des.Id);
			SqlDataReader reader = cmd.ExecuteReader();
			bool _match = reader.HasRows;
			reader.Close();
			if (_match) return false;

			SqlCommand logger = new("UpdateDesignation", conn)
			{
				CommandType = CommandType.StoredProcedure
			};
			logger.Parameters.AddWithValue("@Id", des.Id);
			logger.Parameters.AddWithValue("@DesignationCode", des.Code);
			logger.Parameters.AddWithValue("@DesignationName", des.Name);
			int _rows = logger.ExecuteNonQuery();

			conn.Close();

			return (_rows > 0);
		}

		public List<Designation> GetDesignations()
		{
			//get all designations
			SqlConnection conn = new(connectionString);
			conn.Open();
			SqlCommand logger = new("GetDesignations", conn)
			{
				CommandType = CommandType.StoredProcedure
			};
			
			SqlDataReader reader = logger.ExecuteReader();
			List<Designation> dess = new();
			while (reader.Read())
			{
				Designation des = new()
				{
					Id = Convert.ToInt32(reader["Id"]),
					Code = reader["DesignationCode"].ToString() ?? "",
					Name = reader["DesignationName"].ToString() ?? ""
				};
				dess.Add(des);
			}
			
			conn.Close();
			return dess;
		}

		public Designation? GetDesignation(int id)
		{
			//get designation by id
			SqlConnection conn = new(connectionString);
			conn.Open();
			SqlCommand logger = new("GetDesignationById", conn)
			{
				CommandType = CommandType.StoredProcedure
			};
			logger.Parameters.AddWithValue("@Id", id);
			SqlDataReader reader = logger.ExecuteReader();

			if (!reader.HasRows)
				return null;

			Designation des = new();
			while (reader.Read())
			{
				des = new()
				{
					Id = Convert.ToInt32(reader["Id"]),
					Code = reader["DesignationCode"].ToString() ?? "",
					Name = reader["DesignationName"].ToString() ?? "",
				};
			}

			conn.Close();
			return des;
		}
	}

	public interface IDesignationService
	{
		List<Designation> GetDesignations();
		bool AddDesignation(Designation des);
		bool UpdateDesignation(Designation des);
		bool DeleteDesignation(int id);
		Designation? GetDesignation(int id);
	}
}
