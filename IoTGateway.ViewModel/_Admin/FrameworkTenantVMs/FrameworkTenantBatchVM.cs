using WalkingTec.Mvvm.Core;

namespace WalkingTec.Mvvm.Mvc.Admin.ViewModels.FrameworkTenantVMs
{
    public partial class FrameworkTenantBatchVM : BaseBatchVM<FrameworkTenant, FrameworkTenant_BatchEdit>
    {
        public FrameworkTenantBatchVM()
        {
            ListVM = new FrameworkTenantListVM();
            LinkedVM = new FrameworkTenant_BatchEdit();
        }
    }

    /// <summary>
    /// Class to define batch edit fields
    /// </summary>
    public class FrameworkTenant_BatchEdit : BaseVM
    {
        protected override void InitVM()
        {
        }
    }
}