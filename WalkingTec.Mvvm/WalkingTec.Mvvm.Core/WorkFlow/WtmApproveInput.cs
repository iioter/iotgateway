namespace WalkingTec.Mvvm.Core.WorkFlow
{
    public class WtmApproveInput
    {
        public string Action { get; set; }
        public LoginUserInfo CurrentUser { get; set; }
        public string Remark { get; set; }
    }
}