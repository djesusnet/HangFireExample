namespace HangFireJobsExamples.Repositories.Interfaces;

public interface IUserRepository
{
    void UserInsert(string name, string email);
}