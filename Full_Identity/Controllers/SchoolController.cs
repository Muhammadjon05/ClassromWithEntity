using Full_Identity.DTOs;
using Full_Identity.Helper;
using IdentityData.AppDbContext;
using IdentityData.Entities;
using IdentityData.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Full_Identity.Controllers;

[Authorize]
public class SchoolController: Controller
{
    private readonly Context _context;
    private readonly UserProvider _userProvider;
    public SchoolController(Context context, UserProvider userProvider)
    {
        _context = context;
        _userProvider = userProvider;
    }
    public async Task<IActionResult> Index()
    {
        var schools = await _context.Schools.Include(i => i.UserSchools)
            .ToListAsync();
        return View(schools);
    }

    [HttpGet]
    public IActionResult CreateSchool()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> CreateSchool(CreateSchoolDTO createSchoolDto)
    {
        var school = new School()
        {
            Name = createSchoolDto.Name,
            Description = createSchoolDto.Description,
        };
        if (createSchoolDto.Photo != null)
        {
            school.PhotoUrl = await FileHelper.SaveSchoolFile(createSchoolDto.Photo);
        }
        school.UserSchools = new List<UserSchool>()
        {
            new UserSchool()
            {
                UserId = _userProvider.UserId,
                Status = UserStatus.Creator
            }
        };
        _context.Schools.Add(school);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> GetSchoolById(Guid id)
        {
            var schoolId = await _context.Schools.
                Include(i=>i.UserSchools).
                ThenInclude(i=>i.User).
                FirstOrDefaultAsync(i => i.Id == id);
                
            return View(schoolId);
        }
    public async Task<IActionResult> JoinSchool(Guid id)
    {
        var userId =  _userProvider.UserId;
        var school = await _context.Schools.
            Include(i => i.UserSchools)
            .ThenInclude(i => i.User).
                FirstOrDefaultAsync(i => i.Id == id);
        var exist = school.UserSchools.Any(i => i.User.Id == userId);
        if (!exist)
        {
            school.UserSchools.Add(new UserSchool()
            {
                UserId = userId,
                Status = UserStatus.Student
            });
        }
        await _context.SaveChangesAsync();
        return RedirectToAction("GetSchoolById", new { school.Id });
    }

    [HttpGet]
    public async Task<IActionResult> UpdateSchool(Guid Id)
    {
        var school = await _context.Schools.
            FirstOrDefaultAsync(i => i.Id == Id);

        ViewBag.Id = Id;
        return View(new UpdataScoolDTO()
        {
            Name = school.Name,
            Description = school.Description,
        });
    }
    [HttpPost]
    public async Task<IActionResult> UpdateSchool(Guid Id,[FromForm] UpdataScoolDTO updataScoolDto)
    {
        var school = await _context.Schools. 
            FirstOrDefaultAsync(i => i.Id == Id);
        school.Name = updataScoolDto.Name;
        school.Description = updataScoolDto.Description;

        if (updataScoolDto.Photo != null)
        {
            school.PhotoUrl = await FileHelper.SaveSchoolFile(updataScoolDto.Photo);
        }
        
        await _context.SaveChangesAsync();
        return RedirectToAction("GetSchoolById", new { id=Id });
    }

    public async Task<IActionResult> UpdateRole(Guid schoolId,Guid userId, UserStatus status)
    {
        var schoolRole = await _context.UserSchools.
            FirstOrDefaultAsync(i => i.SchoolId == schoolId && i.UserId == userId);
        if(schoolRole!.Status != UserStatus.Creator || status != UserStatus.Creator)
            schoolRole!.Status = status;
        await _context.SaveChangesAsync();
        return RedirectToAction("GetSchoolById", new { id = schoolRole.SchoolId });
    }
}