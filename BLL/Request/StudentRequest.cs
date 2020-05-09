using BLL.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Request
{
    public class StudentRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int DepartmentId { get; set; }

    }

    public class StudentRequestValidator : AbstractValidator<StudentRequest>
    {
        private readonly IServiceProvider serviceProvider;

        public StudentRequestValidator(IServiceProvider serviceProvider)
        {

            RuleFor(x => x.Name).NotNull().NotEmpty().Length(0, 100);
            RuleFor(x => x.DepartmentId).NotNull().NotEmpty();
            RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress().MustAsync(EmailExist).WithMessage("Email Already Exist");
            this.serviceProvider = serviceProvider;
        }

        private async Task<bool> EmailExist(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return  false;
            }
            var studentService = serviceProvider.GetRequiredService<IStudentService>();
            return await studentService.IsEmailExist(email);
        }
    }
}
