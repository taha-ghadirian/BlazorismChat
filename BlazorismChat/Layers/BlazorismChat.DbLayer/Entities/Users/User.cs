using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorismChat.DbLayer.Entities.Users;

public record User
{
    public User()
    {
        RegisterTime = DateTime.Now;
    }

    public User(string userName) : this()
    {
        UserName = userName;
    }

    [Key] public int UserId { get; set; }

    #region Statics

    #region Username

    private string _userName;
    [Display(Name = "نام کاربری", Prompt = "نام کاربری")]
    [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
    [MinLength(3, ErrorMessage = "فیلد {0} باید حداقل {1} کاراکتر باشد.")]
    [MaxLength(60, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
    public string UserName
    {
        get => _userName;
        set => _userName = FixedUserName = value;
    }

    //----------------------------------------------------------------------------------------------

    private string _fixedUserName;
    [Display(Name = "نام کاربری صحیح")]
    [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
    [MinLength(3, ErrorMessage = "فیلد {0} باید حداقل {1} کاراکتر باشد.")]
    [MaxLength(60, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
    public string FixedUserName
    {
        get => _fixedUserName;
        private set => _fixedUserName = value.ToUpper().Trim();
    }

    #endregion

    #region Email

    private string _email;

    [EmailAddress(ErrorMessage = "ایمیل شما معتبر نمیباشد")]
    [Display(Name = "ایمیل", Prompt = "Name@Example.com")]
    [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
    [MinLength(3, ErrorMessage = "فیلد {0} باید حداقل {1} کاراکتر باشد.")]
    [MaxLength(100, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
    public string Email
    {
        get => _email;
        set => _email = FixedEmail = value;
    }

    //-----------------------------------------------------------------------------------------------------

    private string _fixedEmail;
    [Display(Name = "ایمیل صحیح")]
    [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
    [MinLength(3, ErrorMessage = "فیلد {0} باید حداقل {1} کاراکتر باشد.")]
    [MaxLength(60, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
    public string FixedEmail
    {
        get => _fixedEmail;
        private set => _fixedEmail = value.ToUpper().Trim();
    }

    #endregion

    public bool IsDeleted { get; set; }

    [Display(Name = "تایید ایمیل")]
    public bool IsEmailConfirmed { get; set; }

    [Display(Name = "تایید شماره تلفن")]
    public bool IsPhoneNumberConfirmed { get; set; }

    [Display(Name = "تاریخ ثبت نام")]
    public DateTime RegisterTime { get; set; }

    #endregion

    #region Personal Information

    [Display(Name = "نام", Prompt = "علی")]
    //[Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
    [MinLength(3, ErrorMessage = "فیلد {0} باید حداقل {1} کاراکتر باشد.")]
    [MaxLength(100, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
    public string FirstName { get; set; }

    [Display(Name = "نام خانوادگی", Prompt = "محمدی")]
    //[Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
    [MinLength(3, ErrorMessage = "فیلد {0} باید حداقل {1} کاراکتر باشد.")]
    [MaxLength(100, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
    public string LastName { get; set; }
    
    [Display(Name = "شماره همراه", Prompt = "09131234566")]
    //[Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
    [RegularExpression(@"^09[0-9]{9}$", ErrorMessage = "شماره تلفن وارد شده معتبر نیست")]
    [MaxLength(11, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
    public string PhoneNumber { get; set; }

    #endregion

    #region Security

    [Display(Name = "رمز عبور", Prompt = "رمز عبور")]
    [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
    [MinLength(8, ErrorMessage = "فیلد {0} باید حداقل {1} کاراکتر باشد.")]
    [MaxLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
    public string Password { get; set; }

    /// <summary>
    /// Change in every update
    /// </summary>
    public string IdentityCode { get; set; }

    /// <summary>
    /// Email Activation code.
    /// Change when Security Items edited.
    /// </summary>
    [Display(Name = "کد فعال سازی")]
    [MaxLength(50, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
    public string ActiveCode { get; set; }

    #endregion

    #region Relations
    public ICollection<UserRole> UserRoles { get; set; }
    #endregion

    #region Methods
    public override string ToString() => UserName;

    #endregion
}