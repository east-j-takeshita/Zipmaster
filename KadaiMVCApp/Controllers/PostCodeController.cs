using KadaiMVCApp.Models;
using Microsoft.AspNetCore.Mvc;
using KadaiMVCApp.Repository;
using System.IO.Compression;
using System.Text.Json;
using System.Collections;
using System.Runtime.CompilerServices;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace KadaiMVCApp.Controllers
{
    public class PostCodeController : Controller
    {
        private string _connectionstring;
        
        public PostCodeController()
        {
            _connectionstring = AppSettings.dbConnectString;//AppSettingsクラスから文字列'(静的クラスなので、そのまま取得)
        }
        public IActionResult Index()
        {
            var zipViewModel = new ZipViewModel();
            zipViewModel.Keyword=new Keyword();
            return View("index",zipViewModel);
        }
        public IActionResult PostCodeDetail()
        {
            return View("index");
        }
        public IActionResult Create()//これをまず書かないとビューとして見れない
        {
            return View();
        }

        [HttpPost("/Create")]
        public async Task<ActionResult<List<Zip>>> Create(Zip zip)
        {
            var zipRepository = new ZipRepository();
            zipRepository.CreateZipmaster(zip);
            var zipViewModel = new ZipViewModel();
            zipViewModel.Keyword = new Keyword();
            return View("index", zipViewModel);
        }

        [HttpPost("Index/{postcode?}/{keyword?}")]
        public async Task<ActionResult<ZipViewModel>> Index(string postcode, string keyword)
        {


            //if (postcode.Length != 7)//前方一致のためコメントアウト
            //{
            //    //ConterntResultはActionResultを継承しているから、使える
            //    ViewData["Message"] = "郵便番号が7文字ではありません。";
            //    return View("Index");
            //}
            var zips = new List<Zip>();
            var zipRepository = new ZipRepository();
            zips = await zipRepository.GetZipmaster(postcode,keyword);
            // データが見つからなかった場合
            var zipViewModel = new ZipViewModel();
            zipViewModel.Keyword=new Keyword();
            zipViewModel.Keyword.KeyPostCode = postcode;
            zipViewModel.Keyword.KeyWord= keyword;
            
            if (zips.Count == 0)
            {
                return zipViewModel;
            }
            zipViewModel.ZipsData = zips;//zipsをZipViewModelのZipsDataに格納
            return zipViewModel;
        }

        [HttpGet("PostCodeDetail/{ID?}")]
        public async Task<ActionResult<Zip>> PostCodeDetail(int ID)
        {
            var zip = new Zip();
            var zipRepository = new ZipRepository();
            zip = await zipRepository.PostOrderIDGetZip(ID);

            if (zip == null)
            {
                return View("index", new ZipViewModel());
            }
            else
            {

                return View(zip);

            }
        }

        [HttpPost("Update/{id?}")]//
        public async Task<IActionResult> Update(int id, Zip zipmaster)
        {
            var zipMasterRepository = new ZipRepository();
            zipmaster.PostOrderID = id;
            zipMasterRepository.Update(id, zipmaster);//id入力をPostOrderIDに入れれば、良い

            var zipViewModel = new ZipViewModel();
            zipViewModel.Keyword = new Keyword();
            return Index();

        }


        [HttpPost("Delete/{id?}")]////?は?id を省略可能、外せない。
        public async Task<IActionResult> Delete(int id)
        {
            var zipMasterRepository = new ZipRepository();

            zipMasterRepository.Delete(id);

            var zipViewModel = new ZipViewModel();
            zipViewModel.Keyword = new Keyword();
            return View("index", zipViewModel);
        }

        private String ObjectToString(Zip zip)
        {
            return JsonSerializer.Serialize<Zip>(zip);
        }
    }
}
