using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopmeProject.Models;
using System;
using System.Linq;
using System.Transactions;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        private MyDbContext _context;

        [TestInitialize]
        public void Initialize()
        {
            _context = new MyDbContext();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Dispose();
        }

        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            var newRole = new Role
            {
                Name = "Nhan",
                Discription = "Manage everything"
            };

            // Act
            using (var scope = new TransactionScope())
            {
                _context.Roles.Add(newRole);
                _context.SaveChanges();
                scope.Complete();
            }
         

            // Assert
            var savedRole = _context.Roles.FirstOrDefault(r => r.Id == newRole.Id);
            Assert.IsNotNull(savedRole);
            Assert.AreEqual(newRole.Name, savedRole.Name);
            Assert.AreEqual(newRole.Discription, savedRole.Discription);
        }
    }
}
