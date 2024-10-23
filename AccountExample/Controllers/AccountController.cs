using AccountExample.Models.Accounts;
using AccountManagment.Core.Models;
using AccountManagment.Core.Services;
using AccountManagment.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AccountExample.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IGenericService<Account> _accountService;
        private readonly IGenericService<Transfer> _transferService;
        private readonly IGenericService<AccountTransaction> _accountTransactionService;
        private readonly AppDbContext _context;


        public AccountController(AppDbContext context,IGenericService<Transfer> transferService, IGenericService<Account> accountService, IGenericService<AccountTransaction> accountTransactionService)
        {
            _accountService = accountService;
            _accountTransactionService = accountTransactionService;
            _transferService = transferService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var parsedUserId))
            {
                return RedirectToAction("Login", "User");
            }
            var accounts = await _accountService.Where(x => x.UserId == parsedUserId && !x.IsDeleted).OrderByDescending(x => x.CreatedDate).ToListAsync();
            return View(accounts);
        }
       

        [HttpPost]
        public async Task<IActionResult> Create(string type)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var parsedUserId))
                {
                    return RedirectToAction("Login", "User");
                }
                var account = new Account
                {
                    Type=type,
                    UserId= parsedUserId
                };

                await _accountService.AddAsync(account);
                return RedirectToAction("Index", "Account");
            }

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> CreateTransfer()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var parsedUserId))
            {
                return RedirectToAction("Login", "User");
            }

            var accounts = await _accountService
                .Where(x => x.UserId == parsedUserId && !x.IsDeleted && x.IsActive)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            var model = new TransferViewModel
            {
                Accounts = accounts.Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = $"{a.Number} - {a.Iban} - ({a.Balance} ₺)"
                }).ToList()
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> CreateTransfer(TransferViewModel model)
        { var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var parsedUserId))
            {
                return RedirectToAction("Login", "User");
            }
                List<Account> accounts = new();
            if (ModelState.IsValid)
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        
                        var targetAccount = await _accountService.Where(x => x.Iban == model.TargetAccountIban && !x.IsDeleted &&x.IsActive).Include(x => x.User).FirstOrDefaultAsync();
                        var sourceAccount = await _accountService.Where(x => x.Id == model.SourceAccountId && !x.IsDeleted && x.IsActive).Include(x => x.User).FirstOrDefaultAsync();

                        if (targetAccount == null || sourceAccount == null || sourceAccount.Balance<model.Amount)
                        {
                             accounts = await _accountService
                .Where(x => x.UserId == parsedUserId && !x.IsDeleted && x.IsActive)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
                            model.Accounts= accounts.Select(a => new SelectListItem
                            {
                                Value = a.Id.ToString(),
                                Text = $"{a.Number} - {a.Iban} - ({a.Balance} ₺)"
                            }).ToList();
                            if (targetAccount == null)
                            {
                                ModelState.AddModelError("TargetAccountIban", "Alıcı hesap geçersiz.");
                            }
                            else if (sourceAccount == null)
                            {
                                ModelState.AddModelError("SourceAccountId", "Gönderen hesap geçersiz.");
                            }
                            else if (targetAccount.Balance < model.Amount)
                            {
                                ModelState.AddModelError("Amount", "Hesap bakiyesi yetersiz.");
                            }

                            return View(model);
                        }

                        var transfer = new Transfer
                        {
                            Amount = model.Amount,
                            Description = model.Description,
                            SourceAccountId = model.SourceAccountId,
                            TargetAccountId = targetAccount.Id
                        };

                        await _transferService.AddAsync(transfer);

                        sourceAccount.Balance -= model.Amount;
                        targetAccount.Balance += model.Amount;

                        await _accountService.UpdateAsync(targetAccount);
                        await _accountService.UpdateAsync(sourceAccount);

                        var sourceTransaction = new AccountTransaction()
                        {
                            Amount = model.Amount,
                            Description = model.Description,
                            AccountId = sourceAccount.Id,
                            Direction = false, // Çıkış işlemi
                            TransferId = transfer.Id
                        };
                        await _accountTransactionService.AddAsync(sourceTransaction);

                        var targetTransaction = new AccountTransaction()
                        {
                            Amount = model.Amount,
                            Description = model.Description,
                            AccountId = targetAccount.Id,
                            Direction = true, // Giriş işlemi
                            TransferId = transfer.Id
                        };
                        await _accountTransactionService.AddAsync(targetTransaction);

                        await transaction.CommitAsync();

                        return RedirectToAction("ListTransfer", "Account", targetAccount.Id);
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();

                        ModelState.AddModelError(string.Empty, "Transfer işlemi sırasında bir hata oluştu: " + ex.Message);
                    }
                }
            }
             accounts = await _accountService
              .Where(x => x.UserId == parsedUserId && !x.IsDeleted && x.IsActive)
              .OrderByDescending(x => x.CreatedDate)
              .ToListAsync();
            model.Accounts = accounts.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = $"{a.Number} - {a.Iban} - ({a.Balance} ₺)"
            }).ToList();
            return View(model);
        }
        public async Task<IActionResult> ListTransfer()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var parsedUserId))
            {
                return RedirectToAction("Login", "User");
            }
            var transferList = await _transferService.Where(x => x.SourceAccount.UserId == parsedUserId && !x.IsDeleted)
                .Include(x => x.SourceAccount).
                Include(x => x.TargetAccount).ThenInclude(x => x.User)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
            return View(transferList);
        }
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> ListAllTransfer()
        {
            var transferList = await _transferService.Where(x => !x.IsDeleted)
                .Include(x => x.SourceAccount).
                Include(x => x.TargetAccount).ThenInclude(x => x.User)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
            return View(transferList);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]

        public async Task<IActionResult> RemoveTransfer(int id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var transfer = await _transferService.Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
                    if (transfer != null)
                    {
                        var targetTransaction = await _accountTransactionService
                            .Where(x => x.TransferId == id && x.AccountId == transfer.TargetAccountId && !x.IsDeleted)
                            .FirstOrDefaultAsync();
                        var sourceTransaction = await _accountTransactionService
                            .Where(x => x.TransferId == id && x.AccountId == transfer.SourceAccountId && !x.IsDeleted)
                            .FirstOrDefaultAsync();
                        if (targetTransaction == null || sourceTransaction == null)
                        {
                            throw new Exception("İşlem kaydı bulunamadı");
                        }

                        targetTransaction.IsDeleted = true;
                        sourceTransaction.IsDeleted = true;
                        await _accountTransactionService.UpdateAsync(sourceTransaction);
                        await _accountTransactionService.UpdateAsync(targetTransaction);

                        var targetAccount = await _accountService
                            .Where(x => x.Id == transfer.TargetAccountId && !x.IsDeleted)
                            .FirstOrDefaultAsync();
                        var sourceAccount = await _accountService
                            .Where(x => x.Id == transfer.SourceAccountId && !x.IsDeleted)
                            .FirstOrDefaultAsync();
                        if (sourceAccount == null || targetAccount == null)
                        {
                            throw new Exception("Hesap bulunamadı");
                        }
                        sourceAccount.Balance += transfer.Amount; 
                        targetAccount.Balance -= transfer.Amount;  

                        await _accountService.UpdateAsync(targetAccount);
                        await _accountService.UpdateAsync(sourceAccount);

                        transfer.IsDeleted = true;
                        await _transferService.UpdateAsync(transfer);

                        await transaction.CommitAsync();
                    }

                    return RedirectToAction("ListAllTransfer", "Account");
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    return View("Error"); 
                }
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var account = await _accountService.Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
            if (account == null)
            {
                throw new Exception("Hesap bulunamadı");
            }
            var accountTransactions = await _accountTransactionService.Where(x => x.AccountId == id && !x.IsDeleted).OrderByDescending(x => x.CreatedDate).ToListAsync();
            var accountDetail = new AccountDetailWithTransacitonViewModel()
            {
                AccountDetail = account,
                AccountTransacitons = accountTransactions
            };


            return View(accountDetail);
        }
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> RemoveAccount(int id)
        {
            var account = await _accountService.Where(x => x.Id == id && !x.IsDeleted).Include(x=>x.User).FirstOrDefaultAsync();
            if (account == null)
            {
                throw new Exception("Hesap bulunamadı");
            }
            account.IsDeleted = true;
            await _accountService.UpdateAsync(account);


            return RedirectToAction("DetailForAdmin", "User",account.User.Id);
        }
    }
}
