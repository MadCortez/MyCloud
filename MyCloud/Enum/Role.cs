using System.ComponentModel.DataAnnotations;

namespace MyCloud.Enum
{
    public enum Role
    {
        [Display(Name = "Пользователь")]
        User = 0,
        [Display(Name = "Модератор")]
        Moderator = 1,
        [Display(Name = "Админ")]
        Admin = 2,
    }
}
