using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.Office.Interop.Excel;
using TestWebApi.Models;

namespace TestWebApi.Controllers
{
    public class FileController : ApiController
    {
        [HttpPost]
        [Route("api/file/upload")]
        public IHttpActionResult UploadFile()
        {
            var postedFile = HttpContext.Current.Request.Files["File"];
            try
            {
                var filePath = UploadExcelFile(postedFile);
                var excelData = GetExcelDataRange(filePath);

                List<User> users = new List<User>();
                for (int row = 2; row < excelData.Rows.Count + 1; row++)
                {
                    User user = new User();
                    user.Name = excelData.Cells[row, 1].Text;
                    user.Email = excelData.Cells[row, 2].Text;
                    user.Password = excelData.Cells[row, 3].Text;
                    users.Add(user);
                }
                var result = new
                {
                    Status = true,
                    Data = users,
                    Message = "Succcess!",
                    FilePath = filePath
                };
                return Ok(result);
            }
            catch(Exception err)
            {
                var result = new
                {
                    Status = false,
                    Data = "No data available",
                    Message = err.Message
                };
                return Ok(result);
            }
        }


        private Range GetExcelDataRange(string filePath)
        {
            Application application = new Application();
            Workbook workBook = application.Workbooks.Open(filePath);
            Worksheet worksheet = workBook.ActiveSheet;
            Range range = worksheet.UsedRange;
            if (range.Rows.Count ==0)
            {
                //throw new Exception("No data found in uploaded excel file!");
            }
            return range;
        }

        private string UploadExcelFile(HttpPostedFile file)
        {
            try
            {
                string filePath = null;
                string fileName = new String(Path.GetFileNameWithoutExtension(file.FileName).Take(10).ToArray()).Replace(" ", "-");
                fileName = fileName + DateTime.Now.ToString("yyyymmssfff") + Path.GetExtension(file.FileName);
                string fileExtension = Path.GetExtension(file.FileName);
                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    filePath = HttpContext.Current.Server.MapPath("~/FileDocument/Excel/" + fileName);
                }
                else
                {
                    //throw new Exception("Unsupported file format!");
                }
                file.SaveAs(filePath);
                return filePath;
            }catch(Exception err)
            {
                throw new Exception(err.Message);
            }
        }
    }
}
