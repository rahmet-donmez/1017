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

        public AccountController(IGenericService<Transfer> transferService, IGenericService<Account> accountService, IGenericService<AccountTransaction> accountTransactionService)
        {
            _accountService = accountService;
            _accountTransactionService = accountTransactionService;
            _transferService = transferService;
        }

        public async Task<IActionResult> Index()
        {
            var accounts = await _accountService.Where(x => x.UserId == 1 && !x.IsDeleted).OrderByDescending(x => x.CreatedDate).ToListAsync();
            return View(accounts);
        }
        [HttpGet]

        public async Task<IActionResult> Create()
        {
            return View(new AccountCreateViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountCreateViewModel accountCreateViewModel)
        {
            if (ModelState.IsValid)
            {

                var account = new Account
                {
                };

                await _accountService.AddAsync(account);
                return RedirectToAction("Index", "Account");
            }

            return View(accountCreateViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> CreateTransfer()
        {
            var accounts = await _accountService.Where(x => x.UserId == 1 && !x.IsDeleted && x.IsActive).OrderByDescending(x => x.CreatedDate).ToListAsync();
            var model = new TransferViewModel
            {
                Accounts = accounts.Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Number + " - " + a.Iban + " - (" + a.Balance + " ₺)"
                }).ToList()
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTransfer(TransferViewModel model)
        {

            if (ModelState.IsValid)
            {
                var targetAccount = await _accountService.Where(x => x.Iban == model.TargetAccountIban && !x.IsDeleted).FirstOrDefaultAsync();
                var sourceAccount = await _accountService.Where(x => x.Id == model.SourceAccountId && !x.IsDeleted).FirstOrDefaultAsync();

                sourceAccount.Balance -= model.Amount;
                targetAccount.Balance += model.Amount;

                await _accountService.UpdateAsync(targetAccount);
                await _accountService.UpdateAsync(sourceAccount);

                var transfer = new Transfer
                {
                    Amount = model.Amount,
                    Description = model.Description,
                    SourceAccountId = model.SourceAccountId,
                    TargetAccountId = targetAccount.Id
                };

                await _transferService.AddAsync(transfer);
                return RedirectToAction("ListTransfer", "Account", targetAccount.Id);
            }

            return View(model);
        }
        public async Task<IActionResult> ListTransfer()
        {
            var transferList = await _transferService.Where(x => x.SourceAccount.UserId == 1 && !x.IsDeleted)
                .Include(x => x.SourceAccount).
                Include(x => x.TargetAccount).ThenInclude(x => x.User)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
            return View(transferList);
        }

        public async Task<IActionResult> Details(int id)
        {
            var account = await _accountService.Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
            var accountTransactions = await _accountTransactionService.Where(x => x.AccountId == id && !x.IsDeleted).OrderByDescending(x => x.CreatedDate).ToListAsync();
            var accountDetail = new AccountDetailWithTransacitonViewModel()
            {
                AccountDetail = account,
                AccountTransacitons = accountTransactions
            };


            return View(accountDetail);
        }
    }
}
