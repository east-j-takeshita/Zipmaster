using KadaiMVCApp.Models;
using Microsoft.AspNetCore.Mvc;
using KadaiMVCApp.Repository;
using System.IO.Compression;
using System.Text.Json;
using System.Collections;
using System.Runtime.CompilerServices;

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
            ViewData["SearchMessage"] = "";
            ViewData["Search"] = "検索したいキーワードを入力して検索してください";
            return View(new ZipViewModel());
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

        [HttpPost("PostCode/Create")]
        public async Task<ActionResult<List<Zip>>> Create(Zip zip)
        {
            
            var zipRepository=new ZipRepository();
            zipRepository.CreateZipmaster(zip);
            
            return View("index", new ZipViewModel());
        }

        [HttpPost("PostCode/Index")]
        public async Task<ActionResult<List<Zip>>> Index(Keyword keyword)
        {
            ViewData["Postcode"]=keyword.PostCode; 
            ViewData["Prefecture"]=keyword.Prefecture; 
            ViewData["City"]=keyword.City;
            ViewData["ShipToAddress"] = keyword.ShipToAddress; 

            var zips = new List<Zip>();
            //if (postcode.Length != 7)//前方一致のためコメントアウト
            //{
            //    //ConterntResultはActionResultを継承しているから、使える
            //    ViewData["Message"] = "郵便番号が7文字ではありません。";
            //    return View("Index");
            //}

            var zipRepository = new ZipRepository();
            zips =await zipRepository.GetZipmaster(keyword);
            // データが見つからなかった場合
            var zipsData= new ZipViewModel() { };
            if (zips.Count ==0)
            {
                ViewData["SearchMessage"] = "該当データはありません。";
                return View("Index",zipsData);
            }
            zipsData.ZipsData = zips;//zipsをZipViewModelのZipsDataに格納
            
            return View(zipsData);
        }

        [HttpGet("PostCodeDetail/{ID?}")]
        public async Task<ActionResult<Zip>> PostCodeDetail(int ID)
        {
            ViewData["PostOrderID"] = ID;
            var zip = new ZipViewModel();
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

        [HttpPost("PostCode/Update/{id?}")]//
        public async Task<ActionResult> Update(int id, Zip zipmaster)
        {
            var zipMasterRepository = new ZipRepository();
            zipmaster.PostOrderID = id;
            zipMasterRepository.Update(id, zipmaster);//id入力をPostOrderIDに入れれば、良い
            
            return View("index", new ZipViewModel());
           
        }


        [HttpPost("PostCode/Delete/{id?}")]////?は?id を省略可能、外せない。
        public async Task<IActionResult> Delete(int id)
        {
            var zipMasterRepository = new ZipRepository();
            
            zipMasterRepository.Delete(id);

            
            return View("Index", new ZipViewModel());
        }

        private String ObjectToString(Zip zip)
        {
            return JsonSerializer.Serialize<Zip>(zip);
        }
    }
}
