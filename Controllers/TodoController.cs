using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogMVC.Data;
using BlogMVC.Models;
using BlogMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace BlogMVC.Controllers
{
    public class TodoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TodoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Todo
        public async Task<IActionResult> Index()
        {
            List<TodoItem> todoItems = new List<TodoItem>();
            List<TodoItemViewModel> todoItemsVM = new List<TodoItemViewModel>();
            TodoItemViewModel todoItemVM = new TodoItemViewModel();
            todoItems = await _context.TodoItem.ToListAsync();
            foreach (TodoItem item in todoItems)
            {
              todoItemVM = new TodoItemViewModel
                {
                    Id = item.Id,
                    Task = item.Task,
                    Detail = item.Detail,
                    IsComplete = item.IsComplete,
                    IsWorkingOn = item.IsWorkingOn,
                    Created = item.Created,
                    Finished = item.Finished,
                    StartWorking = item.StartWorking,                  
            };
                if (item.IsComplete && item.IsWorkingOn)
                {
                    todoItemVM.Status = "Complete";
                }
                if (!item.IsComplete && !item.IsWorkingOn)
                {
                    todoItemVM.Status = "Pending";
                }
                if (item.IsWorkingOn && !item.IsComplete)
                {
                    todoItemVM.Status = "In Progress";
                }
                if (item.StartWorking == null)
                {
                    todoItemVM.PlanDuration = 0;
                }
                else
                {
                    todoItemVM.PlanDuration = ((TimeSpan)(item.StartWorking - item.Created)).Days;
                }
                if (item.Finished == null)
                {
                    todoItemVM.WorkDuration = 0;
                }
                else
                {
                    todoItemVM.WorkDuration = ((TimeSpan)(item.Finished - item.StartWorking)).Days;
                }
                if (item.StartWorking != null && item.Finished != null)
                {
                    todoItemVM.TotalDuration = ((TimeSpan)(item.Finished - item.Created)).Days;
                }
              
                todoItemsVM.Add(todoItemVM);
            }
            

            return View(todoItemsVM);
        }

        // GET: Todo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoItem = await _context.TodoItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todoItem == null)
            {
                return NotFound();
            }

            return View(todoItem);
        }
        [HttpGet,Authorize(Roles = "Admin")]
        // GET: Todo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Todo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, Authorize(Roles="Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Created,Task,Detail,IsComplete,IsWorkingOn,StartWorking,Finished,Status")] TodoItemViewModel todoItemVM)
        {
            TodoItem todoItem;
            if (todoItemVM.Status == "Pending")
            {
                todoItemVM.Created = DateTime.Now;
                todoItem = new TodoItem
                {
                    Id= todoItemVM.Id,
                    Created = todoItemVM.Created,
                    Task = todoItemVM.Task,
                    Detail = todoItemVM.Detail,
                    IsComplete = false,
                    IsWorkingOn = false,
                };
            } else if (todoItemVM.Status == "In Progress")
            {
                todoItem = new TodoItem
                {
                    Id = todoItemVM.Id,
                    Task = todoItemVM.Task,
                    Detail = todoItemVM.Detail,
                    IsComplete = false,
                    IsWorkingOn = true,
                    StartWorking = todoItemVM.StartWorking,
                };
                if (todoItemVM.Created == null)
                {
                    todoItem.Created = todoItemVM.StartWorking;
                }
                if (todoItemVM.StartWorking == null)
                {
                    todoItem.Created = DateTime.Now;
                    todoItem.StartWorking = todoItem.Created;
                }
            }
            else
            {
               todoItem = new TodoItem
                {
                    Id = todoItemVM.Id,
                    Task = todoItemVM.Task,
                    Detail = todoItemVM.Detail,
                    IsComplete = true,
                    IsWorkingOn = true,
                    StartWorking = todoItemVM.StartWorking,
                    Created = todoItemVM.Created,
                    Finished = todoItemVM.Finished,
                };
                if (todoItemVM.Created == null)
                {
                    todoItem.Created = DateTime.Now;
                }
                if (todoItemVM.StartWorking == null)
                {
                    todoItem.StartWorking = todoItem.Created;
                }
                if (todoItemVM.Finished == null)
                {
                    todoItem.Finished = todoItem.Created;
                }
              
            }
        
            if (ModelState.IsValid)
            {
                _context.Add(todoItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(todoItem);
        }

        // GET: Todo/Edit/5
        [Authorize(Roles= "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            TodoItemViewModel todoItemVM;
            if (id == null)
            {
                return NotFound();
            }

            var todoItem = await _context.TodoItem.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            else
            {
                todoItemVM = new TodoItemViewModel
                {
                    Id = todoItem.Id,
                    Created = todoItem.Created,
                    IsWorkingOn = todoItem.IsWorkingOn,
                    IsComplete = todoItem.IsComplete,
                    Task = todoItem.Task,
                    Detail = todoItem.Detail,
                    StartWorking = todoItem.StartWorking,
                    Finished = todoItem.Finished,
                };
                if (todoItemVM.IsComplete && todoItemVM.IsWorkingOn)
                {
                    todoItemVM.Status = "Complete";
                } 
                if (!todoItemVM.IsComplete && !todoItemVM.IsWorkingOn)
                {
                    todoItemVM.Status = "Pending";
                }
                if (todoItemVM.IsWorkingOn && !todoItemVM.IsComplete)
                {
                    todoItemVM.Status = "In Progress";
                }

            }
            return View(todoItemVM);
        }

        // POST: Todo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Created,Task,Detail,IsComplete,IsWorkingOn,StartWorking,Finished,Status")] TodoItemViewModel todoItemVM)
        {
           
            if (id != todoItemVM.Id)
            {
                return NotFound();
            }

            var todoItem = await _context.TodoItem.FindAsync(id);

            if (todoItemVM.Status == "Pending")
            {
              
                todoItem.Created = DateTime.Now;
                todoItem.Task = todoItemVM.Task;
                todoItem.Detail = todoItemVM.Detail;
                todoItem.IsComplete = false;
                todoItem.IsWorkingOn = false;
                todoItem.StartWorking = null;
                todoItem.Finished = null;         
            }

            if (todoItemVM.Status == "In Progress")
            {
                todoItem.Task = todoItemVM.Task;
                todoItem.Detail = todoItemVM.Detail;
                todoItem.IsComplete = false;
                todoItem.IsWorkingOn = true;
                todoItem.StartWorking = DateTime.Now;
                todoItem.Finished = null;                
            }

            if (todoItemVM.Status == "Complete")
            {
                todoItem.Task = todoItemVM.Task;
                todoItem.Detail = todoItemVM.Detail;
                todoItem.IsComplete = true;
                todoItem.IsWorkingOn = true;
                todoItem.Finished = DateTime.Now;
                }
       
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(todoItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoItemExists(todoItem.Id))
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
            return View(todoItem);
        }
        [Authorize(Roles= "Admin")]
        // GET: Todo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoItem = await _context.TodoItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todoItem == null)
            {
                return NotFound();
            }

            return View(todoItem);
        }

        // POST: Todo/Delete/5
        [HttpPost, ActionName("Delete"), Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var todoItem = await _context.TodoItem.FindAsync(id);
            _context.TodoItem.Remove(todoItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TodoItemExists(int id)
        {
            return _context.TodoItem.Any(e => e.Id == id);
        }
    }
}
