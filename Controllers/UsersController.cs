using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CheapUdemy.Contracts;
using CheapUdemy.Data;
using CheapUdemy.Models.User;
using CheapUdemy.Models.Udem;


namespace CheapUdemy.Controllers
{
    [Route("api/[controller]")]
    //[Host("cheapudemy.com", "www.cheapudemy.com")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUsersRepository _usersRepo;


        public UsersController(IMapper mapper, IUsersRepository UsersRepository)
        {
            this._mapper = mapper;
            this._usersRepo = UsersRepository;
        }




        // GET: api/Users
        [HttpGet]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowCuc")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
          if (await _usersRepo.GetAllAsync() == null)
          {
              return NotFound();
          }


            //Response.Headers.AccessControlAllowOrigin = "https://cheapudemy.com";
            //Response.Headers["Access-Control-Allow-Origin"] = "https://cheapudemy.com";

            return (await _usersRepo.GetAllAsync());
        }


        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(CreateUserDto createUserDto)
        {
            if (await _usersRepo.GetAllAsync() == null)
            {
                //return Problem("Entity set 'MyAppDbContext.Users'  is null.");
            }


            User user = _mapper.Map<User>(createUserDto);

            user.Email = user.Email.ToLower();

            //user.IP = (string)HttpContext.Connection.RemoteIpAddress.Address.ToString();


            if ((await _usersRepo.GetAllAsync()).Any(u => u.Email.ToLower().Equals(user.Email?.ToLower())))
                return Problem("This email is already used!");
            else
            {
                await _usersRepo.AddAsync(user);
                return CreatedAtAction("GetUser", new { Id = user.Id }, user);
            };

        }

        

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id, [FromQuery] string? email="")
        {
            if (await _usersRepo.GetAllAsync() == null)
            {
              return NotFound();
            }

            User? user = email?.Trim().Length >= 5 ? await _usersRepo.GetByEmailAsync(email?.ToLower()) : await _usersRepo.GetAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }


        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest("Invalid Recore Id");
            }

            //_context.Entry(user).State = EntityState.Modified;


            try
            {
                await _usersRepo.UpdateAsync(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (await _usersRepo.GetAllAsync() == null)
            {
                return NotFound();
            }

            var user = await _usersRepo.GetAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _usersRepo.DeleteAsync(id);

            return NoContent();
        }



        private async Task<bool> UserExists(int id)
        {
            //return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();

            return await _usersRepo.Exists(id);
        }



        [HttpPost("Login")]
        public async Task<ActionResult<User>> PostLogin(CreateUserLoginDto createUserLoginDto)
        {

            UserLogin userLogin = _mapper.Map<UserLogin>(createUserLoginDto);

            if (!((await _usersRepo.GetAllAsync()).Any(u => u.Email.ToLower().Equals(userLogin.Email.ToLower()))))
            {
                return NotFound("The data you entered is wrong or the user is not registered !");
            }


            User? nullableUser = await _usersRepo.CheckUserLoginDataAsync(userLogin);


            if (nullableUser is not null)
                return nullableUser;
            else
                return Problem("Wrong details, Please check it and try again!");
        }


        [HttpPost("{id}/udem/accounts")]
        public async Task<ActionResult<List<UdemAccount>?>> PostUdemAccounts(int id, CreateUdemAccountDto createUdemAccount)
        {
            try
            {

                UdemAccount account = _mapper.Map<UdemAccount>(createUdemAccount);

                List<UdemAccount>? accounts = await _usersRepo.AddUdemyAccountAsync(account, id);


                if (accounts is not null)
                    return accounts;
                else
                    return Problem("You are allowed to add a maximum of 5 accounts only !");


            }
            catch (Exception err)
            {
                Console.Write(err.Message);
                return Problem("An error occured while processing your request !");
            }
        }

        [HttpGet("{id}/udem/accounts")]
        public async Task<ActionResult<List<UdemAccount>?>> GetUdemAccounts(int id)
        {

            List<UdemAccount>? accounts = await _usersRepo.GetUdemyAccountsAsync(id);

            if (accounts is not null)
                return accounts;
            else
                return Problem("An error occured while processing your request !");

        }

        [HttpDelete("{id}/udem/accounts/{udemAccId:int}")]
        public async Task<ActionResult<List<UdemAccount>?>> DeleteUdemAccounts(int id, int udemAccId)
        {

            List<UdemAccount>? accounts = await _usersRepo.DeleteUdemyAccountAsync(udemAccId, id);

            if (accounts is not null)
                return accounts;
            else
                return Problem("An error occured while processing your request !");

        }



        [HttpGet("{id}/wallet")]
        public async Task<ActionResult<Wallet?>> GetWallet(int id)
        {

            Wallet? wallet = await _usersRepo.GetWalletAsync(id);

            if (wallet is not null)
                return wallet;
            else
                return NotFound();

        }

        [HttpPatch("{id}/wallet/{xKey}/{amount}")]
        public async Task<ActionResult<Wallet?>> PatchtWallet(int id, string xKey, byte amount, [FromQuery(Name = "payload")] string paymentData)
        {
            if (xKey != "l0llmfa0123321") return Problem("Not authorized !");

            //byte amount_BYTE = (byte)Math.Abs(Convert.ToByte(amount));
            byte absAmount = (byte)Math.Abs(amount);

            if (!CONSTANTS.WALLET_CHARGE_RANGES.Contains<byte>(absAmount)) return Problem("The charge amount is not valid.");

            Wallet? wallet = await _usersRepo.AddWalletCreditsAsync(id, absAmount, paymentData);


            if (wallet is not null)
                return wallet;
            else
                return Problem("The wallet was not found!");

        }


        [HttpPost("{id}/wallet/transfer/{amount}")]
        public async Task<ActionResult<Wallet?>> PostWalletTransferCredits (int id, string amount, [FromQuery(Name = "email")] string? receiverEmail)
        {
            ushort amount_UINT16 = Convert.ToUInt16(amount);
            Wallet? senderWallet = await _usersRepo.TransferWalletCreditsAsync(id, receiverEmail, amount_UINT16);

            if (senderWallet is null) return Problem("There is no enough credits in the wallet !");
            else return senderWallet;
        }



        [HttpPost("{id}/services/purchase")]
        public async Task<ActionResult<UserServicePurchase?>> PostServiceePurchase(int id, CreateUserServicePurchaseDto createUserServicePurchaseDto)
        {
            User? user = await _usersRepo.GetAsync(id);
            Wallet? userWallet = await _usersRepo.GetWalletAsync(id);

            if (user is null || userWallet is null) return NotFound();

            UserServicePurchase m_purchase = _mapper.Map<UserServicePurchase>(createUserServicePurchaseDto);

            if (m_purchase.Type.Equals("gift") && userWallet.Value < CONSTANTS.SERVICE_GIFT_PRICE) return Problem("There is no enough balance !");
            if (m_purchase.Type.Equals("account") && userWallet.Value < CONSTANTS.SERVICE_ACCOUNT_PRICE) return Problem("There is no enough balance !");

            m_purchase.OwnerId = id;
            m_purchase.Price = m_purchase.Type.ToLower().Equals("gift") ? CONSTANTS.SERVICE_GIFT_PRICE : CONSTANTS.SERVICE_ACCOUNT_PRICE;

            //if (m_purchase.UdemEmail is )


            UserServicePurchase? purchase = await _usersRepo.PurchaseServiceAsync(m_purchase);

            if (purchase is not null)
                return purchase;
            else return NotFound();
        }

        [HttpGet("{id}/services/purchase/{purchaseType}/{history}")]
        public async Task<ActionResult<UserServicePurchase>> GetServiceePurchase(int id, string purchaseType, string history)
        {
            await Task.CompletedTask;

            int servicePurchaseId = 0;

            int.TryParse(history?.Split('i')?.ElementAtOrDefault(1), out servicePurchaseId);

            ////if (servicePurchaseId == 0) return NotFound();

            UserServicePurchase? usp = _usersRepo.GetServicePurchase(id, servicePurchaseId, purchaseType, history)?.Result;

            if (usp is null) return NotFound();
            else return usp;
        }

    }
}
