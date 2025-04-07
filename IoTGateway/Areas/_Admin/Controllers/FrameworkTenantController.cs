using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using WalkingTec.Mvvm.Mvc.Admin.ViewModels.FrameworkTenantVMs;

namespace WalkingTec.Mvvm.Mvc.Admin.Controllers
{
    [Area("_Admin")]
    [ActionDescription("MenuKey.FrameworkTenant")]
    [FixConnection(CsName = "default")]
    public partial class FrameworkTenantController : BaseController
    {
        #region Search

        [ActionDescription("Sys.Search")]
        public ActionResult Index()
        {
            if (CanUseTenant() == false)
            {
                return Content(Localizer["_Admin.TenantNotAllowed"]);
            }
            var vm = Wtm.CreateVM<FrameworkTenantListVM>();
            return PartialView(vm);
        }

        [ActionDescription("Sys.Search")]
        [HttpPost]
        public IActionResult Search(FrameworkTenantSearcher searcher)
        {
            if (CanUseTenant() == false)
            {
                return Content(Localizer["_Admin.TenantNotAllowed"]);
            }
            var vm = Wtm.CreateVM<FrameworkTenantListVM>(passInit: true);
            if (ModelState.IsValid)
            {
                vm.Searcher = searcher;
                return Content(vm.GetJson(false));
            }
            else
            {
                return Content(vm.GetError());
            }
        }

        #endregion Search

        #region Create

        [ActionDescription("Sys.Create")]
        public ActionResult Create()
        {
            if (CanUseTenant() == false)
            {
                return Content(Localizer["_Admin.TenantNotAllowed"]);
            }
            var vm = Wtm.CreateVM<FrameworkTenantVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Sys.Create")]
        public ActionResult Create(FrameworkTenantVM vm)
        {
            if (CanUseTenant() == false)
            {
                return Content(Localizer["_Admin.TenantNotAllowed"]);
            }
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
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

        #endregion Create

        #region Edit

        [ActionDescription("Sys.Edit")]
        public ActionResult Edit(string id)
        {
            if (CanUseTenant() == false)
            {
                return Content(Localizer["_Admin.TenantNotAllowed"]);
            }
            var vm = Wtm.CreateVM<FrameworkTenantVM>(id);
            return PartialView(vm);
        }

        [ActionDescription("Sys.Edit")]
        [HttpPost]
        [ValidateFormItemOnly]
        public ActionResult Edit(FrameworkTenantVM vm)
        {
            if (CanUseTenant() == false)
            {
                return Content(Localizer["_Admin.TenantNotAllowed"]);
            }
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

        #endregion Edit

        #region Delete

        [ActionDescription("Sys.Delete")]
        public ActionResult Delete(string id)
        {
            if (CanUseTenant() == false)
            {
                return Content(Localizer["_Admin.TenantNotAllowed"]);
            }
            var vm = Wtm.CreateVM<FrameworkTenantVM>(id);
            return PartialView(vm);
        }

        [ActionDescription("Sys.Delete")]
        [HttpPost]
        public ActionResult Delete(string id, IFormCollection nouse)
        {
            if (CanUseTenant() == false)
            {
                return Content(Localizer["_Admin.TenantNotAllowed"]);
            }
            var vm = Wtm.CreateVM<FrameworkTenantVM>(id);
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

        #endregion Delete

        #region Details

        [ActionDescription("Sys.Details")]
        public ActionResult Details(string id)
        {
            if (CanUseTenant() == false)
            {
                return Content(Localizer["_Admin.TenantNotAllowed"]);
            }
            var vm = Wtm.CreateVM<FrameworkTenantVM>(id);
            return PartialView(vm);
        }

        #endregion Details

        #region BatchEdit

        [HttpPost]
        [ActionDescription("Sys.BatchEdit")]
        public ActionResult BatchEdit(string[] IDs)
        {
            if (CanUseTenant() == false)
            {
                return Content(Localizer["_Admin.TenantNotAllowed"]);
            }
            var vm = Wtm.CreateVM<FrameworkTenantBatchVM>(Ids: IDs);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Sys.BatchEdit")]
        public ActionResult DoBatchEdit(FrameworkTenantBatchVM vm, IFormCollection nouse)
        {
            if (CanUseTenant() == false)
            {
                return Content(Localizer["_Admin.TenantNotAllowed"]);
            }
            if (!ModelState.IsValid || !vm.DoBatchEdit())
            {
                return PartialView("BatchEdit", vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid().Alert(Localizer["Sys.BatchEditSuccess", vm.Ids.Length]);
            }
        }

        #endregion BatchEdit

        #region BatchDelete

        [HttpPost]
        [ActionDescription("Sys.BatchDelete")]
        public ActionResult BatchDelete(string[] IDs)
        {
            if (CanUseTenant() == false)
            {
                return Content(Localizer["_Admin.TenantNotAllowed"]);
            }
            var vm = Wtm.CreateVM<FrameworkTenantBatchVM>(Ids: IDs);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Sys.BatchDelete")]
        public ActionResult DoBatchDelete(FrameworkTenantBatchVM vm, IFormCollection nouse)
        {
            if (CanUseTenant() == false)
            {
                return Content(Localizer["_Admin.TenantNotAllowed"]);
            }
            if (!ModelState.IsValid || !vm.DoBatchDelete())
            {
                return PartialView("BatchDelete", vm);
            }
            else
            {
                Cache.Delete(nameof(GlobalData.AllTenant));
                return FFResult().CloseDialog().RefreshGrid().Alert(Localizer["Sys.BatchDeleteSuccess", vm.Ids.Length]);
            }
        }

        #endregion BatchDelete

        #region Import

        [ActionDescription("Sys.Import")]
        public ActionResult Import()
        {
            if (CanUseTenant() == false)
            {
                return Content(Localizer["_Admin.TenantNotAllowed"]);
            }
            var vm = Wtm.CreateVM<FrameworkTenantImportVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Sys.Import")]
        public ActionResult Import(FrameworkTenantImportVM vm, IFormCollection nouse)
        {
            if (CanUseTenant() == false)
            {
                return Content(Localizer["_Admin.TenantNotAllowed"]);
            }
            if (vm.ErrorListVM.EntityList.Count > 0 || !vm.BatchSaveData())
            {
                return PartialView(vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid().Alert(Localizer["Sys.ImportSuccess", vm.EntityList.Count.ToString()]);
            }
        }

        #endregion Import

        [ActionDescription("Sys.Export")]
        [HttpPost]
        public IActionResult ExportExcel(FrameworkTenantListVM vm)
        {
            if (CanUseTenant() == false)
            {
                return Content(Localizer["_Admin.TenantNotAllowed"]);
            }
            return vm.GetExportData();
        }

        private bool CanUseTenant()
        {
            if (Wtm.LoginUserInfo != null && (Wtm.LoginUserInfo.CurrentTenant == null || Wtm.GlobaInfo.AllTenant.Any(x => x.TCode == Wtm.LoginUserInfo.CurrentTenant && x.Enabled == true && x.EnableSub == true)))
            {
                return true;
            }
            return false;
        }
    }
}