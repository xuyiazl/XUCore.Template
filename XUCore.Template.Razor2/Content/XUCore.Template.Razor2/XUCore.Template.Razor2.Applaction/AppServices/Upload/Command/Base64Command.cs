namespace XUCore.Template.Razor2.Applaction.Upload
{
    public class Base64Command : Command<bool>
    {
        public string Base64 { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.IsValid;
        }

        public class Validator : CommandValidator<Base64Command>
        {
            public Validator()
            {
                RuleFor(x => x.Base64).NotEmpty().WithName("Base64");
            }
        }
    }
}
