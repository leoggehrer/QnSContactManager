using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Contract = QnSContactManager.Contracts.Persistence.App.IContact;
using Model = QnSContactManager.AspMvc.Models.Persistence.App.Contact;

namespace QnSContactManager.AspMvc.Controllers
{
    public class ContactController : AccessController
    {
        public ContactController(IFactoryWrapper factoryWrapper) 
            : base(factoryWrapper)
        {
        }
        [ActionName("Create")]
        public async Task<IActionResult> CreateAsync()
        {
            using var ctrl = Factory.Create<Contract>(SessionWrapper.LoginSession.SessionToken);
            var entity = await ctrl.CreateAsync();

            return View(ConvertTo<Model, Contract>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<IActionResult> CreateAsync(Model model)
        {
            using var ctrl = Factory.Create<Contract>(SessionWrapper.LoginSession.SessionToken);
            
            try
            {
                await ctrl.InsertAsync(model);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                model.ActionError = ex.Message;
            }
            return View(model);
        }

        private string CsvNull => "<NULL>";
        private string Separator => ";";
        private string[] CsvHeader => new String[] { "Id", "Name", "Email", "Addresse", "Type" };
        
        [ActionName("Export")]
        public async Task<FileResult> ExportAsync()
        {
            using var ctrl = Factory.Create<Contract>(SessionWrapper.SessionToken);
            var fileName = $"{typeof(Model).Name}.csv";
            var entities = await ctrl.GetAllAsync();

            return ExportDefault(CsvHeader, entities, fileName);
        }
        protected virtual FileResult ExportDefault(IEnumerable<string> csvHeader, IEnumerable<Contract> entities, string fileName)
        {
            List<byte> contentData = new List<byte>();
            var encodingPreamble = Encoding.UTF8.GetPreamble();

            contentData.AddRange(encodingPreamble);
            contentData.AddRange(Encoding.UTF8.GetBytes(csvHeader.Aggregate((s1, s2) => $"{s1}{Separator}{s2}")));

            foreach (var item in entities)
            {
                var exportLine = new StringBuilder();

                foreach (var field in csvHeader)
                {
                    if (exportLine.Length > 0)
                        exportLine.Append(Separator);

                    var pi = item.GetType().GetProperty(field);

                    if (pi != null && pi.CanRead)
                    {
                        var value = pi.GetValue(item);

                        if (value != null)
                        {
                            exportLine.Append(value.ToString());
                        }
                        else
                        {
                            exportLine.Append(CsvNull);
                        }
                    }
                }
                contentData.AddRange(Encoding.UTF8.GetBytes(Environment.NewLine));
                contentData.AddRange(Encoding.UTF8.GetBytes(exportLine.ToString()));
            }
            string contentType = "text/csv";

            return File(contentData.ToArray(), contentType, fileName);
        }

        public ActionResult Import()
        {
            var model = new Models.Modules.Export.ImportModel();

            return View(model);
        }

        enum ImportAction
        {
            None = 0,
            Insert,
            Update,
            Delete,
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Import")]
        public async Task<IActionResult> ImportAsync(Models.Modules.Export.ImportModel model)
        {
            var fileCount = GetRequestFileCount();
            using var ctrl = Factory.Create<Contract>(SessionWrapper.SessionToken);

            if (fileCount == 1)
            {
                var hpf = GetRequestFileData(0);

                if (hpf.Length > 0)
                {
                    var idIdx = Array.IndexOf(CsvHeader, "Id");
                    var text = Encoding.Default.GetString(hpf, 0, hpf.Length);
                    var lines = text.Split(Environment.NewLine);
                    var logInfos = new List<Models.Modules.Export.ImportModel.LogInfo>();

                    for (int i = 1; i < lines.Length; i++)
                    {
                        var action = ImportAction.None;

                        try
                        {
                            var data = lines[i].Split(Separator);

                            if (idIdx >= 0 && CsvHeader.Length == data.Length)
                            {
                                if (Int32.TryParse(data[idIdx], out int id))
                                {
                                    if (id < 0)
                                    {
                                        action = ImportAction.Delete;
                                        await ctrl.DeleteAsync(Math.Abs(id));
                                    }
                                    else if (id > 0)
                                    {
                                        action = ImportAction.Update;
                                        await ctrl.UpdateAsync(CreateModelFromCsv(CsvHeader, data));
                                    }
                                    else
                                    {
                                        action = ImportAction.Insert;
                                        await ctrl.InsertAsync(CreateModelFromCsv(CsvHeader, data));
                                    }
                                }
                                else
                                {
                                    data[idIdx] = "0";
                                    action = ImportAction.Insert;
                                    await ctrl.InsertAsync(CreateModelFromCsv(CsvHeader, data));
                                }
                                logInfos.Add(new Models.Modules.Export.ImportModel.LogInfo
                                {
                                    IsError = false,
                                    Prefix = $"Line: {i} - {action}",
                                    Text = "OK",
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            logInfos.Add(new Models.Modules.Export.ImportModel.LogInfo
                            {
                                IsError = true,
                                Prefix = $"Line: {i} - {action}",
                                Text = ex.Message,
                            });
                        }
                    }
                    model.LogInfos = logInfos;
                }
            }
            return View(model);
        }

        private Model CreateModelFromCsv(string[] csvHeader, string[] data)
        {
            var result = new Model();

            for (int i = 0; i < csvHeader.Length; i++)
            {
                var pi = result.GetType().GetProperty(csvHeader[i]);

                if (pi != null && pi.CanWrite)
                {
                    var csvVal = data[i];

                    if (csvVal.Equals(CsvNull))
                    {
                        pi.SetValue(result, null);
                    }
                    else if (pi.PropertyType.IsEnum)
                    {
                        pi.SetValue(result, Enum.Parse(pi.PropertyType, csvVal));
                    }
                    else
                    {
                        pi.SetValue(result, Convert.ChangeType(csvVal, pi.PropertyType));
                    }
                }
            }
            return result;
        }

        protected int GetRequestFileCount()
        {
            return Request.Form.Files.Count;
        }
        protected IFormFile GetRequestFormFile(int index)
        {
            IFormFile result = null;

            if (Request.Form.Files.Count > index)
            {
                result = Request.Form.Files[index];
            }
            return result;
        }
        protected string GetRequestFileName(int index)
        {
            IFormFile formFile = GetRequestFormFile(index);

            return formFile?.FileName ?? string.Empty;
        }
        protected byte[] GetRequestFileData(int index)
        {
            return GetRequestFileData(GetRequestFormFile(index));
        }
        protected byte[] GetRequestFileData(IFormFile formFile)
        {
            byte[] result = null;

            if (formFile != null)
            {
                using var inputStream = formFile.OpenReadStream();
                if (!(inputStream is MemoryStream memoryStream))
                {
                    using (memoryStream = new MemoryStream())
                    {
                        inputStream.CopyTo(memoryStream);
                        result = memoryStream.ToArray();
                    }
                }
                else
                {
                    result = memoryStream.ToArray();
                }
            }
            return result;
        }
    }
}