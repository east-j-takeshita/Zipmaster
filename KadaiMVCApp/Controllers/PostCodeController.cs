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
            zipViewModel.Message= new Message();
            zipViewModel.Keyword=new Keyword();
            zipViewModel.Message.SearchMessage = "";
            return View(zipViewModel);
        }
        public IActionResult PostCodeDetail()
        {
            ViewData["Message"] = "";

            return View("index", new ZipViewModel());

        }
        public IActionResult Create()//これをまず書かないとビューとして見れない
        {
            return View(new ZipViewModel());
        }

        [HttpPost("Create")]
        public async Task<ActionResult<List<Zip>>> Create(Zip zip)
        {

            var zipRepository = new ZipRepository();
            zipRepository.CreateZipmaster(zip);
            var zipViewModel = new ZipViewModel();
            zipViewModel.Keyword = new Keyword();
            return View("index", zipViewModel);
        }

        [HttpPost("Index")]
        public async Task<ActionResult<List<Zip>>> Index(string postcode, string keyword)
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
            var zipViewModel = new ZipViewModel() { };
            zipViewModel.Message = new Message();
            zipViewModel.Message.SearchMessage = "";
            zipViewModel.Keyword=new Keyword();
            zipViewModel.Keyword.KeyPostCode = postcode;
            zipViewModel.Keyword.KeyWord= keyword;

            if (zips.Count == 0)
            {
                return View("Index", zipViewModel);
            }
            zipViewModel.ZipsData = zips;//zipsをZipViewModelのZipsDataに格納

            return View(zipViewModel);
        }

        [HttpGet("PostCodeDetail/{ID?}")]
        public async Task<ActionResult<Zip>> PostCodeDetail(int ID)
        {
            ViewData["PostOrderID"] = ID;
            var zip = new Zip();
            var zipRepository = new ZipRepository();
            zip = await zipRepository.PostOrderIDGetZip(ID);

            if (zip == null)
            {
                ViewData["Message"] = "該当データはありません。";

                return View("index", new ZipViewModel());
            }

            //string[] zipData = new string[] { zip.PostOrderID.ToString(), zip.GroupCode, zip.OldPostCode,
            //    zip.PostCode,zip.Prefecture_Kana, zip.City_Kana, zip.ShipToAddress_Kana,zip.Prefecture, zip.City,
            //    zip.ShipToAddress,zip.SameShipToAddress.ToString(),zip.SubDistrictLevel.ToString(),
            //    zip.ChomeName.ToString(),zip.MultiplecityNumber.ToString(),zip.UpdateDate.ToString(),
            //    zip.UpdateReason.ToString()};
            else
            {

                return View(zip);

            }
        }

        [HttpPost("Update/{id?}")]//
        public async Task<ActionResult> Update(int id, Zip zipmaster)
        {
            var zipMasterRepository = new ZipRepository();
            zipmaster.PostOrderID = id;
            zipMasterRepository.Update(id, zipmaster);//id入力をPostOrderIDに入れれば、良い

            var zipViewModel = new ZipViewModel();
            zipViewModel.Message = new Message();
            zipViewModel.Message.Title = "検索";
            zipViewModel.Message.SearchMessage = "";
            zipViewModel.Keyword = new Keyword();
            zipViewModel.Message.SearchPostCode = "検索したい郵便番号を入力して検索してください";
            zipViewModel.Message.TextAreaMessage = "検索したいキーワードを入力して検索してください";
            return View("index", zipViewModel);

        }


        [HttpPost("Delete/{id?}")]////?は?id を省略可能、外せない。
        public async Task<IActionResult> Delete(int id)
        {
            var zipMasterRepository = new ZipRepository();

            zipMasterRepository.Delete(id);

            var zipViewModel = new ZipViewModel();
            zipViewModel.Message = new Message();
            zipViewModel.Message.Title = "検索";
            zipViewModel.Message.SearchMessage = "";
            zipViewModel.Keyword = new Keyword();
            zipViewModel.Message.SearchPostCode = "検索したい郵便番号を入力して検索してください";
            zipViewModel.Message.TextAreaMessage = "検索したいキーワードを入力して検索してください";
            return View("index", zipViewModel);
        }

        private String ObjectToString(Zip zip)
        {
            return JsonSerializer.Serialize<Zip>(zip);
        }
    }
}
