using DataAcess.Repository;
using Microsoft.Data.SqlClient;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class TodoService
    {
        public async Task<TodoMaster> createTodoMaster(TodoMaster todoMaster)
        {
            return await new GenericRepository<TodoMaster>().Insert(todoMaster);
        }
        public async Task<TodoMaster> getTodoMasterByID(int todoID)
        {
            return await new GenericRepository<TodoMaster>().FindOne(t=>t.Id == todoID);
        }
        public async Task<List<TodoMaster>> getTodoMasterList()
        {
            return await new GenericRepository<TodoMaster>().GetAll();
        }
        public async Task<List<TodoMaster>> getTodoMasterListByStatus(string status)
        {
            SqlParameter[] sqlParams = new SqlParameter[] { new SqlParameter("@TodoStatus", status) };
            return await new GenericRepository<TodoMaster>().FindUsingSPAsync("getTodoListByStatus @TodoStatus", sqlParams);
        }
    }
}
