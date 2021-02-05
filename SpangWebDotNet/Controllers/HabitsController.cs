using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpangWebDotNet.Data;
using SpangWebDotNet.Models;

namespace SpangWebDotNet.Controllers
{
    public class HabitsController : Controller
    {
        private readonly HabitTrackerContext _context;

        public HabitsController(HabitTrackerContext context)
        {
            _context = context;
        }

        // GET: Habits
        public async Task<IActionResult> Index()
        {
            return View(await _context.Habit.ToListAsync());
        }

        // GET: Habits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habits = await _context.Habit
                .FirstOrDefaultAsync(m => m.Id == id);
            if (habits == null)
            {
                return NotFound();
            }

            return View(habits);
        }

        // GET: Habits/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Habits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Daily_Habit,Positive_Negative_Neutral")] Habits habits)
        {
            if (ModelState.IsValid)
            {
                _context.Add(habits);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(habits);
        }

        // GET: Habits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habits = await _context.Habit.FindAsync(id);
            if (habits == null)
            {
                return NotFound();
            }
            return View(habits);
        }

        // POST: Habits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Habit,Completed")] Habits habits)
        {
            if (id != habits.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(habits);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HabitsExists(habits.Id))
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
            return View(habits);
        }

        // GET: Habits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habits = await _context.Habit
                .FirstOrDefaultAsync(m => m.Id == id);
            if (habits == null)
            {
                return NotFound();
            }

            return View(habits);
        }

        // POST: Habits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var habits = await _context.Habit.FindAsync(id);
            _context.Habit.Remove(habits);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HabitsExists(int id)
        {
            return _context.Habit.Any(e => e.Id == id);
        }
    }
}
