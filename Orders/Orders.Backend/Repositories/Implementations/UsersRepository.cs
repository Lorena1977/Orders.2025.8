using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.Repositories.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;

namespace Orders.Backend.Repositories.Implementations
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly SignInManager<User> _signInManager;

        public UsersRepository(DataContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
        {
            _context = context; //Acceso a base de datos
            _userManager = userManager; //Provee los métodos para acceder a usuarios
            _roleManager = roleManager; //Provee los métodos necesarios para manejar roles
            _signInManager = signInManager; //Provee los métodos necesarios para Loggearse
        }

        //Método para adicionar un usuario
        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password); //Llamo al metodo CreateAsyn del userManager.
        }

        //Asocia un usuario a un Role
        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);//Llamo al metodo AddToRoleAsync del userManager.
        }

        //Método que valida el Role.
        public async Task CheckRoleAsync(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);//Si existe el role, devuelve true, sino, lo crea.
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }

        //Método que obtiene el usuario. Pero queremos que nos de el usuario asociado a la ciudad
        // aquí es donde necesito el Datacontext.
        public async Task<User> GetUserAsync(string email)
        {
            var user = await _context.Users
                .Include(u => u.City!) //Que el usuario me incluya la ciudad
                .ThenInclude(c => c.State!) //Que me incluya el estado
                .ThenInclude(s => s.Country) //Que me incluya el Pais
                .FirstOrDefaultAsync(x => x.Email == email);
            return user!;
        }

        //Método que devuelve si un usuario está asociado a un rol concreto.
        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }


        //Método que le pasamos el mail y la contraseña. EL último parámetro me devuelve si voy a bloquear el usuario 
        // por intentos fallidos
        public async Task<SignInResult> LoginAsync(LoginDTO model)
        {
            return await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, true);
        }

        //Método que se desloggea.
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        //Método que me obtiene el usuario a partir del Guid (numero) asociado al userId que nunca se repite.
        //Esto nos servirá cuando tengamos que confirmar el usuario.
        public async Task<User> GetUserAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.City!)
                .ThenInclude(c => c.State!)
                .ThenInclude(s => s.Country)
                .FirstOrDefaultAsync(x => x.Id == userId.ToString());
            return user!;
        }

        //Método que me permite cambiar la password (utilizando el userManager).
        public async Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        //Método que me permite actualizar el usuario (usando también el userManager)
        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        //Método que genera el mail de confirmación
        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        //Método que confirma el mail.
        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }


        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(user, token, password);
        }




    }
}
