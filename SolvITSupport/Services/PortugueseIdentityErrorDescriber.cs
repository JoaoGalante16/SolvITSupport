using Microsoft.AspNetCore.Identity;

namespace SolvITSupport.Services
{
    // Esta classe substitui as mensagens de erro padrão do Identity
    public class PortugueseIdentityErrorDescriber : IdentityErrorDescriber
    {
        // == MENSAGENS DE ERRO DE PASSWORD ==

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "A password deve conter pelo menos um caracter não alfanumérico (ex: !, @, #)." };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError { Code = nameof(PasswordRequiresUpper), Description = "A password deve conter pelo menos uma letra maiúscula ('A'-'Z')." };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError { Code = nameof(PasswordRequiresLower), Description = "A password deve conter pelo menos uma letra minúscula ('a'-'z')." };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError { Code = nameof(PasswordRequiresDigit), Description = "A password deve conter pelo menos um dígito ('0'-'9')." };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError { Code = nameof(PasswordTooShort), Description = $"A password deve ter pelo menos {length} caracteres." };
        }

        public override IdentityError PasswordMismatch()
        {
            // Esta é a mensagem de "Password inválida" no ecrã de Login
            return new IdentityError { Code = nameof(PasswordMismatch), Description = "Email ou password inválidos." };
        }


        // == MENSAGENS DE ERRO DE UTILIZADOR ==

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError { Code = nameof(DuplicateEmail), Description = $"O email '{email}' já está a ser utilizado." };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            // No seu caso, UserName e Email são iguais
            return new IdentityError { Code = nameof(DuplicateUserName), Description = $"O email '{userName}' já está a ser utilizado." };
        }

        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError { Code = nameof(InvalidUserName), Description = $"O nome de utilizador '{userName}' é inválido. Só pode conter letras ou dígitos." };
        }

        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError { Code = nameof(InvalidEmail), Description = $"O email '{email}' é inválido." };
        }


        // == MENSAGENS DE ERRO DE PERFIL (ROLE) ==

        public override IdentityError UserAlreadyInRole(string role)
        {
            return new IdentityError { Code = nameof(UserAlreadyInRole), Description = $"O utilizador já pertence ao perfil '{role}'." };
        }

        // Adicione aqui qualquer outra tradução que precisar,
        // basta escrever 'public override' e o Visual Studio/VSCode irá sugerir os métodos.
    }
}