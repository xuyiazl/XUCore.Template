namespace XUCore.Template.Mediator.Domain;

/// <summary>
/// 登录记录
/// </summary>
public class UserLoginRecordDto : DtoBase<UserLoginRecordDto>
{
    /// <summary>
    /// 用户id
    /// </summary>
    public long UserId { get; set; }
    /// <summary>
    /// 登录方式
    /// </summary>
    public string LoginWay { get; set; }
    /// <summary>
    /// 登录时间
    /// </summary>
    public DateTime LoginTime { get; set; }
    /// <summary>
    /// 登录ip
    /// </summary>s
    public string LoginIp { get; set; }
    /// <summary>
    /// 用户名字
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 用户手机
    /// </summary>
    public string Mobile { get; set; }
    /// <summary>
    /// 用户账号
    /// </summary>
    public string UserName { get; set; }
    /// <summary>
    /// 用户头像
    /// </summary>
    public string Picture { get; set; }
}
