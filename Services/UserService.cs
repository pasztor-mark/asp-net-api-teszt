using api_teszt.Database;
using api_teszt.Dtos;
using api_teszt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace api_teszt.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetUsers();
    Task<User> GetUserByEmail(string email);
    Task<User> CreateUser(CreateUserDto user);
    Task<User> UpdateUser(UpdateUserDto user, string email);
    Task<bool> DeleteUser(string email);
}

public class UserService : IUserService
{
    private readonly DatabaseContext _db;

    public UserService(DatabaseContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        return await _db.users.ToListAsync();
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _db.users.Where(u => u.email == email).FirstOrDefaultAsync();
    }


    public async Task<User> CreateUser(CreateUserDto user)
    {
        var created = new User
        {
            bio = user.bio,
            first_name = user.first_name,
            last_name = user.last_name,
            email = user.email,
        };
        var added = await _db.users.AddAsync(created);
        await _db.SaveChangesAsync();
        Console.WriteLine(added.Entity.id);
        return added.Entity;
    }

    public async Task<User> UpdateUser(UpdateUserDto user, string email)
    {
        var userToUpdate = await _db.users.Where(u => u.email == email).FirstOrDefaultAsync();
        if (userToUpdate == null)
        {
            return null;
        }

        if (!user.bio.IsNullOrEmpty())
        {
            userToUpdate.bio = user.bio;
        }
        if (!user.first_name.IsNullOrEmpty())
        {
            userToUpdate.first_name = user.first_name;
        }
        if (!user.last_name.IsNullOrEmpty())
        {
            userToUpdate.last_name = user.last_name;
        }
        if (!user.email.IsNullOrEmpty())
        {
            userToUpdate.email = user.email;
        }

        var upd = _db.users.Update(userToUpdate);
        await _db.SaveChangesAsync();
        return upd.Entity;
    }

    public async Task<bool> DeleteUser(string email)
    {
        var toDelete = await _db.users.Where(u => u.email == email).FirstOrDefaultAsync();
        if (toDelete == null)
        {
            return false;
        }
        _db.users.Remove(toDelete);
        await _db.SaveChangesAsync();
        return true;
    }
}