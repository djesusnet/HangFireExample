using System.Data;
using Dapper;
using HangFireJobsExamples.Repositories.Interfaces;

namespace HangFireJobsExamples.Repositories;

public class UserRepository(IDbConnection dbConnection) : IUserRepository
{
    public void UserInsert(string name, string email)
    {
       var sql = "INSERT INTO Usuarios (Nome, Email) VALUES (@Nome, @Email);";
       dbConnection.Execute(sql, new { Nome = name, Email = email });
    }
}