using Microsoft.AspNetCore.Identity;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;

namespace Orders.Backend.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task<User> GetUserAsync(string email); //Le mando un mail y me devuelve un usuario

        Task<IdentityResult> AddUserAsync(User user, string password); //Le mando usuario y password y me devuelve um
                                                               //IdentityResult, esto es si pudo o no crear el usuario.

        Task CheckRoleAsync(string roleName);//Le paso el role,si no existe lo crea, si existe, no hace nada

        Task AddUserToRoleAsync(User user, string roleName);//Agrega un usuario a un role

        Task<bool> IsUserInRoleAsync(User user, string roleName); //Comprueba si un usuario pertenece a un role.


        Task<SignInResult> LoginAsync(LoginDTO model); //Metodo cuando nos loggeamos
        Task LogoutAsync(); //Método para desloggearnos.
        Task<User> GetUserAsync(Guid userId);//Método que me devuelve el usuario.
        Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword); //Método que me permite modificar la contraseña
        Task<IdentityResult> UpdateUserAsync(User user);//Método que permite actualizar el usuario.
        Task<string> GenerateEmailConfirmationTokenAsync(User user);
        Task<IdentityResult> ConfirmEmailAsync(User user, string token);
        Task<string> GeneratePasswordResetTokenAsync(User user);
        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);


    }

}
