using EventsWebApp.Data.Enum;
using EventsWebApp.Models;
using Microsoft.AspNetCore.Identity;

namespace EventsWebApp.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder) 
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope()) 
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                if (!context.Events.Any()) 
                {
                    context.Events.AddRange(new List<Event>()
                    {
                        new Event()
                        {
                            Title = "Fortnite Tournament",
                            Image = "https://cdn2.unrealengine.com/blade-2560x1440-2560x1440-d4e556fb8166.jpg",
                            Description = "Join the Tournament, all ages can come but the minors MUST come with a guardian.",
                            EventCategory = EventCategory.Gaming,
                            Address = new Address()
                            {
                                Street = "Lego Street",
                                City = "Leganio",
                                State = "Legolas"
                            }

                        },
                         new Event()
                        {
                            Title = "Casual Football Game",
                            Image = "https://assets-global.website-files.com/5ca5fe687e34be0992df1fbe/6235ea7fbaf601e8d3980228_boy-kicking-ball-on-football-field-2021-09-24-03-47-56-utc-min-min.jpg",
                            Description = "Casual Football Game, mostly 18+ students. Need more players",
                            EventCategory = EventCategory.Sports,
                            Address = new Address()
                            {
                                Street = "Ave.M 6",
                                City = "Rabat",
                                State = "Kamra"
                            }

                        }
                    });
                    context.SaveChanges();
                }

                if (!context.Clubs.Any())
                {
                    context.Clubs.AddRange(new List<Club>()
                    {
                        new Club()
                        {
                            Title = "Fortnite Club",
                            Image = "https://cdn2.unrealengine.com/blade-2560x1440-2560x1440-d4e556fb8166.jpg",
                            Description = "Join the Fortnite Club",
                            ClubCategory = ClubCategory.Gaming,
                            Address = new Address()
                            {
                                Street = "Lego Street",
                                City = "Leganio",
                                State = "Legolas"
                            }

                        },
                         new Club()
                        {
                            Title = "Football Club",
                            Image = "https://assets-global.website-files.com/5ca5fe687e34be0992df1fbe/6235ea7fbaf601e8d3980228_boy-kicking-ball-on-football-field-2021-09-24-03-47-56-utc-min-min.jpg",
                            Description = "Join the Football Club",
                            ClubCategory = ClubCategory.Sports,
                            Address = new Address()
                            {
                                Street = "Ave.M 6",
                                City = "Rabat",
                                State = "Kamra"
                            }

                        }
                    });
                    context.SaveChanges();
                }

            }
        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                string adminUserEmail = "teddysmithdeveloper@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        UserName = "teddysmithdev",
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                        Address = new Address()
                        {
                            Street = "123 Main St",
                            City = "Charlotte",
                            State = "NC"
                        }
                    };
                    await userManager.CreateAsync(newAdminUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                string appUserEmail = "user@etickets.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new AppUser()
                    {
                        UserName = "app-user",
                        Email = appUserEmail,
                        EmailConfirmed = true,
                        Address = new Address()
                        {
                            Street = "123 Main St",
                            City = "Charlotte",
                            State = "NC"
                        }
                    };
                    await userManager.CreateAsync(newAppUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }

    }
}
