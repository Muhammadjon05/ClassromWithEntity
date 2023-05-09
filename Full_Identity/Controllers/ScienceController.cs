using Full_Identity.DTOs;
using Full_Identity.Helper;
using IdentityData.AppDbContext;
using IdentityData.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Full_Identity.Controllers;

public class ScienceController : Controller
{
    private readonly Context _context;
    private readonly UserProvider _userProvider;

    public ScienceController(Context context, UserProvider userProvider)
    {
        _context = context;
        _userProvider = userProvider;
    }

    public async Task<IActionResult> Index(Guid Id)
    {
     
        var school = await _context.Schools.
            Include(s => s.Sciences!)
            .ThenInclude(o => o.UserSciences).FirstOrDefaultAsync(k => k.Id == Id);
        return View(school);
    }

    [HttpGet]
    public IActionResult CreateScience(Guid Id)
    {
        ViewBag.Id = Id;
        return View();
    } 
    
    [HttpPost]
    public async Task<IActionResult> CreateScience(CreateScienceDTO createScienceDto,Guid schoolId)
    {
        var userId = _userProvider.UserId;
        var science = new Science()
        {
            SchoolId = schoolId,
            Name = createScienceDto.Name,
            Description = createScienceDto.Description,
        };
        
        science.UserSciences = new List<UserScience>()
        {
            new UserScience()
            {
                UserId = userId,
                Type = IdentityData.Enums.UserScience.Teacher,

            }
        };
        _context.Sciences.Add(science);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index",new{ Id = schoolId});
    }

    public async Task<IActionResult> GetScienceById(Guid Id)
    {
        var science = await _context.Sciences
            .Include(i => i.UserSciences)!.
            ThenInclude(i => i.User).
            ThenInclude(t=>t.UserSchools).Include(i=>i.School).
            FirstOrDefaultAsync(i => i.Id == Id);
        
        
        return View(science);
    }
    [HttpGet]
    public async Task<IActionResult> UpdateScience(Guid Id)
    {
        var science = await _context.Sciences.FirstOrDefaultAsync(i => i.Id == Id);
        ViewBag.Id = Id;
        return View(new UpdateScienceDTO()
        {
            Name = science.Name,
            Description = science.Description,
        });
    }

    [HttpPost]
    public async Task<IActionResult> UpdateScience(Guid Id, [FromForm] UpdateScienceDTO updateScienceDto)
    {
        var science =  await _context.Sciences.FirstOrDefaultAsync(i => i.Id == Id);
        science.Name = updateScienceDto.Name;
        science.Description = updateScienceDto.Description;

        await _context.SaveChangesAsync();
        return RedirectToAction("GetScienceById", new { id = science.Id });
    }

    [HttpGet]
    public IActionResult RequestToJoin(Guid Id)
    {
        ViewBag.Id = Id;
        return View();
    }
  [HttpPost]
    public async Task<IActionResult> RequestToJoin(Guid Id, CreateRequestToJoin createRequestToJoin)
    {
        var toUser = await _context.Users.
            FirstOrDefaultAsync(i => i.UserName == createRequestToJoin.Username);
        var isExist = await _context.SendRequestToJoins.
            AnyAsync(i => i.ToUserId ==toUser!.Id  && i.ScienceId == Id);
        if (isExist)
        {
            return RedirectToAction("GetScienceById", new { id = Id });
        }
        var userId = _userProvider.UserId;

        var request = new SendRequestToJoin()
        {
            FromWhomId = userId,
            ScienceId = Id,
            ToUserId = toUser.Id,
            IsJoined = false
        };
        _context.SendRequestToJoins.Add(request);
        await _context.SaveChangesAsync();
        return RedirectToAction("GetScienceById", new { id = Id });
    }

    public async Task<IActionResult> JoinScience(Guid joinRequestId,bool isJoin)
    {
        var joinRequest = await _context.SendRequestToJoins.FirstOrDefaultAsync(r =>
                r.Id == joinRequestId && r.ToUserId == _userProvider.UserId);
        
        if (isJoin)
        {
            var userScience = new UserScience()
            {
                ScienceId = joinRequest.ScienceId,
                UserId = joinRequest.ToUserId,
                Type = IdentityData.Enums.UserScience.Student
            };
            joinRequest.IsJoined = true;
            _context.UserSciences.Add(userScience);
        }
        else
        {
            _context.SendRequestToJoins.Remove(joinRequest!);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Profile", "User");
    } 




}