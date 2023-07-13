using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace ShopmeProject.Services
{
    public class StateService
    {
        private MyDbContext _context;

        public StateService()
        {
            _context = new MyDbContext();
        }

        public List<State> listAll(int Id)
        {
            return _context.States.Where(x => x.CountryId == Id).OrderBy(c => c.Name).ToList();
        }


        public State Save(State state)
        {

            if (state.Id == 0)
            {
                _context.States.Add(state);
            }
            else
            {
                State stateInDb = _context.States.Find(state.Id);
                stateInDb.Name = state.Name;
                stateInDb.CountryId = state.CountryId;
            }
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
            return state;
        }

        public string delete(int id)
        {
            State stateToDelete = _context.States.Find(id);
            if (stateToDelete != null)
            {
                _context.States.Remove(stateToDelete);
                _context.SaveChanges();
                return "Delete successfull";
            }
            return "Detete false";
        }

        public bool isNameUnique(string name)
        {
            State stateByName = _context.Set<State>().SingleOrDefault(u => u.Name == name);
            if (stateByName == null) return true;


            return false;
        }

    }
}