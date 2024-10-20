using AccountExample.Models.Accounts;
using AccountManagment.Core.Models;
using AccountManagment.Core.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AccountExample.Controllers
{
    public class AccountController : Controller
    {
        private readonly IGenericService<Account> _accountService;
        private readonly IGenericService<Transfer> _transferService;
        private readonly IGenericService<AccountTransaction> _accountTransactionService;

        public AccountController(IGenericService<Transfer> transferService,IGenericService<Account> accountService, IGenericService<AccountTransaction> accountTransactionService)
        {
            _accountService = accountService;
            _accountTransactionService = accountTransactionService;
            _transferService=transferService;
        }

        public async Task<IActionResult> Index()
        {
            var accounts = await _accountService.Where(x => x.UserId == 1 && !x.IsDeleted).ToListAsync();
            return View(accounts);
        }
        public async Task<IActionResult> CreateTransfer()
        {
            var accounts = await _accountService.Where(x => x.UserId == 1 && !x.IsDeleted).ToListAsync();
            var model = new TransferViewModel
            {
                Accounts = accounts.Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Number
                }).ToList()
            };

            return View(model);
        }
        public async Task<IActionResult> CreateTransfer(TransferViewModel model)
        {
            if (ModelState.IsValid)
            {
                var targetAccountId = await _accountService.Where(x => x.Iban==model.TargetAccountIban && !x.IsDeleted).Select(x=>x.Id).FirstOrDefaultAsync();

                var transfer = new Transfer
                {
                    Amount = model.Amount,
                    Description = model.Description,
                    SourceAccountId = model.SourceAccountId,
                    TargetAccountId = targetAccountId
                };

                await _transferService.AddAsync(transfer);
                return RedirectToAction("ListTransfer", "Account", targetAccountId);
            }

            return View(model);
        }
        public async Task<IActionResult> ListTransfer(int accountId)
        {
            var transferList = await _transferService.Where(x => x.SourceAccountId == accountId && !x.IsDeleted)
                .Include(x=>x.SourceAccount).
                Include(x=>x.TargetAccount).ThenInclude(x=>x.User)
                .OrderByDescending(x=>x.CreatedDate)
                .ToListAsync();
            return View(transferList);
        }

        public async Task<IActionResult> Details(int id)
        {
            var account=await _accountService.Where(x=>x.Id==id && !x.IsDeleted).FirstOrDefaultAsync();
            var accountTransactions=await _accountTransactionService.Where(x=>x.AccountId==id && !x.IsDeleted).ToListAsync();
            var accountDetail = new AccountDetailWithTransacitonViewModel()
            {
                AccountDetail = account,
                AccountTransacitons = accountTransactions
            };


            return View(accountDetail);
        }
    }
}
