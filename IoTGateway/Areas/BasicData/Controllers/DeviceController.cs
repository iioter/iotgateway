using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Mvc;
using WalkingTec.Mvvm.Core.Extensions;
using IoTGateway.ViewModel.BasicData.DeviceVMs;
using Plugin;
using IoTGateway.ViewModel.BasicData;

namespace IoTGateway.Controllers
{
    [Area("BasicData")]
    [ActionDescription("设备维护")]
    public partial class DeviceController : BaseController
    {
        private DeviceService _DeviceService;
        public DeviceController(DeviceService deviceService)
        {
            _DeviceService = deviceService;
        }
        #region Search
        [ActionDescription("Sys.Search")]
        public ActionResult Index()
        {
            var vm = Wtm.CreateVM<DeviceListVM>();
            return PartialView(vm);
        }

        [ActionDescription("Sys.Search")]
        [HttpPost]
        public string Search(DeviceSearcher searcher)
        {
            var vm = Wtm.CreateVM<DeviceListVM>(passInit: true);
            if (ModelState.IsValid)
            {
                vm.Searcher = searcher;
                return vm.GetJson(false);
            }
            else
            {
                return vm.GetError();
            }
        }

        #endregion

        #region Create
        [ActionDescription("创建设备")]
        public ActionResult Create()
        {
            var vm = Wtm.CreateVM<DeviceVM>();
            vm.Entity.DeviceTypeEnum = Model.DeviceTypeEnum.Device;
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("创建设备")]
        public ActionResult Create(DeviceVM vm)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                vm.Entity.DeviceTypeEnum = Model.DeviceTypeEnum.Device;
                vm.DoAdd();
                if (!ModelState.IsValid)
                {
                    vm.DoReInit();
                    return PartialView(vm);
                }
                else
                {
                    return FFResult().CloseDialog().RefreshGrid();
                }
            }
        }
        #endregion
        #region Create
        [ActionDescription("Sys.Create")]
        public ActionResult CreateGroup()
        {
            var vm = Wtm.CreateVM<DeviceVM>();
            vm.Entity.DeviceTypeEnum = Model.DeviceTypeEnum.Group;
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Sys.Create")]
        public ActionResult CreateGroup(DeviceVM vm)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                vm.Entity.DeviceTypeEnum = Model.DeviceTypeEnum.Group;
                vm.DoAdd();
                if (!ModelState.IsValid)
                {
                    vm.DoReInit();
                    return PartialView(vm);
                }
                else
                {
                    return FFResult().CloseDialog().RefreshGrid();
                }
            }
        }
        #endregion

        #region Edit
        [ActionDescription("Sys.Edit")]
        public ActionResult Edit(string id)
        {
            var vm = Wtm.CreateVM<DeviceVM>(id);
            return PartialView(vm);
        }

        [ActionDescription("Sys.Edit")]
        [HttpPost]
        [ValidateFormItemOnly]
        public ActionResult Edit(DeviceVM vm)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                vm.DoEdit();
                if (!ModelState.IsValid)
                {
                    vm.DoReInit();
                    return PartialView(vm);
                }
                else
                {
                    return FFResult().CloseDialog().RefreshGridRow(vm.Entity.ID);
                }
            }
        }
        #endregion

        #region Delete
        [ActionDescription("Sys.Delete")]
        public ActionResult Delete(string id)
        {
            var vm = Wtm.CreateVM<DeviceVM>(id);
            return PartialView(vm);
        }

        [ActionDescription("Sys.Delete")]
        [HttpPost]
        public ActionResult Delete(string id, IFormCollection nouse)
        {
            var vm = Wtm.CreateVM<DeviceVM>(id);
            vm.DoDelete();
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid();
            }
        }
        #endregion

        #region Details
        [ActionDescription("Sys.Details")]
        public ActionResult Details(string id)
        {
            var vm = Wtm.CreateVM<DeviceVM>(id);
            return PartialView(vm);
        }
        #endregion

        #region BatchEdit
        [HttpPost]
        [ActionDescription("Sys.BatchEdit")]
        public ActionResult BatchEdit(string[] IDs)
        {
            var vm = Wtm.CreateVM<DeviceBatchVM>(Ids: IDs);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Sys.BatchEdit")]
        public ActionResult DoBatchEdit(DeviceBatchVM vm, IFormCollection nouse)
        {
            if (!ModelState.IsValid || !vm.DoBatchEdit())
            {
                return PartialView("BatchEdit",vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid().Alert(Localizer["Sys.BatchEditSuccess", vm.Ids.Length]);
            }
        }
        #endregion

        #region BatchDelete
        [HttpPost]
        [ActionDescription("Sys.BatchDelete")]
        public ActionResult BatchDelete(string[] IDs)
        {
            var vm = Wtm.CreateVM<DeviceBatchVM>(Ids: IDs);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Sys.BatchDelete")]
        public ActionResult DoBatchDelete(DeviceBatchVM vm, IFormCollection nouse)
        {
            if (!ModelState.IsValid || !vm.DoBatchDelete())
            {
                return PartialView("BatchDelete",vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid().Alert(Localizer["Sys.BatchDeleteSuccess", vm.Ids.Length]);
            }
        }
        #endregion

        #region Import
		[ActionDescription("Sys.Import")]
        public ActionResult Import()
        {
            var vm = Wtm.CreateVM<DeviceImportVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Sys.Import")]
        public ActionResult Import(DeviceImportVM vm, IFormCollection nouse)
        {
            if (vm.ErrorListVM.EntityList.Count > 0 || !vm.BatchSaveData())
            {
                return PartialView(vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid().Alert(Localizer["Sys.ImportSuccess", vm.EntityList.Count.ToString()]);
            }
        }
        #endregion

        [ActionDescription("Sys.Export")]
        [HttpPost]
        public IActionResult ExportExcel(DeviceListVM vm)
        {
            ExportDevicesSetting myExporter = new ExportDevicesSetting();
            myExporter.DC = vm.DC;
            var data = myExporter.Export();

            string ContentType = "application/vnd.ms-excel";
            string exportName = "DeviceSettings";
            exportName = $"Export_{exportName}_{DateTime.Now.ToString("yyyyMMddHHmmssffff")}.xlsx";
            FileContentResult Result = new FileContentResult(data, ContentType);
            Result.FileDownloadName = exportName;
            return Result;

            //return vm.GetExportData();
        }

        #region 设备复制
        [ActionDescription("设备复制")]
        public ActionResult Copy()
        {
            var vm = Wtm.CreateVM<CopyVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("设备复制")]
        public ActionResult Copy(CopyVM vm)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                vm.Copy();
                return FFResult().CloseDialog().RefreshGrid().Alert($"{vm.复制结果}");
            }
        }
        #endregion

        #region 获取属性
        [ActionDescription("获取属性")]
        public ActionResult Attribute()
        {
            var vm = Wtm.CreateVM<AttributeVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("获取属性")]
        public ActionResult Attribute(AttributeVM vm)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                vm.Request();
                return FFResult().CloseDialog().RefreshGrid().Alert($"{vm.请求结果}");
            }
        }
        #endregion
        public IActionResult GetMethods(Guid? ID)
        {
            return JsonMore(_DeviceService.GetDriverMethods(ID));
        }


        #region 导入Excel
        [ActionDescription("导入Excel")]
        public ActionResult ImportExcel()
        {
            var vm = Wtm.CreateVM<ImportExcelVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("导入Excel")]
        public ActionResult ImportExcel(ImportExcelVM vm)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                vm.Import();
                return FFResult().CloseDialog().RefreshGrid().Alert($"{vm.导入结果}");
            }
        }
        #endregion
    }
}
