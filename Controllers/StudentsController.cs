using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DOTNET_lab4.Data;
using DOTNET_lab4.Models;
using DOTNET_lab4.Models.ViewModels;

namespace DOTNET_lab4.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolCommunityContext _context;

        public StudentsController(SchoolCommunityContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string ? ID)
        {
            var viewModel = new CommunityViewModel();
            viewModel.Students = await _context.Students
                .Include(i => i.Membership).ThenInclude(i => i.community)
                .AsNoTracking()
                .OrderBy(i => i.LastName)
                .ToListAsync();
            if(ID != null)
            {
                ViewData["StudentID"] = ID;
                viewModel.Memberships = viewModel.Students.Where(x => x.id.ToString() == ID).Single().Membership.AsEnumerable();
            }

            return View(viewModel);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/EditMemberships/5
        public async Task<IActionResult> EditMemberships(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viewModel = new StudentMembershipsViewModel();
            viewModel.student = _context.Students
                .Include(i => i.Membership)
                .ThenInclude(i => i.community) // fetch related data to student
                .Where(i => i.id == id).Single();

            var members = viewModel.student.Membership.Select(m => m.community).ToArray(); // make queryable array

            viewModel.communities = await _context.Communities.Select(i => i)
                .Where(i => !members.Contains(i)) // if the student membership list has (i)community we drop it from the comunities list
                .ToListAsync();  
                
            return View(viewModel);
        }

        //POST students/SetMemberships/5
        [HttpPost]
        public async Task<IActionResult> SetMemberships([Bind("StudentID","CommunityID")]CommunityMembership membership)
        {
            var Mexists = await _context.Memberships.FindAsync(membership.StudentID,membership.CommunityID); // check if membership exists
            if(Mexists != null)
            {
                _context.Memberships.Remove(Mexists); // remove the existing membership
                await _context.SaveChangesAsync();
            }
            else
            {
                var Sexists = await _context.Students.FindAsync(membership.StudentID); // check if student exists
                var Cexists = await _context.Communities.FindAsync(membership.CommunityID); // check if community exists
                if (Sexists == null || Cexists == null)
                {
                    return RedirectToAction(nameof(Index)); //the student or community don't exist return to students/index
                }
                _context.Memberships.Add(membership);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("EditMemberships", new { id = membership.StudentID });
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,LastName,FirstName,EnrollmentDate,MembershipID")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,LastName,FirstName,EnrollmentDate,MembershipID")] Student student)
        {
            if (id != student.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.id == id);
        }
    }
}
