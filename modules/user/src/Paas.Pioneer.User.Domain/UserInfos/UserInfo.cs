using System;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Paas.Pioneer.Domain;
using Paas.Pioneer.User.Domain.Shared.UserInfos;

namespace Paas.Pioneer.User.Domain.UserInfos;

/// <summary>
/// ΢��App
/// </summary>
[Comment("�����ֵ�")]
[Table("WeChat_App")]
[Index(nameof(UserId), Name = "IX_UserId")]
public class UserInfo : BaseEntity
{
    /// <summary>
    /// �û�id
    /// </summary>
    [Comment("�û�id")]
    [Column("UserId", TypeName = "char(36)")]
    public virtual Guid UserId { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [Comment("����")]
    [Column("NickName", TypeName = "varchar(50)")]
    [CanBeNull]
    public virtual string NickName { get; set; }

    /// <summary>
    /// �Ա�
    /// </summary>
    [Comment("�Ա�")]
    public virtual byte Gender { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [Comment("����")]
    [Column("NickName", TypeName = "varchar(50)")]
    [CanBeNull]
    public virtual string Language { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [Comment("����")]
    [Column("City", TypeName = "varchar(50)")]
    [CanBeNull]
    public virtual string City { get; set; }

    /// <summary>
    /// ʡ��
    /// </summary>
    [Comment("����")]
    [Column("Province", TypeName = "varchar(50)")]
    [CanBeNull]
    public virtual string Province { get; set; }

    /// <summary>
    /// ��
    /// </summary>
    [Comment("��")]
    [Column("Country", TypeName = "varchar(50)")]
    [CanBeNull]
    public virtual string Country { get; set; }

    /// <summary>
    /// ͷ��
    /// </summary>
    [Comment("ͷ��")]
    [Column("Country", TypeName = "varchar(50)")]
    [CanBeNull]
    public virtual string AvatarUrl { get; set; }

    public UserInfo()
    {

    }

    public UserInfo(Guid id,
        Guid? tenantId,
        Guid userId,
        UserInfoModel model)
    {
        Id = id;
        TenantId = tenantId;
        UserId = userId;
        UpdateInfo(model);
    }

    public void UpdateInfo(UserInfoModel model)
    {
        NickName = model.NickName;
        Gender = model.Gender;
        Language = model.Language;
        City = model.City;
        Province = model.Province;
        Country = model.Country;
        AvatarUrl = model.AvatarUrl;
    }
}
