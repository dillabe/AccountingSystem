using AccountingSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class TransactionController : Controller
{
    private readonly ApplicationDbContext _context;

    public TransactionController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Transaction/Create
    public async Task<IActionResult> Create()
    {
        // Зареждаме акаунтите за dropdown-a
        var accounts = await _context.Accounts.ToListAsync();
        ViewData["AccountId"] = new SelectList(accounts, "Id", "Name");
        return View();
    }

    // POST: Transaction/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Transaction transaction)
    {
        // Проверка за валидността на модела
        if (!ModelState.IsValid)
        {
            var accounts = await _context.Accounts.ToListAsync();
            ViewData["AccountId"] = new SelectList(accounts, "Id", "Name", transaction.AccountId);
            return View(transaction);
        }

        try
        {
            // Проверка дали акаунтът съществува
            var accountExists = await _context.Accounts.AnyAsync(a => a.Id == transaction.AccountId);
            if (!accountExists)
            {
                ModelState.AddModelError("", "Акаунтът не съществува.");
                var accounts = await _context.Accounts.ToListAsync();
                ViewData["AccountId"] = new SelectList(accounts, "Id", "Name", transaction.AccountId);
                return View(transaction);
            }

            // Добавяме новата транзакция в базата
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            // Пренасочваме към списъка с транзакции след успешното записване
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException dbEx)
        {
            // Логваме грешка при опит за запис в базата данни
            ModelState.AddModelError("", "Неуспешно записване в базата данни. Моля, опитайте отново.");
        }
        catch (Exception ex)
        {
            // Логваме общи грешки
            ModelState.AddModelError("", "Нещо се обърка при създаването на транзакцията.");
        }

        // Ако не е преминала валидацията или възникне грешка, връщаме формата
        var accountsList = await _context.Accounts.ToListAsync();
        ViewData["AccountId"] = new SelectList(accountsList, "Id", "Name", transaction.AccountId);
        return View(transaction);
    }

    // GET: Transaction/Index
    public async Task<IActionResult> Index()
    {
        // Зареждаме транзакциите, включително и свързаните акаунти
        var transactions = await _context.Transactions
                                          .Include(t => t.Account)
                                          .ToListAsync();
        return View(transactions);
    }
}
