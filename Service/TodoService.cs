using AutoMapper;
using DataAcess.Repository;
using DTO;
using Microsoft.Data.SqlClient;
using Model.Models;
namespace Service
{
    public class TodoService
    {
        //private Mapper _TotoMasterMapper;
        public TodoService()
        {
           // var _MapConfig = new MapperConfiguration(cfg => cfg.CreateMap<TodoMasterDTO, TodoMaster>().ReverseMap());
           // _TotoMasterMapper = new Mapper(_MapConfig);
        }
        public async Task<TodoMaster> createTodoMaster(TodoMasterDTO todoMaster)
        {
            TodoMaster master = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<TodoMasterDTO, TodoMaster>())).Map<TodoMasterDTO, TodoMaster>(todoMaster);
            
            return await new GenericRepository<TodoMaster>().Insert(master);
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
