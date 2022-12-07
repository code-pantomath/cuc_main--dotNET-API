//using AutoMapper;
//using CheapUdemy.Contracts;
//using CheapUdemy.Data;
//using CheapUdemy.Models.Payment;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace CheapUdemy.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class PaymentsController : ControllerBase
//    {

//        private readonly IMapper _mapper;
//        private readonly IPaymentsRepository _paymentsRepo;


//        public PaymentsController(IMapper mapper, IPaymentsRepository PaymentsRepository)
//        {
//            this._mapper = mapper;
//            this._paymentsRepo = PaymentsRepository;
//        }




//        [HttpPost]
//        public async Task<ActionResult<Payment>> PostPayment (CreatePaymentDto createPaymentDto)
//        {
//            Payment m_payment = _mapper.Map<Payment>(createPaymentDto);
//            Payment? payment = await _paymentsRepo.Pay(m_payment);

//            if (payment is null) return Problem();

//            return payment;
//        }


//        //////


//    }
//}
