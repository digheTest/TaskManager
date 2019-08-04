using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TaskMangerReference;
using TaskReference;
using TMWebAPI.Models;

namespace TMWebAPI.Controllers
{
    public class ValuesController : ApiController
    {
        ModelFactory _modelFactory;

        public ValuesController()
        {
            _modelFactory = new ModelFactory();
        }
        public ValuesController(ModelFactory modelFactory)
        {
            _modelFactory = modelFactory;
        }
        [HttpGet]
        public IEnumerable<ParentTaskModel> Get()
        {
            TaskManagerRepository parentTaskRep = new TaskManagerRepository();
            return parentTaskRep.GetAllParentTasksRepo().ToList().Select(p => _modelFactory.GetParentTaskMoDel(p));
        }
        [HttpGet]
        public ParentTaskModel GetParentTask(int ParentID)
        {
            TaskManagerRepository parentTaskRep = new TaskManagerRepository();
            return _modelFactory.GetParentTaskMoDel(parentTaskRep.GetParentTaskRepo(ParentID));
        }
        [HttpGet]
        public IEnumerable<TaskModel> GetAllTask()
        {
            TaskManagerRepository parentTaskRep = new TaskManagerRepository();
            return parentTaskRep.GetAllTaskRepo().ToList().Select(t => _modelFactory.GetTaskModel(t));
        }
        [HttpGet]
        public TaskModel GetTask(int TaskID)
        {
            TaskManagerRepository parentTaskRep = new TaskManagerRepository();
            return _modelFactory.GetTaskModel(parentTaskRep.GetTaskRepo(TaskID));
        }
        [HttpPost]
        public IHttpActionResult CreateParentTask([FromBody]ParentTaskModel ParentTask)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid data");
            try
            {
                TaskManagerRepository parentTaskRep = new TaskManagerRepository();
                Parent_Task_Tbl ParentTaskDb = new Parent_Task_Tbl
                {
                    Parent_Task = ParentTask.ParentTaskName,
                };
                string result = "{'ParentTaskID': " + parentTaskRep.CreateParentTask(ParentTaskDb) + "}";
                JObject json = JObject.Parse(result);
                return Ok<JObject>(json);

            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred in CreateParentTask :" + ex.StackTrace);
            }

        }

        // PUT api/values/5
        [HttpPut]
        public IHttpActionResult EditParentTask([FromBody]ParentTaskModel ParentTask)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid data");
            try
            {
                TaskManagerRepository parentTaskRep = new TaskManagerRepository();

                Parent_Task_Tbl ParentTaskDb = parentTaskRep.GetParentTaskRepo(ParentTask.ParentTaskID);

                if (ParentTaskDb != null)
                {
                    ParentTaskDb.Parent_Task = ParentTask.ParentTaskName;
                    string result = "{'ParentTaskID': " + parentTaskRep.EditParentTask(ParentTaskDb) + "}";
                    JObject json = JObject.Parse(result);
                    return Ok<JObject>(json);
                }
                else
                {
                    return BadRequest("Error occurred during data update in EditParentTask");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred in EditParentTask :" + ex.StackTrace);
            }
        }
        [HttpPost]
        public IHttpActionResult CreateTask([FromBody]TaskModel TaskModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid data");
            try
            {
                TaskManagerRepository taskRep = new TaskManagerRepository();
                Task_Tbl TaskDb = new Task_Tbl
                {
                    Task = TaskModel.TaskName,
                    Start_Date = Convert.ToDateTime(TaskModel.StartDate),
                    End_Date = Convert.ToDateTime(TaskModel.EndDate),
                    Priority = TaskModel.Priority
                };
                string result = "{'TaskID': " + taskRep.CreateTask(TaskDb) + "}";
                JObject json = JObject.Parse(result);
                return Ok<JObject>(json);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred in CreateTask :" + ex.StackTrace);
            }

        }

        // PUT api/values/5
        [HttpPut]
        public IHttpActionResult EditTask([FromBody]TaskModel TaskModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid data");
            try
            {
                TaskManagerRepository taskRep = new TaskManagerRepository();

                Task_Tbl TaskDb = taskRep.GetTaskRepo(TaskModel.TaskId);

                if (TaskDb != null)
                {
                    TaskDb.Task = TaskModel.TaskName;
                    TaskDb.Start_Date = Convert.ToDateTime(TaskModel.StartDate);
                    TaskDb.End_Date = Convert.ToDateTime(TaskModel.EndDate);
                    TaskDb.Priority = TaskModel.Priority;
                    TaskDb.Is_Completed = Convert.ToBoolean(TaskModel.IsCompleted);
                    string result = "{'TaskID': " + taskRep.EditTask(TaskDb) + "}";
                    JObject json = JObject.Parse(result);
                    return Ok<JObject>(json);
                }
                else
                {
                    return BadRequest("Error occurred during data update in EditTask");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred in EditTask :" + ex.StackTrace);
            }
        }
        [HttpDelete]
        public IHttpActionResult DeleteTask(int TaskID)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid data");
            try
            {
                TaskManagerRepository TaskRep = new TaskManagerRepository();

                if (TaskID > 0)
                {
                    TaskRep.DeleteTask(TaskID);
                    return Ok();
                }
                else
                {
                    return BadRequest("Error occurred during data deletion in DeleteTask");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred in DeleteTask :" + ex.StackTrace);
            }
        }
        [HttpDelete]
        public IHttpActionResult DeleteParentTask(int ParentTaskID)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid data");
            try
            {
                TaskManagerRepository parentTaskRep = new TaskManagerRepository();


                if (ParentTaskID > 0)
                {
                    parentTaskRep.DeleteParentTask(ParentTaskID);
                    return Ok();
                }
                else
                {
                    return BadRequest("Error occurred during data deletion in DeleteParentTask");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred in DeleteParentTask :" + ex.StackTrace);
            }
        }

        [HttpPut]
        public IHttpActionResult EditEndTask([FromBody]TaskModel TaskModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid data");
            try
            {
                TaskManagerRepository taskRep = new TaskManagerRepository();

                Task_Tbl TaskDb = taskRep.GetTaskRepo(TaskModel.TaskId);

                if (TaskDb != null)
                {
                    TaskDb.Is_Completed = Convert.ToBoolean(TaskModel.IsCompleted);
                    string result = "{'TaskID': " + taskRep.EditEndTask(TaskDb) + "}";
                    JObject json = JObject.Parse(result);
                    return Ok<JObject>(json);
                }
                else
                {
                    return BadRequest("Error occurred during data update in EditEndTask");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred in EditEndTask :" + ex.StackTrace);
            }
        }

    }
}
