using Full_Identity.DTOs;
using Full_Identity.Helper;
using IdentityData.AppDbContext;
using IdentityData.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Full_Identity.Controllers;

public class TaskController : Controller
{
    private readonly Context _context;
    private readonly UserProvider UserProvider;

        public TaskController(Context context, UserProvider userProvider)
        {
            _context = context;
            UserProvider = userProvider;
        }
    public async Task<IActionResult> Index(Guid scienceId)
    {
        
        var tasks = await _context.TaskEntities.
            Where(i=>i.ScienceId ==scienceId)
            .ToListAsync();
        ViewBag.ScienceId = scienceId;
        return View(tasks);
    }

    [HttpGet]
    public IActionResult AddTask(Guid scienceId)
    {
        ViewBag.ScienceId = scienceId;
        return View();
    }   
    [HttpPost]
    public async Task<IActionResult> AddTask(Guid scienceId,[FromForm] CreateTaskDto createTaskDto)
    {
        var task = new TaskEntity()
        {
            Title = createTaskDto.Title,
            Description = createTaskDto.Description,
            CreatedDate = createTaskDto.CreatedDate,
            StartDate = createTaskDto.StartDate,
            EndDate = createTaskDto.EndDate,
            ScienceId = scienceId,
            Status = TaskStatus.Created,
            MaxBall = createTaskDto.MaxBall,
        };
        _context.TaskEntities.Add(task);
        await _context.SaveChangesAsync();
        
        return RedirectToAction("Index", new { scienceId = scienceId });
    }

    public Task<IActionResult> GetById(Guid taskId)
    {
        var task =  _context.TaskEntities.Include(y=>y.TaskComments).ThenInclude(i=>i.User).FirstOrDefault(i => i.Id == taskId)!;
        return Task.FromResult<IActionResult>(View(task));
    }

    public async Task<IActionResult> AddComment(Guid taskId,[FromForm] CreateComment createComment)
    {
        var comment = new TaskComment()
        {
            Message = createComment.Comment,
            TaskId = taskId,
            UserId = UserProvider.UserId
        };
        _context.TaskComments.Add(comment);
        await _context.SaveChangesAsync();
        return RedirectToAction("GetById", new { taskId });
    }
}