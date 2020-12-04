using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using Roommates.Models;


namespace Roommates.Repositories
{
    class RoommateRepository: BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }
        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Roommate AS rm " +
                                      "LEFT JOIN Room AS r ON rm.RoomId=r.Id " +
                                      "WHERE rm.Id=@id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    Roommate roommate = null;
                    if (reader.Read())
                    {
                        Room roomToAdd = new Room
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("RoomId")),
                            Name=reader.GetString(reader.GetOrdinal("Name")),
                            MaxOccupancy=reader.GetInt32(reader.GetOrdinal("MaxOccupancy"))
                        };
                        roommate = new Roommate
                        {
                            Id = id,
                            Firstname = reader.GetString(reader.GetOrdinal("FirstName")),
                            Lastname = reader.GetString(reader.GetOrdinal("LastName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            MovedInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                            Room = roomToAdd
                        };
                    }
                    reader.Close();
                    return roommate;
                }
            }
        }
        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "Select * FROM Roommate";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Roommate> Roommates = new List<Roommate>();
                    while (reader.Read())
                    {
                        RoomRepository roomrepo = new RoomRepository(@"server=localhost\SQLExpress;database=Roommates;integrated security=true");
                        Roommate roommate = new Roommate
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Firstname = reader.GetString(reader.GetOrdinal("FirstName")),
                            Lastname = reader.GetString(reader.GetOrdinal("LastName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            MovedInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                            Room = roomrepo.GetById(reader.GetInt32(reader.GetOrdinal("RoomId")))
                        };
                        Roommates.Add(roommate);
                    }
                    reader.Close();
                    return Roommates;
                }
            }
        }

    }
}
