﻿using FluentValidation;
using System.ComponentModel.DataAnnotations;
using XUCore.Ddd.Domain.Commands;
using XUCore.Ddd.Domain.Exceptions;
using XUCore.Extensions;
using XUCore.NetCore.Data;

namespace XUCore.Template.EasyFreeSql.Applaction.User.User
{
    /// <summary>
    /// 密码修改命令
    /// </summary>
    public class UserUpdatePasswordCommand : UpdateCommand<long>
    {
        /// <summary>
        /// 旧密码
        /// </summary>
        [Required]
        public string OldPassword { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        public string NewPassword { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }

        public class Validator : CommandIdValidator<UserUpdatePasswordCommand, bool, long>
        {
            public Validator()
            {
                AddIdValidator();

                RuleFor(x => x.OldPassword).NotEmpty().MaximumLength(30).WithName("旧密码");
                RuleFor(x => x.NewPassword).NotEmpty().MaximumLength(30).WithName("新密码").NotEqual(c => c.OldPassword).WithName("新密码不能和旧密码相同");
            }
        }
    }
}