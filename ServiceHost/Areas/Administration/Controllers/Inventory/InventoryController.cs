using DiscountManagement.Application.Contracts.CustomerDiscount;
using InventoryManagement.Application.Contracts.Inventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Product;

namespace ServiceHost.Areas.Administration.Controllers.Inventory
{
    [Authorize(Policy = "Administration")]
    public class InventoryController : Controller
    {
        private readonly IInventoryApplication _inventoryApplication;
        private readonly IProductApplication _productApplication;

        private List<InventoryViewModel> _inventoryViewModels = new List<InventoryViewModel>();


        public InventoryController(IInventoryApplication inventoryApplication, IProductApplication productApplication)
        {
            _inventoryApplication = inventoryApplication;
            _productApplication = productApplication;
        }

        [TempData]
        public string Message { get; set; }


        [Area("Administration")]
        [Route("admin/inventory/index")]
        [HttpGet]
        public IActionResult Index(long? productId, bool? inStock)
        {
            var serchModel = new InventorySearchModel()
            {
                InStock = inStock,
                ProductId = productId
            };

            _inventoryViewModels = _inventoryApplication.Search(serchModel);

            var products = new SelectList(_productApplication.GetProducts(), "Id", "Name");
            ViewBag.products = products;

            return View(_inventoryViewModels);
        }



        [Area("Administration")]
        [Route("admin/inventory/Create")]
        [HttpGet]
        public IActionResult Create()
        {
            var command = new CreateInventory();
            
            command.Products = _productApplication.GetProducts();
            return PartialView("_Create", command);
        }

        [Area("Administration")]
        [Route("admin/inventory/Create")]
        [HttpPost]
        public JsonResult Create(CreateInventory commend)
        {
            var result = _inventoryApplication.Create(commend);

            return new JsonResult(result);
        }



        [Area("Administration")]
        [Route("admin/inventory/Edit")]
        [HttpGet]
        public IActionResult Edit(long id)
        {
            var editInventory = _inventoryApplication.GetDetails(id);
            editInventory.Products = _productApplication.GetProducts();

            return PartialView("_Edit", editInventory);
        }

        [Area("Administration")]
        [Route("admin/inventory/Edit")]

        public JsonResult Edit(EditInventory commend)
        {
            var result = _inventoryApplication.Edit(commend);
            return new JsonResult(result);
        }

        [Area("Administration")]
        [Route("admin/inventory/Increase")]
        [HttpGet]
        public IActionResult Increase(long id)
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
        public IActionResult Increase(IncreaseInventory command)
        {

            var result = _inventoryApplication.Increase(command);

            return new JsonResult(result);
        }




        [Area("Administration")]
        [Route("admin/inventory/Decrease")]
        public IActionResult Decrease(long id)
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
        public IActionResult Decrease(DecreaseInventory command)
        {

            var result = _inventoryApplication.Reduce(command);


            return new JsonResult(result);
        }




        [Area("Administration")]
        [Route("admin/inventory/Log")]
        [HttpGet]
        public IActionResult Log(long id)
        {
            var result = _inventoryApplication.GetOperationLog(id);


            return PartialView("_Log", result);
        }

       
    }
}
