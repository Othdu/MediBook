using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediBook.Application.Interfaces;
using MediBook.Domain.Entities;
using MediBook.Infrastructure.Data; 

namespace MediBook.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MediBookDbContext _db;
        public UserRepository(MediBookDbContext db) => _db = db;

        public User? GetByEmail(string email) => _db.Users.FirstOrDefault(u => u.Email == email);
        public void Add(User user)=> _db.Users.Add(user);
        public void SaveChanges() => _db.SaveChanges();

    }
}
