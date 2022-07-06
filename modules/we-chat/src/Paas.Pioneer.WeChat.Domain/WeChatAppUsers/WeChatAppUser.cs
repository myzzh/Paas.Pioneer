using System;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Paas.Pioneer.Domain;
using Volo.Abp.Timing;

namespace Paas.Pioneer.WeChat.Domain.WeChatAppUsers;
/// <summary>
/// ΢��AppUser
/// </summary>
[Comment("΢��AppUser")]
[Table("WeChat_AppUser")]
[Index(nameof(WeChatAppId), Name = "IDX_WeChatAppId")]
[Index(nameof(UserId), Name = "IDX_UserId")]
public class WeChatAppUser : BaseEntity
{
    /// <summary>
    /// ΢��appId
    /// </summary>
    [Comment("΢��appId")]
    [Column("WeChatAppId", TypeName = "char(36)")]
    public virtual Guid WeChatAppId { get; set; }

    /// <summary>
    /// �û�Id
    /// </summary>
    [Comment("�û�Id")]
    [Column("UserId", TypeName = "char(36)")]
    public virtual Guid UserId { get; set; }

    /// <summary>
    /// UnionId
    /// </summary>
    [Comment("UnionId")]
    [Column("UnionId", TypeName = "varchar(150)")]
    [CanBeNull]
    public virtual string UnionId { get; set; }

    /// <summary>
    /// OpenId
    /// </summary>
    [Comment("OpenId")]
    [Column("OpenId", TypeName = "varchar(150)")]
    [NotNull]
    public virtual string OpenId { get; set; }

    /// <summary>
    /// SessionKey
    /// </summary>
    [Comment("SessionKey")]
    [Column("SessionKey", TypeName = "varchar(500)")]
    [CanBeNull]
    public virtual string SessionKey { get; set; }

    /// <summary>
    /// SessionKey�޸�ʱ��
    /// </summary>
    [Comment("SessionKey�޸�ʱ��")]
    public virtual DateTime? SessionKeyChangedTime { get; set; }

    protected WeChatAppUser()
    {
    }

    public WeChatAppUser(Guid id,
        Guid? tenantId,
        Guid weChatAppId,
        Guid userId,
        [CanBeNull] string unionId,
        [NotNull] string openId)
    {
        TenantId = tenantId;
        WeChatAppId = weChatAppId;
        UserId = userId;
        UnionId = unionId;
        OpenId = openId;
    }

    public void SetUnionId(string unionId)
    {
        UnionId = unionId;
    }

    public void SetOpenId(string openId)
    {
        OpenId = openId;
    }

    public void UpdateSessionKey([CanBeNull] string sessionKey, IClock clock)
    {
        if (SessionKey == sessionKey)
        {
            return;
        }

        SessionKey = sessionKey;
        SessionKeyChangedTime = clock.Now;
    }
}
