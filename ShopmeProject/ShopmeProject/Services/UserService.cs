using Microsoft.AspNet.Identity;
using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace ShopmeProject.Services
{
    public class UserService
    {
        public const int USERS_PER_PAGE = 4;
        private MyDbContext _context;

        public UserService()
        {
            _context = new MyDbContext();
        }

        public IEnumerable<User> ListAll()
        {
            return _context.Users
         .Include("Roles")
         .OrderBy(u => u.FirstName);
        }
        public IEnumerable<Role> ListRoles()
        {
            return _context.Roles;
        }
        public void save(User user, List<Role> roles)
        {
            var passwordHasher = new PasswordHasher();
            var existingUser = _context.Users.Find(user.Id);

            if (existingUser != null)
            {
                // Update existing user
                existingUser.Email = user.Email;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Roles.Clear();
                if(user.Photos != null)
                {
                    existingUser.Photos = user.Photos;
                }
                foreach (var role in roles)
                {
                    var roleInDatabase = _context.Roles.Find(role.Id);
                    existingUser.Roles.Add(roleInDatabase);
                }

                existingUser.Enabled = user.Enabled;

                if (!string.IsNullOrEmpty(user.Password))
                {
                    // Hash and salt the password
                    var hashedPassword = passwordHasher.HashPassword(user.Password);
                    existingUser.Password = hashedPassword;
                }
            }
            else
            {
                // Create new user
                var hashedPassword = passwordHasher.HashPassword(user.Password);
                user.Password = hashedPassword;

                foreach (var role in roles)
                {
                    var roleInDatabase = _context.Roles.Find(role.Id);
                    user.Roles.Add(roleInDatabase);
                }

                _context.Users.Add(user);
            }

            _context.SaveChanges();
        }
        public void saveDetails(User user)
        {
            var passwordHasher = new PasswordHasher();
            var existingUser = _context.Users.Find(user.Id);

            if (existingUser != null)
            {
                // Update existing user
                existingUser.Email = user.Email;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
               
                if (user.Photos != null)
                {
                    existingUser.Photos = user.Photos;
                }
             

               

                if (!string.IsNullOrEmpty(user.Password))
                {
                    // Hash and salt the password
                    var hashedPassword = passwordHasher.HashPassword(user.Password);
                    existingUser.Password = hashedPassword;
                }
            }
            else
            {
                // Create new user
                var hashedPassword = passwordHasher.HashPassword(user.Password);
                user.Password = hashedPassword;

             

                _context.Users.Add(user);
            }

            _context.SaveChanges();
        }
        public bool isEmailUnique(int id, string email)
        {
            User userByEmail = _context.Set<User>().SingleOrDefault(u => u.Email == email);
            if (userByEmail == null) return true;
            bool isCreatingNew;
            if (id == 0)
            {
                isCreatingNew = true;
            }
            else
            {
                isCreatingNew = false;
            }


            if (isCreatingNew)
            {
                if (userByEmail != null) return false;
            }
            else
            {
                if (userByEmail.Id != id) return false;
            }

            return true;
        }
        public User Get(int id)
        {
            return _context.Users.Find(id);
        }
        public void delete(int id)
        {
            User userToDelete = _context.Users.Find(id);
            if (userToDelete != null)
            {
                _context.Users.Remove(userToDelete);
                _context.SaveChanges();
            }

        }
        public int userPerPage()
        {
            return USERS_PER_PAGE;
        }
        public int CountUsersById(int id)
        {
            return _context.Users.Count(u => u.Id == id);
        }
        public void updateUserEnabledStatus(int id, bool enabled)
        {
            var existingUser = _context.Users.Find(id);
            if (existingUser != null)
            {
                existingUser.Enabled = enabled;
                try
                {
                    _context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);
                    var fullErrorMessage = string.Join("; ", errorMessages);
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }
            }
        }

        public int totalPageUser(string keyword)
        {
            int totalRecords = totalRecordUser(keyword); // get the total number of records in the table
            int totalPages = (int)Math.Ceiling((double)totalRecords / USERS_PER_PAGE); // calculate the total number of pages
            return totalPages;
        }

        public int totalRecordUser(string keyword)
        {
            int totalRecords;
            if (!string.IsNullOrEmpty(keyword))
            {
               totalRecords = _context.Users
                   .Where(x =>
                    x.FirstName.Contains(keyword) ||
                    x.LastName.Contains(keyword) ||
                    x.Email.Contains(keyword)).Count();
            }
            else
            {
                totalRecords = _context.Users.Count();
            }
            return totalRecords;    
         }

        public IEnumerable<User> listByPage(int pageNum, string keyword)
        {
            var query = _context.Users
           .OrderBy(x => x.Id) 
           .Skip((pageNum - 1) * USERS_PER_PAGE) 
           .Take(USERS_PER_PAGE);

            if (!string.IsNullOrEmpty(keyword))
            {
                query = _context.Users
                    .OrderBy(x => x.Id)
                   .Where(x =>
                    x.FirstName.Contains(keyword) ||
                    x.LastName.Contains(keyword) ||
                    x.Email.Contains(keyword))
                   .Skip((pageNum - 1) * USERS_PER_PAGE)
                   .Take(USERS_PER_PAGE)
                   ;
            }
           
            var records = query.ToList(); 
            return records;
        }

        public bool validInfor(string email, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == email && u.Enabled == true);
            if (user == null)
            {
                return false;
            }

            var passwordVerificationResult = Crypto.VerifyHashedPassword(user.Password, password);
            return passwordVerificationResult;
        }

        public User getUserByEmail(string email)
        {
            var user = _context.Users.Include("Roles").FirstOrDefault(u => u.Email == email);
            return user;
        }

    }
}