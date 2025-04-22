using Microsoft.AspNetCore.Mvc;
using ReportingSolution.Data.EventSourcingModels;

namespace ReportingSolution.AKSWeatherApi.Controllers
{
    [ApiController]
    [Route("api/bank-accounts")]
    public class BankAccountsController : ControllerBase
    {
        private readonly IBankAccountRepository _bankAccountRepository;

        public BankAccountsController(IBankAccountRepository bankAccountRepository)
        {
            _bankAccountRepository = bankAccountRepository;
        }

        [HttpGet("{id:guid}")]
        public async ValueTask<IActionResult> Get(Guid id, CancellationToken cancellationToken)
        {
            var original = await _bankAccountRepository.GetByIdAsync(id, cancellationToken);
            if (original is null) 
            {
                return NotFound();
            }

            return Ok(original);
        }

        [HttpPost("")]
        public async ValueTask<IActionResult> OpenBankAccount(string owner, CancellationToken cancellationToken)
        {
            var account = BankAccount.OpenBankAccount(Guid.NewGuid(), owner);
            await _bankAccountRepository.SaveAsync(account, cancellationToken);
            return Ok(account);
        }

        [HttpPost("{id:guid}/deposit")]
        public async ValueTask<IActionResult> Deposit(Guid id, decimal amount, CancellationToken cancellationToken)
        {
            var original = await _bankAccountRepository.GetByIdAsync(id, cancellationToken);
            original.Deposit(amount);
            await _bankAccountRepository.SaveAsync(original, cancellationToken);

            return Ok(original);
        }

        [HttpPost("{id:guid}/withdraw")]
        public async ValueTask<IActionResult> Withdraw(Guid id, decimal amount, CancellationToken cancellationToken)
        {
            var original = await _bankAccountRepository.GetByIdAsync(id, cancellationToken);
            original.Withdraw(amount);
            await _bankAccountRepository.SaveAsync(original, cancellationToken);

            return Ok(original);
        }
    }
}
