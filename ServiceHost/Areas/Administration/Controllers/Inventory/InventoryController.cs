using DiscountManagement.Application.Contracts.CustomerDiscount;
using InventoryManagement.Application.Contracts.Inventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nancy.Json;
using ShopManagement.Application.Contracts.Product;

namespace ServiceHost.Areas.Administration.Controllers.Inventory
{
    [Authorize(Policy = "Administration")]
    public class InventoryController : Controller
    {
        private readonly IInventoryApplication _inventoryApplication;
        private readonly IProductApplication _productApplication;



        public InventoryController(IInventoryApplication inventoryApplication, IProductApplication productApplication)
        {
            _inventoryApplication = inventoryApplication;
            _productApplication = productApplication;
        }

        [TempData]
        public string Message { get; set; }


        [Area("Administration")]
        [Route("admin/inventory")]
        [HttpGet]
        public async Task<IActionResult> Index(long? productId, bool? inStock)
        {
            var serchModel = new InventorySearchModel()
            {
                InStock = inStock,
                ProductId = productId
            };

            var inventoryViewModels = await _inventoryApplication.Search(serchModel);

            var products = new SelectList(await _productApplication.GetProducts(), "Id", "Name");
            ViewBag.products = products;

            return View(inventoryViewModels);
        }



        [Area("Administration")]
        [Route("admin/inventory/Create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var command = new CreateInventory();
            
            command.Products =  await _productApplication.GetProducts();
            return PartialView("_Create", command);
        }

        [Area("Administration")]
        [Route("admin/inventory/Create")]
        [HttpPost]
        public async Task<JsonResult> Create(CreateInventory commend)
        {
            var result = await _inventoryApplication.Create(commend);

            return new JsonResult(result);
        }



        [Area("Administration")]
        [Route("admin/inventory/Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var editInventory = await _inventoryApplication.GetDetails(id);
            editInventory.Products = await _productApplication.GetProducts();

            return PartialView("_Edit", editInventory);
        }

        [Area("Administration")]
        [Route("admin/inventory/Edit")]

        public async Task<JsonResult> Edit(EditInventory commend)
        {
            var result = await _inventoryApplication.Edit(commend);
            return new JsonResult(result);
        }

        [Area("Administration")]
        [Route("admin/inventory/Increase")]
        [HttpGet]
        public async Task<IActionResult> Increase(long id)
        {

            var command = new IncreaseInventory
            {
                InventoryId = id
            };

            return PartialView("_Increase", command);
        }
        [Area("Administration")]
        [Route("admin/inventory/Increase")]
        [HttpPost]
        public async Task<JsonResult> Increase(IncreaseInventory command)
        {

            var result = await _inventoryApplication.Increase(command);

            return new JsonResult(result);
        }




        [Area("Administration")]
        [Route("admin/inventory/Decrease")]
        public async Task<IActionResult> Decrease(long id)
        {

            var command = new DecreaseInventory()
            {
                InventoryId = id
            };

            return PartialView("_Decrease", command);
        }

        [Area("Administration")]
        [Route("admin/inventory/Decrease")]
        [HttpPost]
        public async Task<JsonResult> Decrease(DecreaseInventory command)
        {

            var result = await _inventoryApplication.Reduce(command);


            return new JsonResult(result);
        }




        [Area("Administration")]
        [Route("admin/inventory/Log")]
        [HttpGet]
        public async Task<IActionResult> Log(long id)
        {
            var result = await _inventoryApplication.GetOperationLog(id);


            return PartialView("_Log", result);
        }

       
    }
}
